using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using Random = System.Random;

namespace NetWorkSystem.Kcp
{
    
    /// <summary>
    /// 首先使用UDP进行连接第一次握手，
    /// 再成功收到连接成功的消息后，服务端返回Conv参数并记录连接的IP这是第二次握手。
    /// 第三次握手 客户端接收到conv后再次返回conv，服务端升级。(过时)
    /// 服务器不需要升级，只有客户端要升级
    /// 所以服务器可以直接为Kcp，因为要先获取udp的消息传给kcp
    /// 客户端升级的消息在这中间去处理就可以了。
    /// 只有在三次握手之后才把客户端对应的Conv加入进来。
    /// </summary>
    public class KcpServer : IController
    {
        private bool mIsRunning;
        
        private UdpClient mUdpClient;
        private HashSet<uint> RecordConvs = new();
        private Dictionary<uint, KcpCallback> mKcpSession = new();
        private Dictionary<IPEndPoint, KcpCallback> mKcpSessionIpMap = new();

        private Random mRandom = new ();
        
        private Queue<string> mMsgs = new();

        public async void Start(int port)
        {
            mUdpClient = new UdpClient(port);
            
            //直接跑流程
            mIsRunning = true;
            
            //首先Udp处理数据，主要处理连接请求，或者将数据发给kcp。
            RunUdpReceive();
            
            KcpProcessLoop();
            await UniTask.RunOnThreadPool(UpdateKcp);

            await UniTask.RunOnThreadPool(RunHandleKcpClientMsg);
        }

        public void Close()
        {
            mIsRunning = false;
            mUdpClient.Close();
            foreach (var kcpCallback in mKcpSession)
            {
                kcpCallback.Value.Close();
            }
        }
        
        private async void RunUpgradeReceive()
        {
            while (mIsRunning)
            {
                try
                {
                    var result = await mUdpClient.ReceiveAsync();
                    
                    //第一次握手
                    HandleUpgradeRequest(result);
                    //第三次握手
                    HandleUpgradeSuccess(result);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        //第一次握手成功 服务端发送Conv给客户端
        private void HandleUpgradeRequest(UdpReceiveResult result)
        {
            var json = System.Text.Encoding.UTF8.GetString(result.Buffer);
            var baseMsg = JsonUtility.FromJson<BaseMsg>(json);
            if (baseMsg.MsgType != MsgType.UpgradeRequestC2S) return;
            
            // 生成服务端conv（确保唯一性）
            uint serverConv = (uint)mRandom.Next(1, int.MaxValue);
            while (mKcpSession.ContainsKey(serverConv))
            {
                serverConv = (uint)mRandom.Next(1, int.MaxValue);
            }
            
            var sendJson = JsonUtility.ToJson(new UpgradeSuccessMsg()
            {
                Conv = serverConv,
                MsgType = MsgType.UpgradeSuccessS2C
            });
            var data = System.Text.Encoding.UTF8.GetBytes(sendJson);
            mUdpClient.SendAsync(data, data.Length, result.RemoteEndPoint);
        }

        //重写第一次握手逻辑
        private void HandleUpgradeRequest(IPEndPoint ipEndPoint)
        {
            // 生成服务端conv（确保唯一性）
            uint serverConv = (uint)mRandom.Next(1, int.MaxValue);
            while (mKcpSession.ContainsKey(serverConv))
            {
                serverConv = (uint)mRandom.Next(1, int.MaxValue);
            }
            
            //记录生成的conv
            RecordConvs.Add(serverConv);
            
            var sendJson = JsonUtility.ToJson(new UpgradeSuccessMsg()
            {
                Conv = serverConv,
                MsgType = MsgType.UpgradeSuccessS2C
            });
            var data = System.Text.Encoding.UTF8.GetBytes(sendJson);
            mUdpClient.SendAsync(data, data.Length, ipEndPoint);
        }
        
        //第三次握手 处理接收到客户端返回的Conv 如果一致则握手成功. 生成对应的Kcp会话.
        private void HandleUpgradeSuccess(UdpReceiveResult result)
        {
            var json = System.Text.Encoding.UTF8.GetString(result.Buffer);
            var baseMsg = JsonUtility.FromJson<BaseKcpMsg>(json);
            if (baseMsg.MsgType != MsgType.UpgradeEndC2S) return;
            if (mKcpSession.TryGetValue(baseMsg.Conv, out var value) == false)
            {
                //将udp发送交给kcp解析
                value = new KcpCallback(baseMsg.Conv, buffer =>
                {
                    mUdpClient.SendAsync(buffer, buffer.Length, result.RemoteEndPoint);
                });
                mKcpSession.TryAdd(baseMsg.Conv, value);
                mKcpSessionIpMap.TryAdd(result.RemoteEndPoint, value);
            }
            
        }

        //重写第三次握手逻辑
        private void HandleUpgradeSuccess(uint conv, IPEndPoint ipEndPoint)
        {
            //如果找到了记录的Conv 连接就成功了
            //否则就发送一个连接失败的消息
            if (RecordConvs.Contains(conv) == false)
            {
                var msg = new BaseKcpMsg()
                {
                    MsgType = MsgType.UpgradeFailure
                };
                var baseMsg = JsonUtility.ToJson(msg);
                var jsonBytes = System.Text.Encoding.UTF8.GetBytes(baseMsg);
                mUdpClient.SendAsync(jsonBytes, jsonBytes.Length, ipEndPoint);
            }
            
            //找到了就把记录删了
            RecordConvs.Remove(conv);
            
            //建立KCP连接
            if (mKcpSession.TryGetValue(conv, out var value) == false)
            {
                //将udp发送交给kcp解析
                value = new KcpCallback(conv, buffer =>
                {
                    mUdpClient.SendAsync(buffer, buffer.Length, ipEndPoint);
                });
                mKcpSession.TryAdd(conv, value);
                mKcpSessionIpMap.TryAdd(ipEndPoint, value);
            }
        }

        private async void UpdateKcp()
        {
            while (mIsRunning)
            {
                try
                {
                    foreach (var kvp in mKcpSession)
                    {
                        kvp.Value?.Update();
                    }
                    await UniTask.Delay(10);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }

        }

        private async void RunUdpReceive()
        {
            while (mIsRunning)
            {
                try
                {
                    var udpResult = await mUdpClient.ReceiveAsync();

                    //这里先处理连接请求：先判断是不是请求，不是就拿出Conv，如果Conv没有的话就不处理了。如果有就交给Kcp。
                    //注意使用json序列化所以传递kcp消息的时候注意避免json序列化失败
                    //不好处理，只能先看是否合规json否则就交给kcp。后续将json转化为protobuff等序列化方式。
                    var json = System.Text.Encoding.UTF8.GetString(udpResult.Buffer);
                    if (IsValidJsonSyntax(json) == true)
                    {
                        var baseMsg = JsonUtility.FromJson<BaseKcpMsg>(json);

                        if (baseMsg.MsgType == MsgType.UpgradeRequestC2S)
                        {
                            HandleUpgradeRequest(udpResult.RemoteEndPoint);

                            //消息就不发给kcp了
                            continue;
                        }

                        //第三次握手
                        if (baseMsg.MsgType == MsgType.UpgradeEndC2S)
                        {
                            HandleUpgradeSuccess(baseMsg.Conv, udpResult.RemoteEndPoint);
                            continue;
                        }
                    }
                    
                    if (mKcpSessionIpMap.TryGetValue(udpResult.RemoteEndPoint, out KcpCallback callback))
                    {
                        callback.Input(udpResult.Buffer);

                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
        }
        
        private async void KcpProcessLoop()
        {
            while (mIsRunning)
            {
                try
                {
                    foreach (var callback in mKcpSessionIpMap.Values)
                    {
                        var result = await callback.ReceiveKcpAsync();
                        var json = System.Text.Encoding.UTF8.GetString(result);
                        HandleKcpMsg(json);
                    }

                    await UniTask.Delay(10);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (NullReferenceException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

            }
        }
        
        /// <summary>
        /// 将信息放到队列中让另外的线程处理
        /// </summary>
        /// <param name="json"></param>
        private void HandleKcpMsg(string json)
        {
            lock (mMsgs)
            {
                mMsgs.Enqueue(json);
            }
        }

        private void RunHandleKcpClientMsg()
        {
            while (mIsRunning)
            {
                try
                {
                    lock (mMsgs)
                    {
                        if (mMsgs.Count > 0)
                        {
                            ResolvedKcpMsg();
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// 解析客户端发来的消息
        /// </summary>
        private void ResolvedKcpMsg()
        {
            var json = mMsgs.Dequeue();
            var baseMsg = JsonUtility.FromJson<BaseKcpMsg>(json);

            if(mKcpSession.TryGetValue(baseMsg.Conv, out var value) == false)return;
            switch (baseMsg.MsgType)
            {
                case MsgType.Ping:
                    var msg = JsonUtility.FromJson<PingMsg>(json);
                    var data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(msg));
                    value.SendKcpAsync(data, data.Length);
                    break;
                case MsgType.StartGameRequest:
                    //判断连接的是否是两个及两个以上
                    if (mKcpSession.Count >= 2)
                    {
                        //向前两个发送消息
                        var startGameSuccessMsg = JsonUtility.ToJson(new BaseKcpMsg()
                        {
                            MsgType = MsgType.StartGameSuccess
                        });
                        var startGameSuccessData = System.Text.Encoding.UTF8.GetBytes(startGameSuccessMsg);
                        foreach (var kvp in mKcpSession)
                        {
                            kvp.Value.SendKcpAsync(startGameSuccessData, startGameSuccessData.Length);
                        }
                    }
                    break;
                case MsgType.PlayerMoveSpeed:
                    //接收到A客户端发送的自己的移动消息，交给B客户端
                    foreach (var kvp in mKcpSession)
                    {
                        if (kvp.Key == baseMsg.Conv)
                        {
                            continue;
                        }
                        var playerMoveSpeedMsg = JsonUtility.FromJson<PingMsg>(json);
                        var playerMoveSpeedData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerMoveSpeedMsg));
                        kvp.Value.SendKcpAsync(playerMoveSpeedData, playerMoveSpeedData.Length);
                    }
                    break;
            }
        }

        private bool IsValidJsonSyntax(string json) 
        {
            // 快速检查首尾字符是否为 {} 或 []
            json = json.Trim();
            if (!(json.StartsWith("{") && json.EndsWith("}") || 
                  json.StartsWith("[") && json.EndsWith("]"))) 
            {
                return false;
            }
            // 正则表达式粗略检查关键符号平衡（可选）
            var openBraces = Regex.Matches(json, "{").Count;
            var closeBraces = Regex.Matches(json, "}").Count;
            return openBraces == closeBraces; // 大括号数量需匹配
        }
        
        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;;
        }
    }
}