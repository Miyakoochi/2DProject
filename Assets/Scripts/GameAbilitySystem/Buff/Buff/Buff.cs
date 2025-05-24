using System.Collections.Generic;
using GameAbilitySystem.Buff.Unit;

namespace GameAbilitySystem.Buff.Buff
{
    ///<summary>
    ///用于添加一条buff的信息
    ///</summary>
    public struct AddBuffInfo
    {
        ///<summary>
        ///buff的负责人是谁，可以是null
        ///</summary>
        public IGameAbilityUnit Caster;

        ///<summary>
        ///buff要添加给谁，这个必须有
        ///</summary>
        public IGameAbilityUnit Target;

        ///<summary>
        ///buff的model，这里当然可以从数据里拿，也可以是逻辑脚本现生成的
        ///</summary>
        public BuffDataModel BuffDataModel;

        ///<summary>
        ///要添加的层数，负数则为减少
        ///</summary>
        public int AddStack;

        ///<summary>
        ///是否是一个永久的buff，即便=true，时间设置也是有意义的，因为时间如果被减少到0以下，即使是永久的也会被删除
        ///</summary>
        public bool Permanent;

        ///<summary>
        ///关于时间，是改变还是设置为, true代表设置为，false代表改变
        ///</summary>
        public bool DurationSetTo;

        ///<summary>
        ///时间值，设置为这个值，或者加上这个值，单位：秒
        ///</summary>
        public float Duration;

        ///<summary>
        ///buff的一些参数，这些参数是逻辑使用的，比如wow中牧师的盾还能吸收多少伤害，就可以记录在buffParam里面
        ///</summary>
        public Dictionary<string, object> BuffParam;

        public AddBuffInfo(
            BuffDataModel dataModel, IGameAbilityUnit caster, IGameAbilityUnit target,
            int stack, float duration, bool durationSetTo = true,
            bool permanent = false,
            Dictionary<string, object> buffParam = null
        )
        {
            this.BuffDataModel = dataModel;
            this.Caster = caster;
            this.Target = target;
            this.AddStack = stack;
            this.Duration = duration;
            this.DurationSetTo = durationSetTo;
            this.BuffParam = buffParam;
            this.Permanent = permanent;
        }
    }

    public class Buff
    {
        public BuffDataModel DataModel { get; set; }

        /// <summary>
        /// 是否是一个永久的buff，永久的duration不会减少，但是timeElapsed还会增加
        /// </summary>
        public bool Permanent { get; set; }

        /// <summary>
        /// Buff剩余多久
        /// </summary>
        public double TimeRemaining { get; set; }

        /// <summary>
        /// Buff存在的时长
        /// </summary>
        public double TimeElapsed { get; set; } = 0.0;

        /// <summary>
        /// Buff执行OnTicked的次数 没有则为0
        /// </summary>
        public int Ticked { get; set; } = 0;

        /// <summary>
        /// 制造这个Buff的游戏对象
        /// </summary>
        public IGameAbilityUnit Caster { get; set; }

        /// <summary>
        /// buff的携带者，实际上是作为参数传递给脚本用，具体是谁，可定是所在控件的this.gameObject了
        /// </summary>
        public IGameAbilityUnit Carrier { get; set; }

        /// <summary>
        /// Buff当前层数
        /// </summary>
        public int Stack { get; set; } = 0;

        public Dictionary<string, object> BuffParams = new Dictionary<string, object>();

        public Buff(BuffDataModel dataModel, double timeRemaining,
            IGameAbilityUnit caster, IGameAbilityUnit carrier, int stack,
            bool permanent = false, Dictionary<string, object> buffParams = null)
        {
            DataModel = dataModel;
            TimeRemaining = timeRemaining;
            Caster = caster;
            Carrier = carrier;
            Stack = stack;
            Permanent = permanent;

            if (buffParams == null) return;
            foreach (var buffParam in buffParams)
            {
                BuffParams.Add(buffParam.Key, buffParam.Value);
            }
        }
    }
}