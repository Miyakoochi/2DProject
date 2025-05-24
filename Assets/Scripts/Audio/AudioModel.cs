using QFramework;
using UnityEngine;

namespace Audio
{
    public interface IAudioModel : IModel
    {
        public AudioDataModel AudioAssets { get; set; }
        public Transform AudioManager { get; set; }
    }
    
    public class AudioModel : AbstractModel, IAudioModel
    {
        
        protected override void OnInit()
        {
            
        }

        
        public AudioDataModel AudioAssets { get; set; }
        public Transform AudioManager { get; set; }
    }
}