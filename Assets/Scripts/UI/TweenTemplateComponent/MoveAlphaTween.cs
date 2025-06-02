using DG.Tweening;
using UI.UICore;
using UnityEngine;

namespace UI.TweenTemplateComponent
{
    public class MoveAlphaTween : UITweener
    {
        public Vector3 StartPosition;
        public Vector3 CenterPosition;
        public Vector3 ExitPosition;

        public float MoveDuration = 0.5f;
        public float AlphaDuration = 0.5f;

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
            transform.localPosition = StartPosition;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(CenterPosition, MoveDuration)).OnComplete(onCompleted);
            if (mCanvasGroup)
            {
                sequence.Join(mCanvasGroup.DOFade(1.0f, AlphaDuration));
            }
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(ExitPosition, MoveDuration)).OnComplete(onCompleted);
            if (mCanvasGroup)
            {
                sequence.Join(mCanvasGroup.DOFade(0.0f, AlphaDuration));
            }
        }
    }
}