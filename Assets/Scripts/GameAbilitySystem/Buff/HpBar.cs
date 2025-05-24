using Core.QFrameWork;
using UnityEngine.UI;

namespace GameAbilitySystem.Buff
{
    public class HpBar : BaseController
    {
        public Slider HpProcessBar;
        private BuffState mBuffState;
        private void Awake()
        {
            mBuffState =  GetComponent<BuffState>();
        }

        private void Update()
        {
            if(!HpProcessBar || !mBuffState) return;
            HpProcessBar.value = mBuffState.RemainResource.Hp / mBuffState.Property.MaxHp.GetFinalValue();
        }

        private void FixedUpdate()
        {
            if(!mBuffState) return;
            
            if (mBuffState.RemainResource.Hp < 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}