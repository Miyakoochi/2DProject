using Player.Player;
using QFramework;

namespace Command
{
    public class FireCameraShake : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<PlayerFireEvent>();
        }
    }
}