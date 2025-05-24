using System.Collections.Generic;
using GameAbilitySystem.Buff.Tags;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff.Buff
{
    [CreateAssetMenu(menuName = "DataModel/Buff/BuffDataModel")]
    public class BuffDataModel : ScriptableObject
    {
        /// <summary>
        /// Buff的ID
        /// </summary>
        [InspectorName("Buff的ID")]
        public string Id;

        /// <summary>
        /// Buff的Tags
        /// </summary>
        [InspectorName("Buff的Tags")]
        public List<GameTag> Tags;

        /// <summary>
        /// Buff优先级 优先级越高越前面执行
        /// </summary>
        [InspectorName("Buff的优先级")]
        public int Priority;

        /// <summary>
        /// Buff的最大栈层数
        /// </summary>
        [InspectorName("Buff的最大层数")]
        public int MaxStack;

        /// <summary>
        /// Buff多久Tick一次
        /// </summary>
        [InspectorName("Buff多久Tick一次")]
        public float TickTime;


        //属性及状态影响

        #region Property And State

        /// <summary>
        /// 单位属性修改
        /// Buff会给单位添加的属性
        /// </summary>
        [InspectorName("Buff会添加的属性")]
        public UnitProperty PropAddMod;
        public UnitProperty PropTimesMod;

        /// <summary>
        /// 单位状态修改
        /// Buff会给单位状态的影响
        /// </summary>
        [InspectorName("Buff给单位状态的影响")]
        public UnitState StateMod;

        #endregion


        //回调点

        #region CallBackPoints

        /// <summary>
        /// buff在被添加、改变层数时候触发的事件
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// <param>会传递本次改变的层数
        ///     <name>modifyStack</name>
        /// </param>
        /// </summary>
        [InspectorName("被添加改变层数时触发")]
        public string OnOccur;

        //public object[] OnOccurParams;

        ///<summary>
        ///buff在每个工作周期会执行的函数，如果这个函数为空，或者tickTime < = 0 ，都不会发生周期性工作
        ///<param name="buff">会传递给脚本Buff作为参数</param>
        ///</summary>
        [InspectorName("每个Tick执行的函数")]
        public string OnTick;

        //public object[] OnTickParams;

        /// <summary>
        /// 在这个Buff被移除之前要做的事情，如果运行之后Buff又不足以被删除了就会被保留
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// </summary>
        [InspectorName("被移除之前要做的事情")]
        public string OnRemoved;

        //public object[] OnRemovedParams;

        /// <summary>
        /// 在释放技能的时候运行的buff，执行这个buff获得最终技能要产生的Timeline
        /// 会传递给脚本的Buff
        /// 即将释放的技能skillObj
        /// 释放出来的技能，也就是一个timeline，这里的本质就是让你通过buff还能对timeline进行hack以达到修改技能效果的目的
        /// </summary>
        [InspectorName("释放技能时运行")]
        public string OnCast;

        //public object[] OnCastParams;

        /// <summary>
        /// 在伤害流程中，持有这个buff的人作为攻击者会发生的事情
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// <param>这次的伤害信息
        ///     <name>damageInfo</name>
        /// </param>
        /// <param>挨打的角色对象
        ///     <name>target</name>
        /// </param>
        /// </summary>
        [InspectorName("伤害敌人时发生")]
        public string OnHit;

        //public object[] OnHitParams;

        /// <summary>
        /// 在伤害流程中，持有这个buff的人作为挨打者会发生的事情
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// <param>这次的伤害信息
        ///     <name>damageInfo</name>
        /// </param>
        /// <param>打我的角色，当然可以是空的
        ///     <name>attacker</name>
        /// </param>
        /// </summary>
        [InspectorName("收到伤害时发生")]
        public string OnBeHurt;

        //public object[] OnBeHurtParams;

        /// <summary>
        /// 在伤害流程中，如果击杀目标，则会触发的啥事情
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// <param>这次的伤害信息
        ///     <name>damageInfo</name>
        /// </param>
        /// <param>挨打的角色对象
        ///     <name>target</name>
        /// </param>
        /// </summary>
        [InspectorName("击杀目标时发生")]
        public string OnKill;

        //public object[] OnKillParams;

        /// <summary>
        /// 在伤害流程中，持有这个buff的人被杀死了，会触发的事情
        /// <param>会传递给脚本Buff作为参数
        ///     <name>buff</name>
        /// </param>
        /// <param>这次的伤害信息
        ///     <name>damageInfo</name>
        /// </param>
        /// <param>发起攻击造成击杀的角色对象
        ///     <name>attacker</name>
        /// </param>
        /// </summary>
        [InspectorName("被击杀时发生")]
        public string OnBeKilled;

        //public object[] OnBeKilledParams;

        #endregion
    }

    public delegate void BuffOnOccur(Buff buff, int modifyStack);

    public delegate void BuffOnRemoved(Buff buff);

    public delegate void BuffOnTick(Buff buff);

    public delegate void BuffOnHit(Buff buff, ref DamageInfo damageInfo, IGameAbilityUnit target);

    public delegate void BuffOnBeHurt(Buff buff, ref DamageInfo damageInfo, IGameAbilityUnit attacker);

    public delegate void BuffOnKill(Buff buff, DamageInfo damageInfo, IGameAbilityUnit target);

    public delegate void BuffOnBeKilled(Buff buff, DamageInfo damageInfo, IGameAbilityUnit attacker);

    public delegate TimeLine.TimeLine BuffOnCast(Buff buff, Skill.Skill skill, TimeLine.TimeLine timeline);
}