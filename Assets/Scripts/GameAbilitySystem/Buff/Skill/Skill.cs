using GameAbilitySystem.Buff.Buff;

namespace GameAbilitySystem.Buff.Skill
{
    ///<summary>
    /// 技能是角色拥有的东西，因为角色有技能，玩家或者ai才能操作角色释放技能
    /// 技能相当于TimeLine发送器
    ///</summary>
    public class Skill
    {
        ///<summary>
        ///技能的模板，创建于skillModel，但运行中还是会允许改变
        ///</summary>
        public SkillDataModel DataModel { get; set; }

        ///<summary>
        ///技能等级
        ///</summary>
        public int Level { get; set; }

        ///<summary>
        /// 冷却时间，单位秒。尽管游戏设计里面是没有冷却时间的，但是我们依然需要这个数据
        /// 因为作为一个ARPG子分类，和ARPG游戏有一样的问题：一次按键（时间够久）会发生连续多次使用技能，所以得有一个GCD来避免问题
        /// 当然和wow的gcd不同，这个“GCD”就只会让当前使用的技能进入0.1秒的冷却
        ///</summary>
        public double BuiltInCooldown { get; set; }

        public Skill(SkillDataModel dataModel, int level = 1)
        {
            this.DataModel = dataModel;
            this.Level = level;
            this.BuiltInCooldown = 0;
        }
    }
}