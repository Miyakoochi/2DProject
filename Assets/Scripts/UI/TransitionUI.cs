using Core.QFrameWork;
using Cysharp.Threading.Tasks;
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
            DontDestroyOnLoad(gameObject);
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

        private void StartFadeIn()
        {
            Fade(0, 1);
        }

        private void StartFadeOut()
        {
            Fade(1, 0);
        }

        private async void Fade(float startAlpha, float targetAlpha)
        {
            float time = 0;
            while (time < fadeDuration)
            {
                CanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                time += Time.deltaTime;
                await UniTask.Yield();
            }
            CanvasGroup.alpha = targetAlpha;
        }
    }
}