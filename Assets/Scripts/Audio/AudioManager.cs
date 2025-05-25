using Core.QFrameWork;
using QFramework;
using UnityEngine;

namespace Audio
{
    public class AudioManager : BaseController
    {
        private IAudioSystem mAudioSystem;
        private AudioSource mAudioSource; 
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            mAudioSource = GetComponent<AudioSource>();
            mAudioSystem = this.GetSystem<IAudioSystem>();
        }

        private void Start()
        {
            this.RegisterEvent<PlayBGMEvent>(OnMusicPlay).UnRegisterWhenDisabled(this);
            this.RegisterEvent<EndMusicEvent>(OnEndMusic).UnRegisterWhenDisabled(this);
            this.GetModel<IAudioModel>().AudioManager = transform;
        }

        private void OnEndMusic(EndMusicEvent obj)
        {
            if(!mAudioSource) return;
            mAudioSource.Stop();
        }

        private void OnMusicPlay(PlayBGMEvent obj)
        {
            if(!mAudioSource) return;

            mAudioSource.clip = obj.AudioClip;
            mAudioSource.loop = true;
            mAudioSource.Play();
        }
    }
}