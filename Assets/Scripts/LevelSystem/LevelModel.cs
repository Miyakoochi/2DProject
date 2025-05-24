using QFramework;
using UnityEngine;


namespace LevelSystem
{

    public interface ILevelModel : IModel
    {
        public GameObject CurrentLevelMap { get; set; }
        public LevelDataModel CurrentLevelDataModel { get; set; }
    }
    
    public class LevelModel : AbstractModel, ILevelModel
    {

        protected override void OnInit()
        {
        }
        
        public GameObject CurrentLevelMap { get; set; } = null;
        public LevelDataModel CurrentLevelDataModel { get; set; } = null;


        protected override void OnDeinit()
        {
            base.OnDeinit();
            
        }
    }
}