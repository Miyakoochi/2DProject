using System.Collections.Generic;
using Core.QFrameWork;
using GameAbilitySystem.Buff.Apply.Bullet;
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
            var array =  new List<BulletUnit>(this.GetModel<IBulletManagerModel>().UpdateBulletUnits);
            for (int index = array.Count - 1; index >= 0; index--)
            {
                var obj = array[index];
                obj.SelfRigidbody.velocity = obj.MoveDirection * obj.Speed;
                obj.Duration -= Time.fixedDeltaTime;

                if (obj.Duration <= 0)
                {
                    mBulletManagerSystem.ReleaseBullet(index);
                }
            }
        }

        private void OnDestroy()
        {
            this.GetSystem<IObjectPoolSystem>().ReleaseAll<BulletUnit>();
        }
    }
}
