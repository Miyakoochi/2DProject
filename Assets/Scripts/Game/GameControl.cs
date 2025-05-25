using AssetSystem;
using Audio;
using Command;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using GameAbilitySystem.Buff;
using GameAbilitySystem.Buff.Manager;
using InputSystem;
using NetWorkSystem;
using Player.PlayerManager;
using QFramework;
using SceneSystem;
using StateMachine;
using UI.UICore;
using UnityEngine;

namespace Core
{
    
    public class GameControl : BaseController
    {
        private StateMachineController mStateMachineController;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            mStateMachineController = GetComponent<StateMachineController>();
        }

        private void Start()
        {
            if(!mStateMachineController) return;
            
            mStateMachineController.AddState("InitUI", new InitUIState());
            mStateMachineController.AddState("InitAudio" , new InitAudio());
            mStateMachineController.AddState("InitBuffDataModels" , new InitBuffDataModels());
            mStateMachineController.AddState("UpdateAsset" , new UpdateAssetState());
            mStateMachineController.AddState("GameState", new GameState());
            mStateMachineController.AddState("MainMenuState", new MainMenuState());
            
            mStateMachineController.SetDefaultState("InitUI");
        }
        
        private class InitUIState : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();
                
                this.SendCommand(new InitUICommand());
                this.RegisterEvent<EndInitUIEvent>(OnUIInitEnd);
            }

            private void OnUIInitEnd(EndInitUIEvent @event)
            {
                this.GetSystem<IUISystem>().SetUIShow(UIType.Loading, true);
                this.GetSystem<IUISystem>().FadeOut();
                this.GetSystem<IUISystem>().SetUIShow(UIType.Tips, false);
                SendChangeEvent("InitAudio");
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                this.UnRegisterEvent<EndInitUIEvent>(OnUIInitEnd);
            }
        }

        private class InitAudio : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();
                
                this.GetSystem<IAudioSystem>().LoadMusicDataModel();
                this.RegisterEvent<MusicAssetLoadEnd>(OnMusicAssetLoadEnd);
            }

            private void OnMusicAssetLoadEnd(MusicAssetLoadEnd obj)
            {
                this.GetSystem<IAudioSystem>().PlayBGM(EMusicType.MainMenu);
                SendChangeEvent("InitBuffDataModels");
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                this.UnRegisterEvent<MusicAssetLoadEnd>(OnMusicAssetLoadEnd);
            }
        }
        
        private class UpdateAssetState : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();

                this.RegisterEvent<EndCheckAndUpdateAssetEvent>(OnUpdateAssetEnd);
                //下载并更新资源
                this.GetSystem<IAddressableSystem>().CheckAndDownloadRemoteAsset();
                //热更代码
            }

            private async void OnUpdateAssetEnd(EndCheckAndUpdateAssetEvent obj)
            {            
                await UniTask.WaitForSeconds(1);
                this.GetSystem<IUISystem>().SetUIShow(UIType.Loading, false);
                this.GetSystem<IUISystem>().SetUIShow(UIType.MainMenu, true);
                
                SendChangeEvent("MainMenuState");
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                this.UnRegisterEvent<EndCheckAndUpdateAssetEvent>(OnUpdateAssetEnd);
            }
        }

        private class InitBuffDataModels : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();

                this.RegisterEvent<FinishedLoadAllBuffDataModel>(OnFinishedBuffDataModelLoad);
                this.GetSystem<IGameAbilitySystem>().LoadAllAbilitySystemDataModel();
            }

            private void OnFinishedBuffDataModelLoad(FinishedLoadAllBuffDataModel obj)
            {
                SendChangeEvent("UpdateAsset");
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                
                this.UnRegisterEvent<FinishedLoadAllBuffDataModel>(OnFinishedBuffDataModelLoad);
            }
        }

        private class MainMenuState : StateController
        {
            private bool mIsRunning = true;
            
            protected override void OnEnterState()
            {
                base.OnEnterState();
                
                this.RegisterEvent<SceneEndLoadEvent>(OnSceneSwitch);
                //TODO 注册网络事件
                this.RegisterEvent<ClientConnectSuccessEvent>(OnClientConnectSuccess);
                this.RegisterEvent<StartGameSuccessEvent>(OnStartGame);
            }

            private void OnStartGame(StartGameSuccessEvent obj)
            {
                mIsRunning = false;
                this.GetSystem<IPlayerSystem>().StartSyncVelocity();
            }

            //成功连接后每秒尝试发送匹配开房信息
            //TODO 
            private async void OnClientConnectSuccess(ClientConnectSuccessEvent obj)
            {
                while (mIsRunning)
                {
                    var client = this.GetModel<INetWorkModel>().LocalKcpClient;
                    if (client != null)
                    {
                        var json = JsonUtility.ToJson(new BaseKcpMsg()
                        {
                            MsgType = MsgType.StartGameRequest,
                            Conv = client.SelfConv
                        });
                        client.Send(json);
                    }
                
                    await UniTask.WaitForSeconds(1);
                }
            }

            private void OnSceneSwitch(SceneEndLoadEvent obj)
            {
                SendChangeEvent("GameState");
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                
                this.UnRegisterEvent<SceneEndLoadEvent>(OnSceneSwitch);
                this.UnRegisterEvent<ClientConnectSuccessEvent>(OnClientConnectSuccess);
                this.UnRegisterEvent<StartGameSuccessEvent>(OnStartGame);
            }
        }
        
        private class GameState : StateController
        {
            protected override void OnEnterState()
            {
                base.OnEnterState();

                this.GetSystem<IAudioSystem>().PlayBGM(EMusicType.Game);
                this.GetSystem<IUISystem>().SetUIShow(UIType.StopGame, true);
                //通过资产或者DataModel创建一个规则类，这个规则类可以有一个委托，注册回调函数。游戏流程在收到特定事件后
                //一个统计类注册角色击杀等事件从而修改自己统计的数字。并且也可以发送事件给游戏规则类（游戏规则类可以仅仅注册事件）。
                this.GetModel<IPlayerModel>().KillCount.Register(OnPlayerKillUnit);
                
                //TODO 注册游戏退回标题页面
                this.RegisterEvent<SceneEndLoadEvent>(OnSceneSwitch);
            }

            private void OnSceneSwitch(SceneEndLoadEvent obj)
            {
                this.GetSystem<IUISystem>().SetUIShow(UIType.MainMenu, true);
                this.GetSystem<IUISystem>().FadeOut();
                
                this.GetModel<ITimeLineModel>().TimeLines.Clear();
                this.GetModel<IDamageModel>().DamageInfos.Clear();
                this.GetModel<IBulletManagerModel>().UpdateBulletUnits.Clear();
                
                SendChangeEvent("MainMenuState");
            }

            private async void OnPlayerKillUnit(int obj)
            {
                if (obj == 6)
                {
                    await UniTask.WaitForSeconds(1.0f);

                    //显示游戏结束UI,后续点击返回主界面可以发送事件告诉当前关卡退出，卸载资源等这里就先不做了。
                    this.GetSystem<IUISystem>().SetUIShow(UIType.StopGame, false);
                    this.GetSystem<IUISystem>().SetUIShow(UIType.GameEndMenu, true);
                }
            }

            protected override void OnExitState()
            {
                base.OnExitState();
                
                this.GetSystem<IInputControllerSystem>().UnBindController();
                this.UnRegisterEvent<SceneEndLoadEvent>(OnSceneSwitch);
            }
        }
    }
    
}