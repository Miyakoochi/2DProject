using QFramework;
using UnityEngine;


namespace LevelSystem
{

    public interface ILevelModel : IModel
    {
        public GameObject CurrentLevelMap { get; set; }
        public LevelDataModel CurrentLevelDataModel { get; set; }

        public bool BoundCamera { get; set; }
        public Vector2 LevelBoundLeftDown { get; set; }
        public Vector2 LevelBoundRightUp { get; set; }
    }
    
    public class LevelModel : AbstractModel, ILevelModel
    {

        protected override void OnInit()
        {
        }
        
        public GameObject CurrentLevelMap { get; set; } = null;
        public LevelDataModel CurrentLevelDataModel { get; set; } = null;
        public bool BoundCamera { get; set; } = false;
        public Vector2 LevelBoundLeftDown { get; set; }
        public Vector2 LevelBoundRightUp { get; set; }


        protected override void OnDeinit()
        {
            base.OnDeinit();
            
        }
    }
}