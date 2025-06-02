using System.Collections.Generic;
using CsLua;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.Manager;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.Unit
{
    public interface IUnitSystem : ISystem
    {
        public void ModResource(IGameAbilityUnit unitEntity, UnitResource resource);
        public bool CanBeKilledByDamageInfo(IGameAbilityUnit unitEntity, DamageInfo damageInfo);
        public void AddBuff(IGameAbilityUnit unitEntity, AddBuffInfo buffInfo);

        public List<Buff.Buff> GetUnitBuffsById(IGameAbilityUnit unitEntity, string id,
            List<IGameAbilityUnit> buffCaster = null);

        public void CalculateBuffProperty(IGameAbilityUnit unitEntity);
        public void AddSkill(IGameAbilityUnit unitEntity, string skillId);

        public bool CastSkill(IGameAbilityUnit unitEntity, string skillId);
        //public void CastSkill(int unitEntityId, string skillId);

        public void AddUnit(IGameAbilityUnit unitEntity);
        //public void HandleApplyUnitHit(BulletEntity bulletEntity, IGameAbilityUnit unitEntity);

        //public IGameAbilityUnit CreateUnit(UnitProperty unitProperty, List<string> skills, List<Buff.Buff> buffs,List<string> tags);
            

        //public IGameAbilityUnit CreateUnit(UnitProperty unitProperty);
        //public IGameAbilityUnit GetGameAbilityUnitById(int id);
    }
    
    public class UnitSystem : AbstractSystem, IUnitSystem
    {
        private ITimeLineSystem mTimeLineSystem;
        private IUnitModel mUnitModel;
        private ILuaSystem mLuaSystem;
        
        public void ModResource(IGameAbilityUnit unitEntity, UnitResource resource)
        {
            unitEntity.RemainResource = new UnitResource(
                resource.Hp + unitEntity.RemainResource.Hp);
        }

        public bool CanBeKilledByDamageInfo(IGameAbilityUnit unitEntity, DamageInfo damageInfo)
        {
            if (damageInfo.IsHeal() || unitEntity.State.ImmuneTime > 0)
            {
                return false;
            }

            int damageValue = damageInfo.DamageValue(false);
            return unitEntity.RemainResource.Hp <= damageValue;
        }

        public void AddBuff(IGameAbilityUnit unitEntity, AddBuffInfo buffInfo)
        {
            List<Buff.Buff> buffs = GetUnitBuffsById(unitEntity, buffInfo.BuffDataModel.Id, new List<IGameAbilityUnit> { buffInfo.Caster });

            int modStack = Mathf.Min(buffInfo.AddStack, buffInfo.BuffDataModel.MaxStack);
            
            //如果添加一个不存在且减少层数的buff 则什么都不做
            if (buffs.Count <= 0 && modStack <= 0)
            {
                return;
            }

            Buff.Buff addBuff;
            
            //如果存在buff
            if (buffs.Count > 0)
            {
                //默认只存在一个buff
                addBuff = buffs[0];
                //替换参数
                foreach (var buffParamKvp in buffInfo.BuffParam)
                {
                    addBuff.BuffParams[buffParamKvp.Key] = buffParamKvp.Value;
                }

                var tempStack = modStack + addBuff.Stack;
                if (tempStack > addBuff.DataModel.MaxStack)
                {
                    modStack = addBuff.DataModel.MaxStack - addBuff.Stack;
                }

                if (tempStack < 0)
                {
                    modStack = -addBuff.Stack;
                }
                
                addBuff.Stack += modStack;
                
                addBuff.Permanent = addBuff.Permanent;
            }
            else
            {
                //如果不存在buff并且增加层数 就新建一个
                 addBuff = new Buff.Buff(
                    buffInfo.BuffDataModel, buffInfo.Duration, 
                    buffInfo.Caster, unitEntity, 
                    modStack, buffInfo.Permanent, buffInfo.BuffParam);
                
                unitEntity.Buffs.Add(addBuff);
                buffs.Sort((a, b)=> a.DataModel.Priority.CompareTo(b.DataModel.Priority));
            }

            var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnOccur>(addBuff.DataModel.OnOccur);
            function?.Invoke(addBuff, modStack);
            
            CalculateBuffProperty(unitEntity);
        }

        public List<Buff.Buff> GetUnitBuffsById(IGameAbilityUnit unitEntity, string id, List<IGameAbilityUnit> buffCaster = null)
        {
            List<Buff.Buff> result = new List<Buff.Buff>();
            foreach (var buff in unitEntity.Buffs)
            {
                //如果ID相同并且有相同Caster，则返回相同Caster的buff 
                if (buff.DataModel.Id == id &&
                    (buffCaster is not { Count: > 0 } || buffCaster.Contains(buff.Caster)))
                {
                    result.Add(buff);
                }
            }
            return result;
        }

        /// <summary>
        /// 计算单位属性
        /// </summary>
        /// <param name="unitEntity"></param>
        public void CalculateBuffProperty(IGameAbilityUnit unitEntity)
        {
            UnitProperty property = UnitProperty.Zero;
            foreach (var buff in unitEntity.Buffs)
            {
                property = buff.DataModel.PropAddMod + property;
            }
            
            unitEntity.Property = unitEntity.Property + property;
        }
        
        public void AddSkill(IGameAbilityUnit unitEntity , string skillId)
        {
            var skillModel = this.GetSystem<IGameAbilitySystem>().GetSkillDataModel(skillId);
            var skill = new Skill.Skill(skillModel);
            unitEntity.Skills.Add(skill);
        }

        private bool TryGetSkill(IGameAbilityUnit unitEntity ,string skillId, out Skill.Skill outSkill)
        {
            foreach (var skill in  unitEntity.Skills)
            {
                if (skill.DataModel.Id.Equals(skillId))
                {
                    outSkill = skill;
                    return true;
                }
            }

            outSkill = null;
            return false;
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="unitEntity"></param>
        /// <param name="skillId"></param>
        public bool CastSkill(IGameAbilityUnit unitEntity, string skillId)
        {
            //TODO 判断是否能够使用技能 如果成功则触发技能的时间轴 实际技能的冷却时间设置
            if (TryGetSkill(unitEntity, skillId, out var skill) == false)
            {
                return false;
            }
            
            //如果在冷却
            if (skill.BuiltInCooldown > 0)
            {
                return false;
            }

            //如果施法资源足够
            if (unitEntity.RemainResource.Enough(skill.DataModel.Condition) == true)
            {
                TimeLine.TimeLine timeLine = new TimeLine.TimeLine(skill.DataModel.Effect, unitEntity, skill);
                
                //执行一遍buff中释放技能的回调
                foreach (var buff in unitEntity.Buffs)
                {
                    var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnCast>(buff.DataModel.OnCast);
                    function?.Invoke(buff, skill, timeLine);
                }
                
                //执行资源消耗
                ModResource(unitEntity, -1 * skill.DataModel.Condition);
                
                //保证消耗血量不会为0
                if (unitEntity.RemainResource.Hp <= 0)
                {
                    var resource = unitEntity.RemainResource;
                    resource.Hp = 1;
                    unitEntity.RemainResource = resource;
                }

                mTimeLineSystem.AddTimeLine(timeLine);
            }
            
            //无论是否成功都设定一个内置CD 避免连续多次释放
            skill.BuiltInCooldown = 0.5;
            return true;
        }
        
        public void AddUnit(IGameAbilityUnit unitEntity)
        {
            mUnitModel.Units.Add(unitEntity);
        }

        /*public void HandleApplyUnitHit(BulletEntity bulletEntity , IGameAbilityUnit unitEntity)
        {
            //GD.Print("Caster.RemainResource.Hp: " + bulletEntity.Caster.RemainResource.Hp);
            //GD.Print("enemyActor.RemainResource.Hp: " + unitEntity.RemainResource.Hp);
            //TODO 
           // bulletEntity.DataModel.OnHit?.Invoke(bulletEntity, unitEntity);
        }*/
        
        protected override void OnInit()
        {
            mTimeLineSystem = this.GetSystem<ITimeLineSystem>();
            mUnitModel = this.GetModel<IUnitModel>();
            mLuaSystem = this.GetSystem<ILuaSystem>();
        }
    }
}