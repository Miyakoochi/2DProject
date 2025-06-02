using Core.QFrameWork;
using UnityEngine;

namespace GameAbilitySystem.Buff.DamageValue
{
    public class DamageParticle : BaseController
    {
        public ParticleSystem ParticleSystem;

        private ParticleSystem.EmitParams mEmitParams;
        
        private void Awake()
        {
            ParticleSystem.Stop();
            mEmitParams.velocity = Vector3.up * 5.0f;
            mEmitParams.startLifetime = 1.0f;
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ParticleSystem.isPlaying == true)
                {
                    ParticleSystem.Stop();
                }
                else
                {
                    ParticleSystem.Play();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var position = Input.mousePosition;
                position = Camera.main.ScreenToWorldPoint(position);
                position.z = 0.0f;
                
                EmitDamage(position, Random.Range(0, 10000));

            }



            
        }*/

        /*private void FixedUpdate()
        {
            for (int i = 0; i < 10000; i++)
            {
                float x = Random.Range(0f, 1920.0f);
                float y = Random.Range(0f, 1080.0f);
                var position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0.0f));
                position.z = 0.0f;
                EmitDamage(position, Random.Range(0, 1000));
            }
        }*/

        public void EmitDamage(Vector3 position, int value)
        {
            var color = ChangeDamageValueToColor(value);
            int count = CaculateNumberDigitCount(value);
            
            var vector3 = mEmitParams.startSize3D;
            vector3.x = count * 0.5f;
            vector3.y = 1;
            vector3.z = 1;
            mEmitParams.startSize3D = vector3 * 0.5f;

            //position = Camera.main.ScreenToWorldPoint(position);
            mEmitParams.position = position;
            mEmitParams.startColor = color;
            mEmitParams.velocity = Vector3.up * 3.0f;
            ParticleSystem.Emit(mEmitParams, 1);
        }

        /// <summary>
        /// 将伤害数值转换为颜色 传递给Shader
        /// </summary>
        private Color32 ChangeDamageValueToColor(int value)
        {
            byte r = (byte)((value >> (6 * 4)) & 0xFF);
            byte g = (byte)((value >> (4 * 4)) & 0xFF);
            byte b = (byte)((value >> (2 * 4)) & 0xFF);
            byte a = (byte)((value >> (0 * 4)) & 0xFF);
            
            /*int ra = Mathf.FloorToInt(r);
            int ga = Mathf.FloorToInt(g);
            int ba = Mathf.FloorToInt(b);
            int aa = Mathf.FloorToInt(a);
            var damage = (ra << 24) & 0xFF000000 | (ga << 16) & 0xFF0000 | (ba << 8) & 0xFF00 | aa & 0xFF; 
            Debug.Log(damage);*/
            
            //Debug.Log($"{r}:{g}:{b}:{a}");
            return new Color32(r, g, b, a);
        }
        
        private int CaculateNumberDigitCount(int value)
        {
            int count = 1;

            while (value > 9)
            {
                count++;
                value /= 10;
            }

            return count;
        }
    }
}