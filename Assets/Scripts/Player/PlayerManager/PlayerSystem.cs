using System.Collections.Generic;
using System.Threading.Tasks;
using AssetSystem;
using Common;
using Cysharp.Threading.Tasks;
using GameAbilitySystem.Buff;
using NetWorkSystem;
using Player.Player;
using Player.Player.Data;
using QFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Player.PlayerManager
{
    public interface IPlayerSystem : ISystem
    {
        public UniTask<AsyncOperationHandle<IList<PlayerDataModel>>> LoadAllPlayerDataModel();
        public void ClearAllCache();
        public PlayerUnit CreateSelfPlayerById(int playerUnitId);
        public PlayerUnit CreatePlayerById(int playerUnitId);
    }
    
    public class PlayerSystem : AbstractSystem, IPlayerSystem
    {
        private IPlayerModel mPlayerModel;
        
        private Dictionary<int, PlayerDataModel> mPlayerDataModels = new();
        private AsyncOperationHandle<IList<PlayerDataModel>> mHandles;
        private List<AsyncOperationHandle<PlayerDataModel>> mSingleHandles;

        public async UniTask<AsyncOperationHandle<IList<PlayerDataModel>>> LoadAllPlayerDataModel()
        {
            if (mHandles.IsValid() == true && mHandles.Status == AsyncOperationStatus.Succeeded)
            {
                return mHandles;
            }
            
            mHandles = await this.GetSystem<IAddressableSystem>().LoadAssetsAsync<PlayerDataModel>(new List<string> { Util.PlayerDataModelTag }, null);

            if (mHandles.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var result in mHandles.Result)
                {
                    mPlayerDataModels.TryAdd(result.id, result);
                }
            }           
            else
            {
                Debug.Log("LoadAllPlayerDataModel Load fail");
            }

            return mHandles;
        }

        public void ClearAllCache()
        {
            mPlayerDataModels.Clear();
            mHandles.Release();
            
            foreach (var handle in mSingleHandles)
            {
                handle.Release();
            }
        }

        public async void CreatePlayerByDataModelAsset(string dataModelPath)
        {
            if(this.GetModel<IPlayerModel>().CurrentControlPlayer != null) return;

            var dataModel = await this.GetSystem<IAddressableSystem>().LoadAssetAsync<PlayerDataModel>(dataModelPath);
            
            if(dataModel.Status != AsyncOperationStatus.Succeeded) return;
            
            if(mPlayerDataModels.TryGetValue(dataModel.Result.id, out var value))
            {
                InstantiatePlayerByDataModel(value);
                dataModel.Release();
                return;
            }
            
            InstantiatePlayerByDataModel(dataModel.Result);
            mPlayerDataModels.TryAdd(dataModel.Result.id, dataModel.Result);
            mSingleHandles.Add(dataModel);
        }

        public PlayerUnit CreateSelfPlayerById(int dataModelId)
        {
            if (mPlayerDataModels.TryGetValue(dataModelId, out var value) == false) return null;
            
            InstantiatePlayerByDataModel(value);
            return this.GetModel<IPlayerModel>().CurrentControlPlayer;;
        }

        public PlayerUnit CreatePlayerById(int dataModelId)
        {
            if (mPlayerDataModels.TryGetValue(dataModelId, out var value) == false) return null;
            
            var playerUnit = new PlayerUnit(value);
            return playerUnit;
        }
        
        private void InstantiatePlayerByDataModel(PlayerDataModel dataModel)
        {
            var playerUnit = new PlayerUnit(dataModel);
            this.GetModel<IPlayerModel>().CurrentControlPlayer = playerUnit;
        }
        
        protected override void OnInit()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            
            this.RegisterEvent<UnitKillEvent>(OnKillUnit);
        }

        private void OnKillUnit(UnitKillEvent obj)
        {
            if (ReferenceEquals(obj.AttackUnit, mPlayerModel.CurrentControlPlayer.PlayerBuff))
            {
                
            }
        }
        
        private async void StartSyncVelocity()
        {
            while (mSyncing)
            {
                await UniTask.Delay(10);
                
                if(mPlayerModel.CurrentControlPlayer == null) return;
                
                var client = this.GetModel<INetWorkModel>().LocalKcpClient;
                if (client != null)
                {
                    var playerSpeedJson = JsonUtility.ToJson(new PlayerMoveSpeedMsg()
                    {
                        Conv = client.SelfConv,
                        MsgType = MsgType.PlayerMoveSpeed,
                        MoveVelocity = mPlayerModel.CurrentControlPlayer.Rigidbody.velocity
                    });
                    client.Send(playerSpeedJson);
                }
            }
        }

        private bool mSyncing = false;
    }
}