using Core.QFrameWork;
using CsLua;
using GameAbilitySystem.Buff.TimeLine;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.Manager
{
    public class TimeLineManager : BaseController
    {
        private ITimeLineModel mTimeLineModel;
        private ILuaSystem mLuaSystem;

        private void Awake()
        {
            mTimeLineModel = this.GetModel<ITimeLineModel>();
            mLuaSystem = this.GetSystem<ILuaSystem>();
        }

        private void FixedUpdate()
        {
            if(mTimeLineModel == null || mTimeLineModel.TimeLines == null) return;
            if (mTimeLineModel.TimeLines.Count <= 0)
            {
                return;
            }

            int index = 0;
            while (index < mTimeLineModel.TimeLines.Count)
            {
                var timeLineObj = mTimeLineModel.TimeLines[index];

                double wasTimeElapsed = timeLineObj.TimeElapsed;
                timeLineObj.TimeElapsed += Time.fixedDeltaTime * timeLineObj.TimeScale;

                if (timeLineObj.DataModel.ChargeGoBack.AtDuration >= wasTimeElapsed &&
                    timeLineObj.DataModel.ChargeGoBack.AtDuration < timeLineObj.TimeElapsed)
                {
                    if (timeLineObj.Caster != null)
                    {
                        if ( /*判断是否正在蓄力*/false)
                        {
                            timeLineObj.TimeElapsed = timeLineObj.DataModel.ChargeGoBack.GotoDuration;
                            continue;
                        }
                    }
                }

                //执行时间点内部的节点
                foreach (TimeLineNode timeNode in timeLineObj.DataModel.Nodes)
                {
                    if (timeNode.TimeElapsed >= wasTimeElapsed && timeNode.TimeElapsed < timeLineObj.TimeElapsed)
                    {
                        var function = mLuaSystem.GetLuaFunctionToDelegate<TimeLineEvent>(timeNode.DoEvent);
                        function?.Invoke(timeLineObj, timeNode.Parma);
                    }
                }

                //判断TimeLine是否终结
                if (timeLineObj.DataModel.Duration < timeLineObj.TimeElapsed)
                {
                    mTimeLineModel.TimeLines.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }
    }
}