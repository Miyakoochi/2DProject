using UnityEngine;

namespace Audio
{
     public struct MusicAssetLoadEnd
     {
          
     }
     
     public struct PlayBGMEvent
     {
          public EMusicType MusicType;
          public AudioClip AudioClip;
     }
     
     public struct EndMusicEvent
     {
     }
}