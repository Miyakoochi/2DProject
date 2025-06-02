using DG.Tweening;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.TweenTemplateComponent
{
    public class BoardTween : UITweener
    {
        [FormerlySerializedAs("StartPosition")] public Vector2 StartPositionOffset;
        public Vector2 CenterPosition;
        [FormerlySerializedAs("EndPosition")] public Vector2 EndPositionOffset;

        public float FallDutation = 0.8f;
        
        private RectTransform mRectTransform;
        private RectTransform mMainCanvasRect;

        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
            mMainCanvasRect = this.GetModel<IUIModel>().StaticUIs.GetComponent<RectTransform>();
        }

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            mRectTransform.anchoredPosition = new Vector2(0.0f, mMainCanvasRect.rect.height * 0.5f + StartPositionOffset.y);
            
            var boardSequence = DOTween.Sequence();
            
            boardSequence.Append(mRectTransform.DOAnchorPosY(CenterPosition.y, FallDutation)
                .SetEase(Ease.OutBounce)).OnComplete(onCompleted);
            
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            var boardSequence = DOTween.Sequence();
            
            boardSequence.Append(mRectTransform.DOAnchorPosY(mMainCanvasRect.rect.height * 0.5f + EndPositionOffset.y, FallDutation * 0.5f)
                .SetEase(Ease.OutCubic)).OnComplete(onCompleted);
        }
    }
}