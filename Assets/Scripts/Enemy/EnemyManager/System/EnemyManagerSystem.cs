using AssetSystem;
using Enemy.EnemyManager.Model;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy.EnemyManager.System
{
    public interface IEnemyManagerSystem : ISystem
    {
        /// <summary>
        /// 通过敌人的数据模型去创建对象
        /// 实际上它还是要获取对应的敌人的预制件
        /// 要实现这个还需要将敌人通用化
        /// 所以暂时不使用该函数
        /// </summary>
        /// <param name="dataModel"></param>
        public void CreateEnemyUnitByDataModel(AssetReference dataModel);
        public void CreateEnemyUnitByDataModel(string dataModelPath);

        /// <summary>
        /// 通过敌人的预制件去创建对象
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        public void CreateEnemyUnitByPrefab(AssetReference prefab, Vector3 position);
        public void CreateEnemyUnitByPrefab(string prefabPath , Vector3 position);
        
    }
    
    public class EnemyManagerSystem : AbstractSystem, IEnemyManagerSystem
    {
        private AsyncOperationHandle<GameObject> mHandle;
        
        protected override void OnInit()
        {
            
        }

        public void CreateEnemyUnitByDataModel(AssetReference dataModel)
        {
            
        }

        public void CreateEnemyUnitByDataModel(string dataModelPath)
        {
            
        }

        public void CreateEnemyUnitByPrefab(AssetReference prefab, Vector3 position)
        {

        }

        public async void CreateEnemyUnitByPrefab(string prefabPath, Vector3 position)
        {
            if (mHandle.Status == AsyncOperationStatus.Succeeded)
            {
                InstantiateEnemy(mHandle.Result, position);
            }
            
            var asset = await this.GetSystem<IAddressableSystem>().LoadAssetAsync<GameObject>(prefabPath);
            if (asset.Status == AsyncOperationStatus.Succeeded)
            {
                InstantiateEnemy(asset.Result, position);

                mHandle = asset;
            }
        }

        private void InstantiateEnemy(GameObject prefab, Vector3 position)
        {
            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            var component = gameObject.GetComponent<EnemyUnit.EnemyUnit>();
            if (component)
            {
                this.GetModel<IEnemyManagerModel>().UpdateEnemyUnits.Add(component);
            }
            else
            {
                Object.Destroy(gameObject);
            }
        }
    }
}