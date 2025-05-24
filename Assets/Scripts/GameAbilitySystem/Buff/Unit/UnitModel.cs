using System.Collections.Generic;
using QFramework;

namespace GameAbilitySystem.Buff.Unit
{
    public interface IUnitModel : IModel
    {
        public List<IGameAbilityUnit> Units { get; set; }
    }
    
    public class UnitModel : AbstractModel, IUnitModel
    {
        
        
        protected override void OnInit()
        {
        }

        public List<IGameAbilityUnit> Units { get; set; } = new List<IGameAbilityUnit>();
    }
}