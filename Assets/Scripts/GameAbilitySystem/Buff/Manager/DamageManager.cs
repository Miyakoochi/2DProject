using Command;
using Core.QFrameWork;
using CsLua;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.DamageValue;
using GameAbilitySystem.Buff.Unit;
using QFramework;
using UI.UICore;
using UnityEngine;

namespace GameAbilitySystem.Buff.Manager
{
    public class DamageManager : BaseController
    {
        public DamageParticle DamageParticle;
        
        private IDamageModel mDamageModel;
        private IUnitSystem mUnitSystem;
        private ILuaSystem mLuaSystem;

        private void Start()
        {
            mDamageModel = this.GetModel<IDamageModel>();
            mUnitSystem = this.GetSystem<IUnitSystem>();
            mLuaSystem = this.GetSystem<ILuaSystem>();
        }

        private void FixedUpdate()
        {
            int index = 0;

            //每次处理第一个DamageInfo
            while (index < mDamageModel.DamageInfos.Count)
            {
                var damageInfo = mDamageModel.DamageInfos[index];
                DeadDamage(damageInfo);
                mDamageModel.DamageInfos.RemoveAt(index);
            }
        }
        
               /// <summary>
        /// 处理攻击流程 整个游戏的伤害流程
        /// 前提 进入攻击流程前攻击数值已经与玩家的数值处理过了
        /// 首先判断被攻击者是否被击败 如果被击败则直接退出流程
        /// 走一遍攻击者所有buff的OnHit回调点
        /// 走一遍受攻击者所有buff的OnBeHurt回调点
        /// 如果角色可能被杀死，就会走OnKill和OnBeKilled，这个游戏里面没有免死金牌之类的技能，所以只要判断一次就好
        /// 根据结果处理 治疗或者非无敌状态才会修改数值
        /// 伤害流程走完之后 添加伤害信息附加的Buff
        /// </summary>
        /// <param name="damageInfo"></param>
        private void DeadDamage(DamageInfo damageInfo)
        {
            //首先判断被攻击者是否被击败 如果被击败则直接退出流程
            if (damageInfo.Defender == null)
            {
                return;
            }

            IGameAbilityUnit defenderUnit = damageInfo.Defender;
            if (defenderUnit == null)
            {
                return;
            }

            if (defenderUnit.State.IsDead == true)
            {
                return;
            }

            IGameAbilityUnit attackerUnit = null;
            //走一遍攻击者所有buff的OnHit回调点
            if (damageInfo.Attacker != null)
            {
                attackerUnit = damageInfo.Attacker;

                foreach (var buff in attackerUnit.Buffs)
                {
                    var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnHit>(buff.DataModel.OnHit);
                    function?.Invoke(buff, ref damageInfo, damageInfo.Defender);
                }
            }

            //走一遍受攻击者所有buff的OnBeHurt回调点
            foreach (var buff in defenderUnit.Buffs)
            {
                var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnBeHurt>(buff.DataModel.OnBeHurt);
                function?.Invoke(buff, ref damageInfo, damageInfo.Attacker);
            }

            //如果角色可能被杀死，就会走OnKill和OnBeKilled
            /*
                这里判断角色是否能被杀死
                需要一个系统判断角色是否无敌
                且伤害信息是否为治疗
                最后根据伤害计算角色是否可能被杀死
                因此这里面会将伤害数值计算好
                所以可以先提前计算好再使用伤害数值
            */
            if (mUnitSystem.CanBeKilledByDamageInfo(defenderUnit, damageInfo))
            {
                if (attackerUnit != null)
                {
                    foreach (var buff in attackerUnit.Buffs)
                    {
                        var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnKill>(buff.DataModel.OnKill);
                        function?.Invoke(buff, damageInfo, damageInfo.Defender);
                    }
                }

                foreach (var buff in defenderUnit.Buffs)
                {
                    var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnBeKilled>(buff.DataModel.OnBeKilled);
                    function?.Invoke(buff, damageInfo, damageInfo.Attacker);
                }
                this.SendCommand<UnitBeDeadCommand>(new UnitBeDeadCommand(attackerUnit, defenderUnit));
            }

            //根据结果处理 治疗或者非无敌状态才会修改数值
            bool isHeal = damageInfo.IsHeal();
            int damageValue = damageInfo.DamageValue(isHeal);

            if (isHeal || defenderUnit.State.ImmuneTime <= 0)
            {
                if (damageInfo.RequireDoHurt() == true && mUnitSystem.CanBeKilledByDamageInfo(defenderUnit, damageInfo) == false)
                {
                    //UnitAnim ua = defenderChaState.GetComponent<UnitAnim>();
                    //if (ua) ua.Play("Hurt");
                }

                mUnitSystem.ModResource(defenderUnit, new UnitResource(
                    -damageValue
                ));

                //按游戏设计的规则跳数字，如果要有暴击，也可以丢在策划脚本函数（lua可以返回多参数）也可以随便怎么滴
                //system直接创建（利用对象池）UI管理
                //this.SendCommand(new CreateDamageCommand(defenderUnit.Owner.transform, Mathf.Abs(damageValue)));
                if (DamageParticle)
                {
                    DamageParticle.EmitDamage(defenderUnit.Owner.transform.position, Mathf.Abs(damageValue));
                }
            }

            //伤害流程走完之后 添加伤害信息附加的Buff
            foreach (var addBuff in damageInfo.AddBuffs)
            {
                //如果目标是攻击者但是并没有攻击者 则不处理buff添加
                var target = addBuff.Target;
                var unit = target.Equals(damageInfo.Attacker) ? attackerUnit : defenderUnit;

                if (unit is { State: { IsDead: false } })
                {
                    mUnitSystem.AddBuff(unit, addBuff);
                }
            }
        }
    }
}