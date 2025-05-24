using System.Collections.Generic;
using AssetSystem;
using Common;
using Cysharp.Threading.Tasks;
using Player.PlayerManager;
using QFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LevelSystem
{
    
    public interface ILevelSystem : ISystem
    {
        /// <summary>
        /// 加载主游戏场景
        /// 如果需要进度就给返回值改一下就行了
        /// </summary>
        /// <param name="mapId"></param>
        public void CreateLevelMap(int mapId);
        
        /// <summary>
        /// 获取地图数据是否加载好
        /// </summary>
        /// <returns></returns>
        public bool GetMapDataLoadedState();
        
        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <returns></returns>
        public UniTask<AsyncOperationHandle<IList<LevelDataModel>>> LoadLevelDataModel();

    }
    
    public class LevelSystem : AbstractSystem ,ILevelSystem
    {
        private ILevelModel mLevelModel;
        private IAddressableSystem mAddressableSystem;
        private IPlayerSystem mPlayerSystem;
        
        private AsyncOperationHandle<IList<LevelDataModel>> mMapsHandle;

        private Dictionary<int, LevelDataModel> LevelDataModels { get; set; } = new();

        
        public bool GetMapDataLoadedState()
        {
            if (mMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                return true;
            }

            return false;
        }
        
        public async UniTask<AsyncOperationHandle<IList<LevelDataModel>>> LoadLevelDataModel()
        {
            if(mMapsHandle.IsValid() == true && mMapsHandle.Status == AsyncOperationStatus.Succeeded) return mMapsHandle;
            
            mMapsHandle = await mAddressableSystem.LoadAssetsAsync<LevelDataModel>(new List<string>{ Util.LevelDataModelTag }, null);

            if (mMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var model in mMapsHandle.Result)
                {
                    LevelDataModels.TryAdd(model.MapId, model);
                }
            }
            else
            {
                Debug.Log("mMapsHandle Load fail");
            }

            return mMapsHandle;
        }

        public async void CreateLevelMap(int mapId)
        {
            //加载地图
            if (mMapsHandle.IsValid() == false || mMapsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                await LoadLevelDataModel();
            }

            if (mMapsHandle.IsValid() == true && mMapsHandle.Status != AsyncOperationStatus.Succeeded) return;
            if (LevelDataModels.TryGetValue(mapId, out var value) == false) return;

            mLevelModel.CurrentLevelDataModel = value;
        }
        
        protected override void OnInit()
        {
            mLevelModel = this.GetModel<ILevelModel>();
            mAddressableSystem = this.GetSystem<IAddressableSystem>();
            mPlayerSystem = this.GetSystem<IPlayerSystem>();
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();
            
            LevelDataModels.Clear();
            
            if (mMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                mMapsHandle.Release();
            }
        }
    }
}