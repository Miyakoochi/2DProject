using DG.Tweening;
using UI.UICore;
using UnityEngine;

namespace UI.TweenTemplateComponent
{
    public class MoveTween : UITweener
    {
        public Vector3 StartPosition;
        public Vector3 CenterPosition;
        public Vector3 ExitPosition;

        public float MoveDuration;
        

        public override void EnterTween(TweenCallback onCompleted = null)
        {
            transform.localPosition = StartPosition;
            transform.DOLocalMove(CenterPosition, MoveDuration).OnComplete(onCompleted);
        }

        public override void ExitTween(TweenCallback onCompleted = null)
        {
            transform.DOLocalMove(ExitPosition, MoveDuration).OnComplete(onCompleted);
        }
    }
}