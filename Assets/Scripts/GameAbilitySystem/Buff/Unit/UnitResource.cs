using System;

namespace GameAbilitySystem.Buff.Unit
{
    //TODO 资源消耗需要优化 目前只能消耗固定值
    [Serializable]
    public struct UnitResource
    {
        /// <summary>
        /// 当前生命值
        /// </summary>
        public float Hp;

        public UnitResource(float hp)
        {
            Hp = hp;
        }

        public bool Enough(UnitResource requirement)
        {
            return Hp >= requirement.Hp;
        }

        public static UnitResource operator+(UnitResource a, UnitResource b)
        {
            return new UnitResource(a.Hp + b.Hp);
        }

        public static UnitResource operator -(UnitResource a, UnitResource b)
        {
            return new UnitResource(a.Hp - b.Hp);
        }

        public static UnitResource operator *(UnitResource a, float b)
        {
            return new UnitResource(a.Hp * b);
        }
        
        public static UnitResource operator *(float a, UnitResource b)
        {
            return new UnitResource(b.Hp * a);
        }
    }
}

