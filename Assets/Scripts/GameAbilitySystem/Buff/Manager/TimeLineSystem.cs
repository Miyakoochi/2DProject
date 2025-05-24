using System.Collections.Generic;
using GameAbilitySystem.Buff.TimeLine;
using GameAbilitySystem.Buff.Unit;
using QFramework;

namespace GameAbilitySystem.Buff.Manager
{
    public interface ITimeLineSystem : ISystem
    {
        public void AddTimeLine(TimeLine.TimeLine timeLine);
        public void AddTimeLine(TimeLineDataModel timeLineDataModel, IGameAbilityUnit caster, object source);
    }

    public partial class TimeLineSystem : AbstractSystem, ITimeLineSystem
    {
        private ITimeLineModel mTimeLineModel;
        
        protected override void OnInit()
        {
            mTimeLineModel = this.GetModel<ITimeLineModel>();
        }

        public void AddTimeLine(TimeLineDataModel timeLineDataModel, IGameAbilityUnit caster, object source)
        {
            if (CasterHasTimeLine(caster) == true)
            {
                return;
            }

            mTimeLineModel.TimeLines.Add(new TimeLine.TimeLine(timeLineDataModel, caster, source));
        }

        /// <summary>
        /// 添加一个TimeLine
        /// 同一个Caster只能有一个TimeLine在作用
        /// </summary>
        /// <param name="timeLine"></param>
        public void AddTimeLine(TimeLine.TimeLine timeLine)
        {
            if (timeLine.Caster != null && CasterHasTimeLine(timeLine.Caster) == true)
            {
                return;
            }

            mTimeLineModel.TimeLines.Add(timeLine);
        }

        /// <summary>
        /// 判断TimeLines中有没有对应的Caster
        /// </summary>
        /// <param name="Caster"></param>
        /// <returns></returns>
        private bool CasterHasTimeLine(object Caster)
        {
            foreach (var timeLine in mTimeLineModel.TimeLines)
            {
                if (timeLine.Caster == Caster)
                {
                    return true;
                }
            }

            return false;
        }
    }
}