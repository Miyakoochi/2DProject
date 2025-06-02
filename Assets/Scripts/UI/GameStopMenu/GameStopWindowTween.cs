using DG.Tweening;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.GameStopMenu
{
    public class GameStopWindowTween : UITweener
    {
        [FormerlySerializedAs("StartPosition")] public float StartPositionOffset;
        public Vector3 TargetPosition;

        public float Duration;
        private RectTransform mMainCanvasRect;

        private void Awake()
        {
            mMainCanvasRect = this.GetModel<IUIModel>().StaticUIs.GetComponent<RectTransform>();
        }

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            if (mMainCanvasRect)
            {
                transform.localPosition = new Vector3(0.0f, mMainCanvasRect.rect.height * 0.5f + StartPositionOffset, 0.0f);
                transform.DOLocalMove(TargetPosition, Duration).SetUpdate(true).SetEase(Ease.OutBounce).OnComplete(onCompleted);
            }
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
        }
    }
}