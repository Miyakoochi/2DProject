using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMusicType
{
    Click,
    Switch,
    MainMenu,
    Game,
    Attack,
    EnemyBeHit
}

[Serializable]
public struct MusicAsset
{
    public EMusicType MusicType;
    public AudioClip AudioAsset;
}

[CreateAssetMenu(menuName = "AudioDataModel")]
public class AudioDataModel : ScriptableObject
{
    public List<MusicAsset> MusicAssets = new();

    private Dictionary<EMusicType, AudioClip> mAudioClips = new();
    private void OnEnable()
    {
        foreach (var music in MusicAssets)
        {
            mAudioClips.TryAdd(music.MusicType, music.AudioAsset);
        }
    }

    public AudioClip GetAudioClip(EMusicType musicType)
    {
        return mAudioClips.GetValueOrDefault(musicType);
    }
}
