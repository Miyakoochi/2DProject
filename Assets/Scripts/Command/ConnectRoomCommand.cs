using NetWorkSystem;
using QFramework;

namespace Command
{
    public class ConnectRoomCommand : AbstractCommand
    {
        private string mIp;
        private int mPort;
        
        public ConnectRoomCommand(string ip, int port)
        {
            mIp = ip;
            mPort = port;
        }
        
        protected override void OnExecute()
        {
            this.SendEvent<ConnectRoomEvent>(new ConnectRoomEvent()
            {
                ip = mIp,
                port = mPort
            });
        }
    }
}