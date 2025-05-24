using System.Collections;
using Core.QFrameWork;
using QFramework;
using TMPro;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TipsUI : BaseController
    {
        public AnimationCurve enterCurve; // 进入动画曲线
        public AnimationCurve exitCurve;  // 退出动画曲线
        public float moveDuration = 1.2f; // 单次动画时长
        public float stayDuration = 2f;   // 停留时间

        public Vector2 startPos;    // 起始位置（屏幕外）
        public Vector2 centerPos;   // 中心位置
        public Vector2 endPos;      // 结束位置（屏幕外）
        private RectTransform targetRect;
        private Coroutine animationCoroutine;

        public TextMeshProUGUI TextMesh;

        private IUISystem mUISystem;
        
        private void Awake()
        {
            this.RegisterEvent<ShowTipsEvent>(OnTipsShow);
            targetRect = GetComponent<RectTransform>();
            mUISystem = this.GetSystem<IUISystem>();
        }

        private void OnTipsShow(ShowTipsEvent obj)
        {
            if (TextMesh)
            {
                TextMesh.text = obj.Tips;
                StartAnimationSequence();
            }
        }

        void StartAnimationSequence()
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
        
            animationCoroutine = StartCoroutine(AnimationSequence());
        }
        IEnumerator AnimationSequence()
        {
            // 进入动画
            yield return StartCoroutine(MoveAnimation(startPos, 
                centerPos, 
                enterCurve));
            // 停留
            yield return new WaitForSeconds(stayDuration);
            // 退出动画
            yield return StartCoroutine(MoveAnimation(centerPos,
                endPos,
                exitCurve));
            
            mUISystem.SetUIShow(UIType.Tips, false);
        }
        IEnumerator MoveAnimation(Vector2 startPos, Vector2 endPos, AnimationCurve curve)
        {
            float time = 0;
        
            while (time < moveDuration)
            {
                time += Time.deltaTime;
                float t = curve.Evaluate(time / moveDuration);
                targetRect.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, t);
                yield return null;
            }
        
            targetRect.anchoredPosition = endPos;
        }
    }
}