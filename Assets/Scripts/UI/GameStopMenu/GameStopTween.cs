using DG.Tweening;
using UI.UICore;
using UnityEngine;

namespace UI.GameStopMenu
{
    public class GameStopTween : UITweener
    {
        public float StartAlpha;
        public float TargetAlpha;
        public float Duration;

        public CanvasGroup CanvasGroup;
        
        public override void EnterTween(TweenCallback onCompleted = null)
        {
            if (CanvasGroup)
            {
                CanvasGroup.alpha = StartAlpha;
                CanvasGroup.DOFade(TargetAlpha, Duration).SetUpdate(true).OnComplete(onCompleted);
            } 
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            
        }
    }
}