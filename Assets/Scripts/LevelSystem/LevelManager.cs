using AssetSystem;
using Core.QFrameWork;
using InputSystem;
using Player.PlayerManager;
using QFramework;
using UnityEngine;

namespace LevelSystem
{
    public class LevelManager : BaseController
    {
        private ILevelModel mLevelModel;
        private IAddressableModel mAddressableModel;
        private IPlayerModel mPlayerModel;
        private IPlayerSystem mPlayerSystem;
        
        private void Awake()
        {
            mLevelModel = this.GetModel<ILevelModel>();
            mAddressableModel = this.GetModel<IAddressableModel>();
            mPlayerModel = this.GetModel<IPlayerModel>();
            mPlayerSystem = this.GetSystem<IPlayerSystem>();
        }

        private void Start()
        {
            mLevelModel.CurrentLevelMap = Instantiate(mLevelModel.CurrentLevelDataModel.MapPrefab);
            var map = mLevelModel.CurrentLevelMap.GetComponent<LevelMapController>();
            var player = mPlayerSystem.CreateSelfPlayerById(0);
            player.SetPlayerUnitPosition(map.Player1StartPositions.position);

            if (mPlayerModel.DoCreateNetWorkPlayer == true)
            {
                var player2 = mPlayerSystem.CreatePlayerById(0);
                player2.SetPlayerUnitPosition(map.Player2StartPositions.position);
                mPlayerModel.OtherPlayer.Add(player2);
            }
            this.GetSystem<IInputControllerSystem>().BindControllerToUnit(player);
        }
    }
}