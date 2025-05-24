
using System;

namespace GameAbilitySystem.Buff.Unit
{
    [Serializable]
    public struct UnitProperty
    {
        /// <summary>
        /// 最大生命值
        /// </summary>
        public UnitPropertyType MaxHp;
    
        /// <summary>
        /// 攻击力
        /// </summary>
        public UnitPropertyType Attack;

        /// <summary>
        /// 平时移动速度
        /// </summary>
        public UnitPropertyType ConstantSpeed;
    
        public UnitProperty(float baseMaxHp, float attack, float constantSpeed = 100.0f)
        {
            MaxHp = new UnitPropertyType(baseMaxHp);
            Attack = new UnitPropertyType(attack);
            ConstantSpeed = new UnitPropertyType(constantSpeed);
        }

        public UnitProperty(UnitPropertyType maxHp, UnitPropertyType attack, UnitPropertyType constantSpeed)
        {
            MaxHp = maxHp;
            Attack = attack;
            ConstantSpeed = constantSpeed;
        }

        public static UnitProperty operator +(UnitProperty a, UnitProperty b)
        {
            return new UnitProperty(a.MaxHp + b.MaxHp,
                a.Attack + b.Attack, a.ConstantSpeed + b.ConstantSpeed);
        }
    
        public static UnitProperty Zero = new UnitProperty(0, 0);
    }
}