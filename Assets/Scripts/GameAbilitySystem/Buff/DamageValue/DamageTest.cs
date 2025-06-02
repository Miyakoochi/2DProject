using Command;
using Core.QFrameWork;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.DamageValue
{
    public class DamageTest : BaseController
    {
        private void FixedUpdate()
        {
            for (int i = 0; i < 100; i++)
            {                
                float x = Random.Range(0f, 1920.0f);
                float y = Random.Range(0f, 1080.0f);
                var position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0.0f));
                position.z = 0.0f;
                this.SendCommand(new CreateDamagePositionCommand(position, Mathf.Abs(Random.Range(0, 1000))));
            }
        }
    }
}