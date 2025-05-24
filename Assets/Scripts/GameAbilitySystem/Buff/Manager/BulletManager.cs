using Core.QFrameWork;
using ObjectPool;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.Manager
{
    public class BulletManager : BaseController
    {

        private IBulletManagerModel mBulletManagerModel;
        private IBulletManagerSystem mBulletManagerSystem;

        private void Awake()
        {
            mBulletManagerModel = this.GetModel<IBulletManagerModel>();
            mBulletManagerSystem = this.GetSystem<IBulletManagerSystem>();
        }

        private void FixedUpdate()
        {
            int index = 0;

            while (index < mBulletManagerModel.UpdateBulletUnits.Count)
            {
                var obj = mBulletManagerModel.UpdateBulletUnits[index];
                obj.SelfRigidbody.velocity = obj.MoveDirection * obj.Speed;
                obj.Duration -= Time.fixedDeltaTime;

                if (obj.Duration <= 0)
                {
                    mBulletManagerSystem.ReleaseBullet(index);
                }
                else
                {
                    index++;
                }
            }
        }
    }
}
