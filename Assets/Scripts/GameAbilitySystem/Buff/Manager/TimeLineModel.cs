using System.Collections.Generic;
using QFramework;

namespace GameAbilitySystem.Buff.Manager
{
    public interface ITimeLineModel : IModel
    {
        public List<TimeLine.TimeLine> TimeLines { get; set; }

    }
    
    public class TimeLineModel : AbstractModel, ITimeLineModel
    {
        
        
        protected override void OnInit()
        {
        }

        public List<TimeLine.TimeLine> TimeLines { get; set; } = new List<TimeLine.TimeLine>();
    }
}