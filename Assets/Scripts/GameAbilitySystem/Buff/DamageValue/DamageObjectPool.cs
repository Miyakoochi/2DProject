using ObjectPool;
using QFramework;
using UI.UICore;
using UnityEngine;

namespace GameAbilitySystem.Buff.DamageValue
{
    public class DamageObjectPool : GameObjectPool
    {
        private void Start()
        {
            this.RegisterEvent<ShowDamageEvent>(OnShowDamage).UnRegisterWhenDisabled(gameObject);
            this.RegisterEvent<ShowDamagePositionEvent>(OnShowDamagePosition).UnRegisterWhenDisabled(gameObject);
        }

        private void OnShowDamagePosition(ShowDamagePositionEvent obj)
        {
            var gameobject = GetObject();
            var damage = gameobject.GetComponent<DamageValue>();
            if (damage)
            {
                damage.SetValue(obj.ShowTransform, obj.Value, () =>
                {
                    RecycleObject(gameobject);  
                });
            }
        }

        private void OnShowDamage(ShowDamageEvent obj)
        {
            var gameobject = GetObject();
            var damage = gameobject.GetComponent<DamageValue>();
            if (damage)
            {
                damage.SetValue(obj.ShowTransform.position, obj.Value, () =>
                {
                    RecycleObject(gameobject);  
                });
            }
        }
    }
}