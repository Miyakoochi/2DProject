using System.Collections.Generic;
using QFramework;

namespace GameAbilitySystem.Buff.Manager
{
    public interface IDamageModel : IModel
    {
        public List<DamageInfo> DamageInfos { get; set; }


    }
    
    public class DamageModel : AbstractModel, IDamageModel
    {
        
        
        protected override void OnInit()
        {
        }

        public List<DamageInfo> DamageInfos { get; set; } = new List<DamageInfo>();
    }
}