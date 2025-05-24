using Core.QFrameWork;
using NetWorkSystem.Kcp;

namespace NetWorkSystem.TestUdp
{
    public class MyKcpServer : BaseController
    {

        private KcpServer Server;

        private void Awake()
        {
            Server = new KcpServer();
            Server.Start(7777);
        }

        private void Start()
        {
        }

        private void OnDestroy()
        {
            Server.Close();
        }
    }
}