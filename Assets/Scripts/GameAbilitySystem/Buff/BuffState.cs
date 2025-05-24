using System.Collections.Generic;
using Core.QFrameWork;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff
{
    public class BuffState : BaseController, IGameAbilityUnit
    {
        public GameObject Owner { get; set; }
        public List<Buff.Buff> Buffs { get; set; } = new();
        public UnitProperty Property { get; set; }
        public UnitState State { get; set; }
        public UnitResource RemainResource { get; set; }
        public List<Skill.Skill> Skills { get; set; } = new();

        private void Awake()
        {
            Owner = gameObject;
        }

        public void SetHpProperty(int MaxHp)
        {
            var hp = Property.MaxHp;
            hp.BaseValue = MaxHp;
            var property = Property;
            property.MaxHp = hp;
            Property = property;

            var resource = RemainResource;
            resource.Hp = MaxHp;
            RemainResource = resource;
        }
    }
}