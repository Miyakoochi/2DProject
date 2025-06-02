using Core.QFrameWork;
using DG.Tweening;
using UnityEngine;

namespace UI.UICore
{
    public abstract class UITweener : BaseController
    {
        public abstract void EnterTween(TweenCallback onCompleted = null);

        public abstract void ExitTween(TweenCallback onCompleted = null);
    }
}