using QFramework;

namespace Pathfinding
{
    public interface IPathModel : IModel
    {
        public bool IsScentPathing { get; set; }
    }
    
    public class PathModel : AbstractModel, IPathModel
    {
        
        
        protected override void OnInit()
        {
            
        }

        public bool IsScentPathing { get; set; } = true;
    }
}