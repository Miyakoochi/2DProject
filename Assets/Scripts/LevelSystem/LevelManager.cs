﻿using AssetSystem;
using Core.QFrameWork;
using InputSystem;
using Player.PlayerManager;
using QFramework;
using UI.UICore;
using UnityEngine;

namespace LevelSystem
{
    public class LevelManager : BaseController
    {
        private ILevelModel mLevelModel;
        private IAddressableModel mAddressableModel;
        private IPlayerModel mPlayerModel;
        private IPlayerSystem mPlayerSystem;

        public bool IsBoundCamera;
        
        private void Awake()
        {
            mLevelModel = this.GetModel<ILevelModel>();
            mAddressableModel = this.GetModel<IAddressableModel>();
            mPlayerModel = this.GetModel<IPlayerModel>();
            mPlayerSystem = this.GetSystem<IPlayerSystem>();
        }

        private void Start()
        {
            this.GetSystem<IUISystem>().FadeOut();
            mLevelModel.CurrentLevelMap = Instantiate(mLevelModel.CurrentLevelDataModel.MapPrefab);
            var map = mLevelModel.CurrentLevelMap.GetComponent<LevelMapController>();

            mLevelModel.BoundCamera = IsBoundCamera;
            if (IsBoundCamera)
            {
                mLevelModel.LevelBoundLeftDown = map.BoundCameraLeftDown.position;
                mLevelModel.LevelBoundRightUp = map.BoundCameraRightUp.position;
            }
            
            var player = mPlayerSystem.CreateSelfPlayerById(0);
            player.SetPlayerUnitPosition(map.Player1StartPositions.position);

            //TODO 可能执行两次
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