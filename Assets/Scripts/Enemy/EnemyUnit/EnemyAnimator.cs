using Core.QFrameWork;
using UnityEngine;

namespace Enemy.EnemyUnit
{
    public class EnemyAnimator : BaseController
    {
        public Animator Animator;
        private EnemyUnit Unit;
        
        private static readonly int Speed = Animator.StringToHash("EnemySpeed");
        private static readonly int FaceAngle = Animator.StringToHash("EnemyFaceAngle");

        private float mScaleX;
        private void Awake()
        {
            Unit = GetComponent<EnemyUnit>();
            mScaleX = transform.localScale.x;
        }

        public void UpdateAnimator()
        {
            if(!Unit) return;
            //Debug.Log($"Unit.Direction: {Unit.Direction}");
            float angle = Vector2.SignedAngle(Vector2.right, Unit.Direction);
            if (angle > 135.0f || angle < -135.0f)
            {
                var scale = transform.localScale;
                scale.x = -mScaleX;
                transform.localScale = scale;
            }
            else
            {
                var scale = transform.localScale;
                scale.x = mScaleX;
                transform.localScale = scale;
            }
            Animator?.SetFloat(FaceAngle, angle);
            float speedValue = Unit.Direction.normalized.magnitude;
            Animator?.SetFloat(Speed, speedValue);
            //Debug.Log($"Angle: {angle}, Speed: {speedValue}");
        }

        private void Update()
        {
            UpdateAnimator();
        }
    }
}