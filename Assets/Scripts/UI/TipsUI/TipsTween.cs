using DG.Tweening;
using UI.UICore;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.TipsUI
{
    public class TipsTween : UITweener
    {
        public RectTransform rectTransform;
        public Vector3 StartPosition;
        public Vector3 CenterPosition;
        public Vector3 ExitPosition;

        public float MoveDuration = 0.5f;

        private CanvasGroup mCanvasGroup;
        
        private void Awake()
        {
            mCanvasGroup = GetComponent<CanvasGroup>();
            mCanvasGroup.alpha = 0;
            mCanvasGroup.interactable = false;
            mCanvasGroup.blocksRaycasts = false;
        }

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            rectTransform.anchoredPosition = StartPosition;
            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPos(CenterPosition, MoveDuration)).OnComplete(onCompleted);
            if (mCanvasGroup)
            {
                sequence.Join(mCanvasGroup.DOFade(1.0f, MoveDuration));
            }

            sequence.AppendInterval(1.0f);
            
            sequence.Append(rectTransform.DOAnchorPos(ExitPosition, MoveDuration));
            if (mCanvasGroup)
            {
                sequence.Join(mCanvasGroup.DOFade(0.0f, MoveDuration));
            }
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
        }
    }
}