using Core.QFrameWork;
using CsLua;
using GameAbilitySystem.Buff.Manager;
using ObjectPool;
using QFramework;
using UnityEngine;
using UnityEngine.Pool;

namespace GameAbilitySystem.Buff.Apply.Bullet
{
    public class BulletCollision : BaseController
    {
        public BulletUnit Owner;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Owner == null)
            {
                return;
            }
            
            if (other.transform == Owner.SelfTransform)
            {
                return;
            }

            if (other.CompareTag("Unit"))
            {
                if (Owner.DataModel)
                {
                    this.GetSystem<IDamageSystem>().CreateDamage(Owner.Owner, other.GetComponent<BuffState>(), new Damage(10));
                }

                var rigidbody2d = other.GetComponent<Rigidbody2D>();
                rigidbody2d.AddForce(Owner.MoveDirection * 1.5f);
                this.GetModel<IBulletManagerModel>().UpdateBulletUnits.Remove(Owner);
                this.GetSystem<IObjectPoolSystem>().ReleaseObject(Owner);
            }
        }
    }
}