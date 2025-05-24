using NetWorkSystem;
using QFramework;

namespace Command
{
    /// <summary>
    /// Controller不能发送事件只能交给Command去处理
    /// </summary>
    public class ClientConnectSuccessCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ClientConnectSuccessEvent>();
        }
    }
}