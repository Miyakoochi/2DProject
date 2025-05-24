using System.Collections.Generic;
using AssetSystem;
using ObjectPool;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Audio
{
    //todo 多音乐播放
    public interface IAudioSystem : ISystem
    {
        public void PlayBGM(EMusicType musicType);
        public void PlayAudioOnce(EMusicType musicType);

        public void EndCurrentBGMPlay(EMusicType musicType);

        public void LoadMusicDataModel();
    }
    
    
    public class AudioSystem : AbstractSystem, IAudioSystem
    {
        private IAddressableSystem mAddressableSystem;
        private IAudioModel mAudioModel;
        
        private AsyncOperationHandle<AudioDataModel> AudioHandle;
        protected override void OnInit()
        {
            mAddressableSystem = this.GetSystem<IAddressableSystem>();
            mAudioModel = this.GetModel<IAudioModel>();
        }

        public void PlayBGM(EMusicType musicType)
        {
            var music = mAudioModel.AudioAssets.GetAudioClip(musicType);
            if (music)
            {
                this.SendEvent<PlayBGMEvent>(new PlayBGMEvent()
                {
                    MusicType = musicType,
                    AudioClip = music
                });
            }
        }

        public void PlayAudioOnce(EMusicType musicType)
        {
            var clip = mAudioModel.AudioAssets.GetAudioClip(musicType);
            if (clip)
            {
                var audioObject = this.GetSystem<IObjectPoolSystem>().GetObject<AudioObject>();
                audioObject.SetAudio(clip);
            }
        }

        public void EndCurrentBGMPlay(EMusicType musicType)
        {
            this.SendEvent<EndMusicEvent>();
        }

        public async void LoadMusicDataModel()
        {
            if(AudioHandle.IsValid() && AudioHandle.Status == AsyncOperationStatus.Succeeded) return;
            
            AudioHandle = await mAddressableSystem.LoadAssetAsync<AudioDataModel>("AudioAssets");
            if (AudioHandle.Status == AsyncOperationStatus.Succeeded)
            {
                mAudioModel.AudioAssets = AudioHandle.Result;
                this.SendEvent<MusicAssetLoadEnd>();
            }
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();
            
            AudioHandle.Release();
        }
    }
}