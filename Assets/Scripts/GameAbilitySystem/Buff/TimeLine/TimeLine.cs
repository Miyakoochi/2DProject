using System.Collections.Generic;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff.TimeLine
{
    public class TimeLine
    {
        /// <summary>
        /// 策划填表设置
        /// </summary>
        public TimeLineDataModel DataModel { get; set; }

        /// <summary>
        /// 创建TimeLine的具体负责人 例如哪个unit对象
        /// </summary>
        public IGameAbilityUnit Caster { get; set; }

        /// <summary>
        /// 创建Timeline的具体参数 例如技能
        /// </summary>
        public object Param { get; set; }

        /// <summary>
        /// TimeLine的倍速
        /// 设置倍速最小值
        /// </summary>
        public float TimeScale
        {
            get => _timeScale;

            set { _timeScale = Mathf.Max(value, 0.1f); }
        }

        private float _timeScale = 1.0f;

        ///<summary>
        ///Timeline已经运行了多少秒了
        ///</summary>
        public float TimeElapsed = 0.0f;

        ///<summary>
        ///一些重要的逻辑参数，是根据游戏机制在程序层提供的。
        ///</summary>
        public Dictionary<string, object> Values = new Dictionary<string, object>();


        public TimeLine(TimeLineDataModel dataModel, IGameAbilityUnit caster, object param)
        {
            this.DataModel = dataModel;
            this.Caster = caster;
            this.Param = param;

            /*对不同的Caster对象提供逻辑额外参数
            if (caster){
                ChaState cs = caster.GetComponent<ChaState>();
                if (cs){
                    this.values.Add("faceDegree", cs.faceDegree);
                    this.values.Add("moveDegree", cs.moveDegree);
                }
                this._timeScale = cs.actionSpeed;
            }
            */
        }
    }
}