using Core.QFrameWork;
using UnityEngine;

namespace Enemy.EnemyUnit
{
    public class EnemyAnimator : BaseController
    {
        public Animator Animator;

        private SpriteRenderer mSpriteRenderer;
        private EnemyUnit Unit;
        
        private static readonly int Speed = Animator.StringToHash("EnemySpeed");
        private static readonly int FaceAngle = Animator.StringToHash("EnemyFaceAngle");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int BeHurtTag = Animator.StringToHash("BeHurtTag");
        
        private void Awake()
        {
            Unit = GetComponent<EnemyUnit>();
            mSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateAnimator()
        {
            if(!Unit) return;
            
            float angle = Vector2.SignedAngle(Vector2.right, Unit.Direction);
            if (angle >= 135.0f || angle <= -135.0f)
            {
                mSpriteRenderer.flipX = true;
            }
            else
            {
                mSpriteRenderer.flipX = false; 
            }
            
            Animator?.SetBool(IsHurt, Unit.BeHurt);
            Animator?.SetFloat(FaceAngle, angle);
            float speedValue = Unit.Direction.normalized.magnitude;
            Animator?.SetFloat(Speed, speedValue);

            var state = Animator?.GetCurrentAnimatorStateInfo(0);
            if (state != null && state.Value.tagHash == BeHurtTag && state.Value.normalizedTime >= 1.0f)
            {
                Unit.BeHurt = false;
            }
        }

        private void Update()
        {
            UpdateAnimator();
        }
    }
}