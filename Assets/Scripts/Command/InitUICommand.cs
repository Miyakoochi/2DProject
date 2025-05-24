using QFramework;
using UI.UICore;

namespace Command
{
    public class InitUICommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<InitUIEvent>();
        }
    }
}