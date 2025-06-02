using DG.Tweening;
using QFramework;
using UI.UICore;
using UnityEngine;

namespace UI.TweenTemplateComponent
{
    public class BoardAlphaUpdateTween : UITweener
    {
        public float StartAlpha;
        public float EndAlpha;

        public Vector2 StartPositionOffset;
        public Vector2 CenterPosition;
        public Vector2 EndPositionOffset;

        public float Duration;
        
        private RectTransform mMainCanvasRect;
        private CanvasGroup mCanvasGroup;
        public void Awake()
        {
            mMainCanvasRect = this.GetModel<IUIModel>().StaticUIs.GetComponent<RectTransform>();
            mCanvasGroup = GetComponent<CanvasGroup>();
        }

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            transform.localPosition = new Vector3(0.0f,  mMainCanvasRect.rect.height * 0.5f + StartPositionOffset.y, 0.0f);
            mCanvasGroup.alpha = StartAlpha;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(CenterPosition, Duration).SetEase(Ease.OutBounce).OnComplete(onCompleted))
                .Join(mCanvasGroup.DOFade(EndAlpha, Duration)).SetUpdate(true);
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMove(new Vector3(0.0f, EndPositionOffset.y + mMainCanvasRect.rect.height * 0.5f, 0.0f), Duration).OnComplete(onCompleted))
                .Join(mCanvasGroup.DOFade(StartAlpha, Duration)).SetUpdate(true);
        }
    }
}