using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.TimeLine;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff.Skill
{
    ///<summary>
    ///策划填表的技能
    ///</summary>
    
    [CreateAssetMenu(menuName = "DataModel/Buff/SkillDataModel")]
    public class SkillDataModel : ScriptableObject
    {
        ///<summary>
        ///技能的id
        ///</summary>
        public string Id;

        ///<summary>
        ///技能使用的条件，这个游戏中只有资源需求，比如hp、ammo之类的
        ///</summary>
        public UnitResource Condition;

        ///<summary>
        ///技能的消耗，成功之后会扣除这些资源
        ///</summary>
        public UnitResource Cost;

        ///<summary>
        ///技能的效果，必然是一个timeline
        ///</summary>
        public TimeLineDataModel Effect;

        ///<summary>
        ///学会技能的时候，同时获得的buff
        ///</summary>
        public AddBuffInfo[] Buff;

        public SkillDataModel(string id, UnitResource cost, UnitResource condition, string effectTimeline, AddBuffInfo[] buff)
        {
            this.Id = id;
            this.Cost = cost;
            this.Condition = condition;
            //this.Effect = DesingerTables.Timeline.data[effectTimeline]; //SceneVariants.desingerTables.timeline.data[effectTimeline];
            this.Buff = buff;
        }
    }
}