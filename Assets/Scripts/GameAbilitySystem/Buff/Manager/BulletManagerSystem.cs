using AssetSystem;
using Audio;
using GameAbilitySystem.Buff.Apply.Bullet;
using GameAbilitySystem.Buff.Unit;
using ObjectPool;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameAbilitySystem.Buff.Manager
{
    public interface IBulletManagerSystem : ISystem
    {
        /// <summary>
        ///  通过子弹的数据去创建子弹对象
        /// </summary>
        /// <param name="bulletDataModelId"></param>
        /// <param name="createUnit"></param>
        public void CreateBulletById(string bulletDataModelId, IGameAbilityUnit createUnit);

        public void ReleaseBullet(int index);
    }
    
    public class BulletManagerSystem : AbstractSystem, IBulletManagerSystem
    {
        private IGameAbilitySystem mGameAbilitySystem;
        private IObjectPoolSystem mObjectPoolSystem;
        private IAudioSystem mAudioSystem;
        private IBulletManagerModel mBulletManagerModel;
        
        protected override void OnInit()
        {
            mGameAbilitySystem = this.GetSystem<IGameAbilitySystem>();
            mObjectPoolSystem = this.GetSystem<IObjectPoolSystem>();
            mBulletManagerModel = this.GetModel<IBulletManagerModel>();
            mAudioSystem = this.GetSystem<IAudioSystem>();
        }

        public void CreateBulletById(string bulletDataModelId, IGameAbilityUnit createUnit)
        {

            //发射子弹
            // 获取鼠标在屏幕上的位置
            Vector3 mousePos = Input.mousePosition;

            // 将鼠标在屏幕上的位置转换为世界空间中的位置
            if (!Camera.main) return;
            
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));

            // 计算物体需要朝向的方向
            Vector3 direction = (worldPos - createUnit.Owner.transform.position).normalized;
            
            
            //TODO 可以设置为事件
            CreateBulletById(bulletDataModelId, createUnit, createUnit.Owner.transform.position, direction);
            
        }

        public void ReleaseBullet(int index)
        {
            var unit = mBulletManagerModel.UpdateBulletUnits[index];
            mObjectPoolSystem.ReleaseObject(unit);
            mBulletManagerModel.UpdateBulletUnits.RemoveAt(index);
        }

        private void CreateBulletById(string bulletId, IGameAbilityUnit createUnit, Vector3 startPosition, Vector3 moveDirection)
        {
            
            var dataModel = mGameAbilitySystem.GetBulletDataModel(bulletId);
            if(!dataModel) return;

            var unit = mObjectPoolSystem.GetObject<BulletUnit>();
            unit.Set(dataModel);
            unit.Owner = createUnit;
            unit.MoveDirection = moveDirection;
            unit.SelfTransform.position = startPosition;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            // 应用旋转
            unit.SelfTransform.rotation = Quaternion.Euler(0, 0, angle);
            
            mBulletManagerModel.UpdateBulletUnits.Add(unit);
            mAudioSystem.PlayAudioOnce(EMusicType.Attack);
        }
    }
}