using System.Collections.Generic;
using AssetSystem;
using Common;
using Cysharp.Threading.Tasks;
using Enemy.EnemyUnit.DataModel;
using QFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DataModelManager
{
    public interface IDataModelSystem : ISystem
    {
        public UniTask<AsyncOperationHandle<IList<EnemyUnitDataModel>>> LoadAllEnemyDataModel();
        public EnemyUnitDataModel GetEnemyDataModel(int id);
    }
    
    public class DataModelSystem : AbstractSystem, IDataModelSystem
    {
        private Dictionary<int, EnemyUnitDataModel> mEnemyDataModels = new();
        private AsyncOperationHandle<IList<EnemyUnitDataModel>> mHandles;
        private List<AsyncOperationHandle<EnemyUnitDataModel>> mSingleHandles;

        public async UniTask<AsyncOperationHandle<IList<EnemyUnitDataModel>>> LoadAllEnemyDataModel()
        {
            if (mHandles.IsValid() == true && mHandles.Status == AsyncOperationStatus.Succeeded)
            {
                return mHandles;
            }
            
            mHandles = await this.GetSystem<IAddressableSystem>().LoadAssetsAsync<EnemyUnitDataModel>(new List<string> { Util.EnemyDataModelTag }, null);

            if (mHandles.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var result in mHandles.Result)
                {
                    mEnemyDataModels.TryAdd(result.Id, result);
                }
            }           
            else
            {
                Debug.Log("LoadAllEnemyDataModel Load fail");
            }

            return mHandles;
        }

        public EnemyUnitDataModel GetEnemyDataModel(int id)
        {
            if (mEnemyDataModels.TryGetValue(id, out var value) == false) return null;
            
            return value;
        }

        private void ClearAllCache()
        {
            mEnemyDataModels.Clear();
            mHandles.Release();
            
            foreach (var handle in mSingleHandles)
            {
                handle.Release();
            }
        }
        
        protected override void OnInit()
        {
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();
            ClearAllCache();
        }
    }
}