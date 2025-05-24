using System;

namespace GameAbilitySystem.Buff.Unit
{
    [Serializable]
    public struct UnitState
    {
        /// <summary>
        /// 单位是否死亡
        /// </summary>
        public bool IsDead;

        /// <summary>
        /// 单位无敌时间
        /// </summary>
        public double ImmuneTime;

        /// <summary>
        /// 单位阵营
        /// </summary>
        public int Side;

        /// <summary>
        /// 是否在蓄力
        /// </summary>
        public bool IsCharging;
    }
}