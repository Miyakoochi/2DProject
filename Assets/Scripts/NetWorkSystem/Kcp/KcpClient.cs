using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Command;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace NetWorkSystem.Kcp
{
    public class KcpClient : IController
    {
        private KcpCallback mKcpCallback;
        private UdpClient mUdpClient;

        private bool mIsRunning;
        private IPEndPoint mServer;
        private long mLastPingTime = 0;
        private long mCurrentRtt = 0;
        private Stopwatch mRttTimeWatch = new Stopwatch();

        public uint SelfConv = 0;

        public async void ConnectAsync(string serverIp, int serverPort)
        {
            mUdpClient = new UdpClient(0);
            mServer = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            
            
            var upgradeMsg = new UpgradeRequestMsg()
            {
                MsgType = MsgType.UpgradeRequestC2S
            };
            var bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(upgradeMsg));
            
            await mUdpClient.SendAsync(bytes, bytes.Length, mServer);
            
            var result = await mUdpClient.ReceiveAsync();
            
            var baseKcpMsg = JsonUtility.FromJson<BaseKcpMsg>(System.Text.Encoding.UTF8.GetString(result.Buffer));
            if (baseKcpMsg.MsgType != MsgType.UpgradeSuccessS2C) return;

            SelfConv = baseKcpMsg.Conv;
            var upgradeEndMsgMsg = new UpgradeEndMsg()
            {
                Conv = baseKcpMsg.Conv,
                MsgType = MsgType.UpgradeEndC2S
            };
            var endByte = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(upgradeEndMsgMsg));
            await mUdpClient.SendAsync(endByte, endByte.Length, mServer);

            mKcpCallback = new KcpCallback(SelfConv, bytes1 =>
            {
                mUdpClient.SendAsync(bytes1, bytes1.Length, mServer);
            });
            mIsRunning = true;
            
            this.SendCommand<ClientConnectSuccessCommand>();
            
            StartKcp();
        }

        public void Close()
        {
            mIsRunning = false;
            mUdpClient.Close();
            mKcpCallback.Close();
        }

        public void Send(string json)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(json);
            mKcpCallback.SendKcpAsync(data, data.Length);
        }
        
        private async void StartKcp()
        {
            mRttTimeWatch.Start();

            await UniTask.RunOnThreadPool(UpdateKcp);
            RunUdpReceive();
            RunKcpReceive();
            PingKcp();
        }

        private async void UpdateKcp()
        {
            while (mIsRunning)
            {
                try
                {
                    mKcpCallback.Update();
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

        public long GetRTT()
        {
            return mCurrentRtt;
        }
        
        private async void RunUdpReceive()
        {
            while (mIsRunning)
            {
                try
                {
                    var udpResult = await mUdpClient.ReceiveAsync();
                    mKcpCallback.Input(udpResult.Buffer);
                }
                catch(ObjectDisposedException)
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

        private async void RunKcpReceive()
        {
            while (mIsRunning)
            {
                try
                {
                    var result = await mKcpCallback.ReceiveKcpAsync();
                    var json = System.Text.Encoding.UTF8.GetString(result);
                    //TODO 执行处理方法
                    HandleKcpPing(json);
                    this.GetSystem<INetWorkSystem>().ResponseMessage(json);
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
                    throw;
                }
            }
        }
        
        private void SendKcp(string json)
        {
            var endByte = System.Text.Encoding.UTF8.GetBytes(json);
            mKcpCallback.SendKcpAsync(endByte, endByte.Length);
        }
        
        private async void PingKcp()
        {
            while (mIsRunning)
            {
                try
                {
                    await UniTask.Delay(1000);
                    mLastPingTime = mRttTimeWatch.ElapsedMilliseconds;
                    var json = JsonUtility.ToJson(new PingMsg()
                    {
                        MsgType = MsgType.Ping,
                        PingTime = mLastPingTime,
                        Conv = SelfConv
                    });
                    SendKcp(json);
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

                //mLastPingTime = mRttTimeWatch.ElapsedMilliseconds;
            }
        }

        private void HandleKcpPing(string json)
        {
            var baseMsg = JsonUtility.FromJson<BaseKcpMsg>(json);
            if (baseMsg.MsgType == MsgType.Ping)
            {
                var pingMsg = JsonUtility.FromJson<PingMsg>(json);
                var now = mRttTimeWatch.ElapsedMilliseconds;
                mCurrentRtt = now - pingMsg.PingTime;
            }
            //mCurrentRtt = mRttTimeWatch.ElapsedMilliseconds - mLastPingTime;
            //Debug.Log($"Ping: {mCurrentRtt} ms");
        }

        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }
    }
    
    
}