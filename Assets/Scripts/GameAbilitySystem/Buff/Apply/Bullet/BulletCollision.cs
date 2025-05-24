using Core.QFrameWork;
using CsLua;
using GameAbilitySystem.Buff.Manager;
using QFramework;
using UnityEngine;

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
                    this.GetSystem<IDamageSystem>().CreateDamage(Owner.Owner, other.GetComponent<BuffState>(), new Damage(25));
                }
            }
        }
    }
}