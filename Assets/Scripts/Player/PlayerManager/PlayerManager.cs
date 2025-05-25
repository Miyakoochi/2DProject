using Core.QFrameWork;
using Player.Player;
using QFramework;
using UnityEngine;

namespace Player.PlayerManager
{
    public class PlayerManager : BaseController
    {
        private IPlayerModel mPlayerModel;

        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
        }

        private void Update()
        {
            mPlayerModel.CurrentControlPlayer?.UpdateAnimator();
            foreach (var playerUnit in mPlayerModel.OtherPlayer)
            {
                playerUnit?.UpdateAnimator();
            }
        }

        private void FixedUpdate()
        {
            mPlayerModel.CurrentControlPlayer?.UpdateVelocity();
            foreach (var playerUnit in mPlayerModel.OtherPlayer)
            {
                playerUnit?.UpdateVelocity();
            }
        }
    }
}