using System;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using NetWorkSystem.Kcp;
using QFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace NetWorkSystem
{
    public class HttpResult
    {
        public string Result;
        public long ResponseCode;

        public HttpResult(string result, long code)
        {
            Result = result;
            ResponseCode = code;
        }
    }
    
    public interface INetWorkSystem : ISystem
    {
        /// <summary>
        /// 网络处理线程调用
        /// </summary>
        /// <param name="jsonData"></param>
        public void ResponseMessage(string jsonData);

        /// <summary>
        /// 基于HTTP协议 传输层TCP
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public UniTask<HttpResult> Req(string url, string method, string token, byte[] body);

        /// <summary>
        /// 创建UDP服务器
        /// </summary>
        /// <param name="port"></param>
        public void CreateUpdServer(int port);

        public void CreateUdpClient(string ip, int port);

        /// <summary>
        /// 检查Ip地址是否合乎规范
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsValidIP(string ip);

        
        public bool IsValidPort(string portStr);

        public void CloseNetWork();

    }
    
    public class NetWorkSystem : AbstractSystem, INetWorkSystem
    {
        private int mUpgradeCount = 10;
        
        private INetWorkModel mNetWorkModel;
        
        
        public void ResponseMessage(string jsonData)
        {
            if(mNetWorkModel.Msgs == null) return;
            
            lock (mNetWorkModel.Msgs)
            {
                mNetWorkModel.Msgs.Enqueue(jsonData);
            }
        }
        
        public async UniTask<HttpResult> Req(string url, string method, string token, byte[] body) 
        {
            try
            {
                var request = new UnityWebRequest(url, method);
                request.timeout = 10;

                if (method == "POST" && body != null) 
                {
                    UploadHandler handler = new UploadHandlerRaw(body);
                    handler.contentType = "application/json";
                    request.useHttpContinue = false;
                    request.uploadHandler = handler;
                }

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");
                request.disposeUploadHandlerOnDispose = true;
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeCertificateHandlerOnDispose = true;
                if (!string.IsNullOrEmpty(token))
                {
                    request.SetRequestHeader("Authorization", token);
                }

                request.downloadHandler = new DownloadHandlerBuffer();

                var response = await request.SendWebRequest().WithCancellation(default);
                
                //UIController.Instance.HidePage(UIPageType.LoadingUI);
                if (response.result == UnityWebRequest.Result.ConnectionError || response.responseCode != 200)
                {
                    return new HttpResult(null, response.responseCode);
                }

                byte[] results = request.downloadHandler.data;
                string s = Encoding.UTF8.GetString(results);
                return new HttpResult(s, response.responseCode);
            } 
            catch (Exception e) 
            {
                Debug.LogError("http" + e);
                //UIController.Instance.HidePage(UIPageType.LoadingUI);
                if (e is UnityWebRequestException exception)
                {
                    return new HttpResult(null, exception.ResponseCode);
                }

                return new HttpResult(null, -1);
            }
        }

        public void CreateUpdServer(int port)
        {
            mNetWorkModel.LocalKcpServer = new KcpServer();
            mNetWorkModel.LocalKcpServer.Start(port);
        }

        public void CreateUdpClient(string ip, int port)
        {
            mNetWorkModel.LocalKcpClient = new KcpClient();
            mNetWorkModel.LocalKcpClient.ConnectAsync(ip, port);
            mUpgradeCount = 10;
        }
        
        public void CloseNetWork()
        {
            if (mNetWorkModel.LocalKcpClient != null)
            {
                mNetWorkModel.LocalKcpClient.Close();
            }

            if (mNetWorkModel.LocalKcpServer != null)
            {
                mNetWorkModel.LocalKcpServer.Close();
            }
        }
        
        public void SendToServer<T>(T msg) where T : BaseMsg
        {
            
        }
        
        public bool IsValidIP(string ip)
        {
            // 分割字符串并检查基本格式
            string[] parts = ip.Split('.');
            if (parts.Length != 4) return false;
            foreach (string part in parts)
            {
                // 检查空字符串和非数字内容
                if (string.IsNullOrEmpty(part)) return false;
                if (part.StartsWith("-")) return false; // 排除负数
        
                // 检查前导零
                if (part.Length > 1 && part[0] == '0') return false;
                // 验证是否为有效数字
                if (!int.TryParse(part, out int num)) return false;
        
                // 检查数值范围
                if (num < 0 || num > 255) return false;
            }
            return true;
        }
        
        public bool IsValidPort(string portStr)
        {
            // 空值检测
            if (string.IsNullOrWhiteSpace(portStr)) return false;
            // 检查纯数字组成
            if (!portStr.All(char.IsDigit)) return false;
            // 前导零检测（允许单个0，禁止类似"0123"）
            if (portStr.Length > 1 && portStr[0] == '0') return false;
            // 转换为数值并验证范围
            if (!int.TryParse(portStr, out int port)) return false;
            return port >= 0 && port <= 65535;
        }
        
        protected override void OnInit()
        {
            mNetWorkModel = this.GetModel<INetWorkModel>();
        }
    }
}