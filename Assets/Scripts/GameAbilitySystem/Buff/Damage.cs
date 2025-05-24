using System.Collections.Generic;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff
{
    ///<summary>
    ///游戏中任何一次伤害、治疗等逻辑，都会产生一条damageInfo，由此开始正常的伤害流程，而不是直接改写hp
    ///值得一提的是，在类似“攻击时产生额外一次伤害”这种效果中，额外一次伤害也应该是一个damageInfo。
    ///</summary>
    public class DamageInfo
    {
        ///<summary>
        ///造成伤害的攻击者，当然可以是null的
        ///</summary>
        public IGameAbilityUnit Attacker { get; set; }

        ///<summary>
        ///造成攻击伤害的受击者，这个必须有
        ///</summary>
        public IGameAbilityUnit Defender { get; set; }

        ///<summary>
        ///这次伤害的类型Tag，这个会被用于buff相关的逻辑，是一个极其重要的信息
        ///这里是策划根据游戏设计来定义的，比如游戏中可能存在"frozen" "fire"之类的伤害类型，还会存在"directDamage" "period" "reflect"之类的类型伤害
        ///根据这些伤害类型，逻辑处理可能会有所不同，典型的比如"reflect"，来自反伤的，那本身一个buff的作用就是受到伤害的时候反弹伤害，如果双方都有这个buff
        ///并且这个buff没有判断damageInfo.tags里面有reflect，则可能造成“短路”，最终有一下有一方就秒了。
        ///</summary>
        public List<DamageInfoTag> Tags = new List<DamageInfoTag>();

        ///<summary>
        ///伤害值，其实伤害值是多元的，通常游戏都会有多个属性伤害，所以会用一个struct，否则就会是一个int
        ///尽管起名叫Damage，但实际上治疗也是这个，只是负数叫做治疗量，这个起名看似不严谨，对于游戏（这个特殊的业务）而言却又是严谨的
        ///</summary>
        public Damage DamageObj { get; set; }

        ///<summary>
        ///是否命中，是否命中与是否暴击并不直接相关，都是单独的算法
        ///作为一个射击游戏，子弹命中敌人是一种技巧，所以在这里设计命中了还会miss是愚蠢的
        ///因此这里的hitRate始终是2，就是必定命中的，之所以把这个属性放着，也是为了说明问题，而不是这个属性真的对这个游戏有用。
        ///要不要这个属性还是取决于游戏设计，比如当前游戏，本不该有这个属性。
        ///</summary>
        public double HitRate { get; set; } = 2.00f;

        ///<summary>
        ///伤害过后，给角色添加的buff
        ///</summary>
        /// //TODO 可能要添加
        public List<AddBuffInfo> AddBuffs = new List<AddBuffInfo>();

        public DamageInfo(IGameAbilityUnit attacker, IGameAbilityUnit defender, Damage damage,
            List<DamageInfoTag> tags)
        {
            this.Attacker = attacker;
            this.Defender = defender;
            this.DamageObj = damage;

            if(tags == null)return;
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }
        
        public int DamageValue(bool asHeal)
        {
            return DamageObj.Bullet + DamageObj.Explosion + DamageObj.Mental;
            //return DesignerScripts.CommonScripts.DamageValue(this, asHeal);
        }
        
        public bool IsHeal()
        {
            for (int i = 0; i < this.Tags.Count; i++)
            {
                if (Tags[i] == DamageInfoTag.DirectHeal || Tags[i] == DamageInfoTag.PeriodHeal)
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///根据tag决定是否要播放受伤动作，当然你还可以是根据类型决定不同的受伤动作，但是我这个demo就没这么复杂了
        ///</summary>
        public bool RequireDoHurt()
        {
            for (int i = 0; i < this.Tags.Count; i++)
            {
                if (Tags[i] == DamageInfoTag.DirectDamage)
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///将添加buff信息添加到伤害信息中来
        ///buffOnHit\buffBeHurt\buffOnKill\buffBeKilled等伤害流程张的buff添加通常走这里
        ///<param name="buffInfo">要添加的buff的信息</param>
        ///</summary>
        public void AddBuffToCha(AddBuffInfo buffInfo)
        {
            this.AddBuffs.Add(buffInfo);
        }
    }

    ///<summary>
    ///游戏中伤害值的struct，这游戏的伤害类型包括子弹伤害（治疗）、爆破伤害（治疗）、精神伤害（治疗）3种，这两种的概念更像是类似物理伤害、金木水火土属性伤害等等这种元素伤害的概念
    ///但是游戏的逻辑可能会依赖于这个伤害做一些文章，比如“受到子弹伤害减少90%”之类的
    ///</summary>
    public struct Damage
    {
        public int Bullet;
        public int Explosion;
        public int Mental;

        public Damage(int bullet, int explosion = 0, int mental = 0)
        {
            this.Bullet = bullet;
            this.Explosion = explosion;
            this.Mental = mental;
        }

        ///<summary>
        ///统计规则，在这个游戏里伤害和治疗不能共存在一个结果里，作为抵消用
        ///<param name="asHeal">是否当做治疗来统计</name>
        ///</summary>
        public int Overall(bool asHeal = false)
        {
            return (asHeal == false)
                ? (Mathf.Max(0, Bullet) + Mathf.Max(0, Explosion) + Mathf.Max(0, Mental))
                : (Mathf.Min(0, Bullet) + Mathf.Min(0, Explosion) + Mathf.Min(0, Mental));
        }

        public static Damage operator +(Damage a, Damage b)
        {
            return new Damage(
                a.Bullet + b.Bullet,
                a.Explosion + b.Explosion,
                a.Mental + b.Mental
            );
        }

        public static Damage operator *(Damage a, float b)
        {
            return new Damage(
                Mathf.RoundToInt(a.Bullet * b),
                Mathf.RoundToInt(a.Explosion * b),
                Mathf.RoundToInt(a.Mental * b)
            );
        }
    }

    ///<summary>
    ///伤害类型的Tag元素，因为DamageInfo的逻辑需要的严谨性远高于其他的元素，所以伤害类型应该是枚举数组的
    ///这个伤害类型不应该是类似 火伤害、水伤害、毒伤害之类的，如果是这种元素伤害，那么应该是在damage做文章，即damange不是int而是一个struct或者array或者dictionary，然后DamageValue函数里面去改最终值算法
    ///这里的伤害类型，指的还是比如直接伤害、反弹伤害、dot伤害等等，一些在逻辑处理流程会有不同待遇的东西，比如dot伤害可能不会触发一些效果等，当然这最终还是取决于策划设计的规则。
    ///</summary>
    public enum DamageInfoTag
    {
        DirectDamage = 0, //直接伤害
        PeriodDamage = 1, //间歇性伤害
        ReflectDamage = 2, //反噬伤害
        DirectHeal = 10, //直接治疗
        PeriodHeal = 11, //间歇性治疗
        MonkeyDamage = 9999 //这个类型的伤害在目前这个demo中没有意义，只是告诉你可以随意扩展，仅仅比string严肃些。
    }
}