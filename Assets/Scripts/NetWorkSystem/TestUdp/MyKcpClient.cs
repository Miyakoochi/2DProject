using Common;
using Core.QFrameWork;
using NetWorkSystem.Kcp;
using UnityEngine;

namespace NetWorkSystem.TestUdp
{
    public class MyKcpClient : BaseController
    {
        private KcpClient mKcpClient;

        private void Start()
        {
            mKcpClient = new KcpClient();
            mKcpClient.ConnectAsync(NetWorkUtil.ServerDefaultIpAddress, 7777);
        }

        public void Send()
        {
            Debug.Log("Send");
            //mKcpClient.Send("你好你好你好");
        }

        public long GetRTT()
        {
            return mKcpClient.GetRTT();
        }
        
        private void OnDestroy()
        {
            mKcpClient.Close();
        }
    }
}