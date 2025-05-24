using NetWorkSystem;
using QFramework;

namespace Command
{
    public class CreateRoomCommand : AbstractCommand
    {
        
        private int mPort;
        
        public CreateRoomCommand(int port)
        {
            mPort = port;
        }
        
        protected override void OnExecute()
        {
            this.SendEvent<CreateRoomEvent>(new CreateRoomEvent()
            {
                port = mPort
            });
        }
    }
}