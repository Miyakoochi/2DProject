using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using QFramework;
using UnityEngine;

namespace UI
{
    public class TransitionUI : BaseController
    {
        public CanvasGroup CanvasGroup;
        public float fadeDuration = 0.3f;
        private void Awake()
        {
        }

        private void Start()
        {
            this.RegisterEvent<TransitionEndEvent>(OnTransitionEnd);
            this.RegisterEvent<TransitionStartEvent>(OnTransitionStart);
        }

        private void OnTransitionStart(TransitionStartEvent obj)
        {
            if (CanvasGroup)
            {
                CanvasGroup.alpha = 1;
            }
        }

        private void OnTransitionEnd(TransitionEndEvent obj)
        {
            if (CanvasGroup)
            {
                StartFadeOut();
            }
        }

        private void StartFadeOut()
        {
            CanvasGroup.DOFade(0, fadeDuration);
        }
        
    }
}