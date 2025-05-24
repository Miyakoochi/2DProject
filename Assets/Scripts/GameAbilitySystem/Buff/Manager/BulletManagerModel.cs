using System.Collections.Generic;
using GameAbilitySystem.Buff.Apply.Bullet;
using QFramework;

namespace GameAbilitySystem.Buff.Manager
{
    public interface IBulletManagerModel : IModel
    {
        public List<BulletUnit> UpdateBulletUnits { get; set; }
    }
    
    public class BulletManagerModel : AbstractModel, IBulletManagerModel
    {
        protected override void OnInit()
        {
            
        }
        
        public List<BulletUnit> UpdateBulletUnits { get; set; } = new();
    }
}