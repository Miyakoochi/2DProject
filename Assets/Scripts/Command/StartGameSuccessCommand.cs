using NetWorkSystem;
using Player.PlayerManager;
using QFramework;
using SceneSystem;
using UI.UICore;

namespace Command
{
    public class StartGameSuccessCommand : AbstractCommand
    {
        private static bool HasStart = false;
        protected override void OnExecute()
        {
            if (HasStart == false)
            {
                this.SendEvent(new StartGameSuccessEvent());
                this.GetModel<IPlayerModel>().DoCreateNetWorkPlayer = true;
                this.GetSystem<ISceneSystem>().LoadGameScene(1001, 0);
                this.GetSystem<IUISystem>().SetAllUIHide();
                HasStart = true;
            }
        }
    }
}