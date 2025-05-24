using Core.QFrameWork;
using Player.PlayerManager;
using QFramework;

namespace Player.Player
{
    public class CameraToPlayer : BaseController
    {
        private IPlayerModel mPlayerModel;

        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
        }

        private void Start()
        {
            if (mPlayerModel.CurrentControlPlayer != null)
            {
                transform.SetParent(mPlayerModel.CurrentControlPlayer.Owner.transform, false);
            }
        }
    }
}