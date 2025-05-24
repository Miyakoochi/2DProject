using System.Collections.Generic;
using QFramework;

namespace Enemy.EnemyManager.Model
{
    public interface IEnemyManagerModel : IModel
    {
        public List<EnemyUnit.EnemyUnit> UpdateEnemyUnits { get; set; }
    }
    
    public class EnemyManagerModel : AbstractModel, IEnemyManagerModel
    {
        public List<EnemyUnit.EnemyUnit> UpdateEnemyUnits { get; set; } = new();
        
        protected override void OnInit()
        {
        }
    }
}