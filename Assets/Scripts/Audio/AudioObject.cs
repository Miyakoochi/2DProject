using Core;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using ObjectPool;
using QFramework;
using UnityEngine;

namespace Audio
{
    public class AudioObject : IGameObject, IPoolable<AudioDataModel>, IController
    {
        public GameObject Self { get; set; }

        private AudioSource mAudioSource;
        public AudioObject()
        {
            Self = new GameObject("Audio");

            mAudioSource = Self.AddComponent<AudioSource>();
            if (this.GetModel<IAudioModel>().AudioManager)
            {
                Self.transform.SetParent(this.GetModel<IAudioModel>().AudioManager);
            }
        }
        
        public void Set(AudioDataModel dataModel)
        {
            
        }

        public void Reset()
        {
            
        }

        public void SetAudio(AudioClip clip)
        {
            if (clip)
            {
                mAudioSource.clip = clip;
                mAudioSource.Play();
                AwaitMusicStop();
            }
        }

        private async void AwaitMusicStop()
        {
            while (mAudioSource.isPlaying == true)
            {
                await UniTask.Delay(10);
            }
            
            mAudioSource.Stop();
            this.GetSystem<IObjectPoolSystem>().ReleaseObject(this);
        }

        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }
    }
}