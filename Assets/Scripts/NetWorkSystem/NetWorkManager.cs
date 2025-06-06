﻿using System;
using Command;
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
            DontDestroyOnLoad(gameObject);
            
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
                this.RegisterEvent<ConnectRoomEvent>(OnRoomConnect);
            }

            private void OnRoomConnect(ConnectRoomEvent obj)
            {
                //创建房间默认本地服务器
                this.GetSystem<INetWorkSystem>().CreateUdpClient(obj.ip, obj.port);
            }

            private void OnRoomCreate(CreateRoomEvent obj)
            {
                this.GetSystem<INetWorkSystem>().CreateUpdServer(obj.port);
                //创建房间默认本地服务器
                this.GetSystem<INetWorkSystem>().CreateUdpClient(NetWorkUtil.ServerDefaultIpAddress, obj.port);
                //this.GetSystem<INetWorkSystem>().UpgradeUdpToKcp();
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                
                this.UnRegisterEvent<CreateRoomEvent>(OnRoomCreate);
                this.UnRegisterEvent<ConnectRoomEvent>(OnRoomConnect);
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
                    this.SendCommand(new StartGameSuccessCommand());
                    break;
                case MsgType.PlayerMoveSpeed:
                    var playerMoveSpeedJson = JsonUtility.FromJson<PlayerMoveSpeedMsg>(jsonData);
                    var speed = playerMoveSpeedJson.MoveVelocity;
                    if (speed.Equals(Vector2.zero) == false)
                    {
                        Debug.Log($"MoveVelocity : {speed}");
                    }
                    //多玩家要ID的，这里直接赋值给另外的
                    foreach (var playerUnit in this.GetModel<IPlayerModel>().OtherPlayer)
                    {
                        playerUnit.Rigidbody.velocity = playerMoveSpeedJson.MoveVelocity;
                    }
                    break;
            }
        }
    }
}