using System;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAbilitySystem.Buff.DamageValue
{
    public class DamageValue : BaseController
    {
        public Vector3 EndpositionOffset;
        public Vector3 StartScale;
        public Vector3 EndScale;

        public float Duration;
        
        private TextMeshPro mTextMeshPro;
        
        private void Awake()
        {
            mTextMeshPro = GetComponent<TextMeshPro>();
        }


        public async void SetValue(Vector3 position, int value, TweenCallback onTweenComplete)
        {
            transform.position = position;
            transform.localScale = StartScale;
            mTextMeshPro.text = value.ToString();

            await UniTask.WaitForSeconds(1.0f);
            onTweenComplete?.Invoke();
            /*var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(position + EndpositionOffset, Duration).SetEase(Ease.OutCirc).OnComplete(onTweenComplete));
            sequence.Join(transform.DOScale(EndScale, Duration).SetEase(Ease.OutBack));*/
        }
    }
}