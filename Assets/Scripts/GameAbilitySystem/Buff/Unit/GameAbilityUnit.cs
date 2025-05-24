using System.Collections.Generic;
using GameAbilitySystem.Buff.Buff;
using UnityEngine;

namespace GameAbilitySystem.Buff.Unit
{
    public interface IGameAbilityUnit
    {
        public GameObject Owner { get; set; }

        public List<Buff.Buff> Buffs { get; set; }

        /// <summary>
        /// 单位属性
        /// </summary>
        public UnitProperty Property { get; set; }

        public UnitState State { get; set; }

        /// <summary>
        /// 单位剩余资源
        /// </summary>
        public UnitResource RemainResource { get; set; }

        public List<Skill.Skill> Skills { get; set; }
    }
}