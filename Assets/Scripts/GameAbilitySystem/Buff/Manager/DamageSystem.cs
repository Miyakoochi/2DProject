using System.Collections.Generic;
using GameAbilitySystem.Buff.Unit;
using QFramework;

namespace GameAbilitySystem.Buff.Manager
{
    public interface IDamageSystem : ISystem
    {
        public void CreateDamage(IGameAbilityUnit attacker, IGameAbilityUnit defender, Damage damage, List<DamageInfoTag> tags = null);
        public void AddDamageInfo(DamageInfo info);
    }

    /// <summary>
    /// 处理DamageInfo伤害流程
    /// </summary>
    public class DamageSystem : AbstractSystem, IDamageSystem
    {
        private IDamageModel mDamageModel;
        
        protected override void OnInit()
        {
            mDamageModel = this.GetModel<IDamageModel>();
        }

        public void CreateDamage(IGameAbilityUnit attacker, IGameAbilityUnit defender, Damage damage, List<DamageInfoTag> tags = null)
        {
            var damageInfo = new DamageInfo(attacker, defender, damage, tags);
            mDamageModel.DamageInfos.Add(damageInfo);
        }
        
        public void AddDamageInfo(DamageInfo info)
        {
            mDamageModel.DamageInfos.Add(info);
        }
    }
}