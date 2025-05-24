using System;
using Common;
using Core.QFrameWork;
using LevelSystem;
using Player.PlayerManager;
using QFramework;
using SceneSystem;
using StateMachine;
using UI.UICore;
using UnityEngine;

namespace NetWorkSystem
{
    public class NetWorkManager : BaseController
    {
        private INetWorkModel mNetWorkModel;

        private StateMachineController mStateMachineController;
        
        private void Awake()
        {
            mNetWorkModel = this.GetModel<INetWorkModel>();
            mStateMachineController = GetComponent<StateMachineController>();
            //this.RegisterEvent<InitNetWorkEvent>(OnInitNetWork).UnRegisterWhenDisabled(this);
        }

        private void Start()
        {
            mStateMachineController.AddState("UDPServerCreateState", new UDPServerCreateState());
            mStateMachineController.AddState("WaitNetWorkState", new WaitNetWorkState());
            //mStateMachineController.AddState();
            
            mStateMachineController.SetDefaultState("WaitNetWorkState");
        }

        private class WaitNetWorkState : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();
                this.RegisterEvent<CreateRoomEvent>(OnRoomCreate);
            }

            private void OnRoomCreate(CreateRoomEvent obj)
            {
                this.GetSystem<INetWorkSystem>().CreateUpdServer(obj.port);
                //创建房间默认本地服务器
                this.GetSystem<INetWorkSystem>().CreateUdpClient(NetWorkUtil.ServerDefaultIpAddress, obj.port);
                //this.GetSystem<INetWorkSystem>().UpgradeUdpToKcp();
            }
        }
        
        private class UDPServerCreateState : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();

            }
        }
        
        private void OnInitNetWork(InitNetWorkEvent @event)
        {
            
        }
        
        
        private void FixedUpdate()
        {
            //可以单开另外一个线程
            if(mNetWorkModel?.Msgs == null) return;

            lock (mNetWorkModel.Msgs)
            {
                if (mNetWorkModel.Msgs.Count > 0)
                {
                    ResolveMsg(mNetWorkModel.Msgs.Dequeue());
                }
            }
        }

        //TODO 增加网络信息
        private void ResolveMsg(string jsonData)
        {
            var baseMsg = JsonUtility.FromJson<BaseMsg>(jsonData);
            if (baseMsg == null) return;
            
            switch (baseMsg.MsgType)
            {
                case MsgType.StartGameSuccess:
                    this.GetModel<IPlayerModel>().DoCreateNetWorkPlayer = true;
                    this.GetSystem<ISceneSystem>().LoadGameScene(1001, 0);
                    this.GetSystem<IUISystem>().SetAllUIHide();
                    break;
                case MsgType.PlayerMoveSpeed:
                    var playerMoveSpeedJson = JsonUtility.FromJson<PlayerMoveSpeedMsg>(jsonData);
                    //多玩家要ID的，这里直接赋值给另外的
                    foreach (var playerUnit in this.GetModel<IPlayerModel>().OtherPlayer)
                    {
                        playerUnit.Direction = playerMoveSpeedJson.MoveVelocity;
                    }
                    break;
            }
        }
    }
}