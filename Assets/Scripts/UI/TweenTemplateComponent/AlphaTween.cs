using DG.Tweening;
using UI.UICore;
using UnityEngine;

namespace UI.TweenTemplateComponent
{
    public class AlphaTween : UITweener
    {
        private CanvasGroup mCanvasGroup;

        public float Duration = 0.5f;
        private void Awake()
        {
            mCanvasGroup = GetComponent<CanvasGroup>();
        }

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            if (mCanvasGroup)
            {
                mCanvasGroup.alpha = 0.0f;
                mCanvasGroup.DOFade(1.0f, Duration).OnComplete(onCompleted);
            }
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            if (mCanvasGroup)
            {
                mCanvasGroup.DOFade(0.0f, Duration).OnComplete(onCompleted);
            }
        }
    }
}