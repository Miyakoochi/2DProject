using System;

namespace GameAbilitySystem.Buff.Unit
{

    [Serializable]
    public struct UnitPropertyType
    {
        /// <summary>
        /// 基础数值
        /// </summary>
        public float BaseValue;
    
        /// <summary>
        /// 额外固定值
        /// </summary>
        public float AddonValue;

        /// <summary>
        /// 额外乘值
        /// </summary>
        public float AddonPercentValue;
    
        public float GetFinalValue() => BaseValue + AddonValue + AddonPercentValue * BaseValue;
        public int GetFinalValueInt() => (int)MathF.Ceiling(GetFinalValue());
    
        public UnitPropertyType(float baseValue)
        {
            BaseValue = baseValue;
            AddonValue = 0.0f;
            AddonPercentValue = 0;
        }

        public UnitPropertyType(float baseValue, float addonValue, float addonPercentValue)
        {
            BaseValue = baseValue;
            AddonValue = addonValue;
            AddonPercentValue = addonPercentValue;
        }

        public static UnitPropertyType operator +(UnitPropertyType lhs, UnitPropertyType rhs)
        {
            return new UnitPropertyType(lhs.BaseValue + rhs.BaseValue, 
                lhs.AddonValue + rhs.AddonValue,
                lhs.AddonPercentValue + rhs.AddonPercentValue);
        }
    }
}