using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameAbilitySystem.Buff.TimeLine
{
    /// <summary>
    /// 这些都是策划填表（外部设置好）的内容
    /// </summary>
    [CreateAssetMenu(menuName = "DataModel/Buff/TimeLineDataModel")]
    public class TimeLineDataModel : ScriptableObject
    {
        /// <summary>
        /// TimeModel标识
        /// </summary>
        [InspectorName("时间轴ID")]
        public string Id;

        /// <summary>
        /// TimeLine运行多久后发生动作的节点
        /// </summary>
        [InspectorName("时间轴节点列表")]
        public List<TimeLineNode> Nodes;

        /// <summary>
        /// TimeLine一共多长
        /// </summary>
        [InspectorName("时间轴持续时间")]
        public double Duration;

        ///<summary>
        ///如果有caster，并且caster处于蓄力状态，则可能会经历跳转点
        ///</summary>
        [InspectorName("时间轴跳转点")]
        public TimeLineGoTo ChargeGoBack;
    }

    /// <summary>
    /// TimeLine节点 TimeLine经过多少秒后执行的事件
    /// </summary>
    
    [Serializable]
    public struct TimeLineNode
    {
        /// <summary>
        /// 过多少秒后执行
        /// </summary>
        [InspectorName("过多少秒后执行")]
        public double TimeElapsed;

        /// <summary>
        /// 要执行的脚本函数
        /// </summary>
        [InspectorName("执行的脚本函数名称")]
        public string DoEvent;

        /// <summary>
        /// 事件的参数 TODO ????
        /// </summary>
        public List<object> Parma;
        
    }
    
    
    public struct TimeLineGoTo
    {
        /// <summary>
        /// 自身位于的时间点
        /// </summary>
        public float AtDuration;

        /// <summary>
        /// 跳向的时间点
        /// </summary>
        public float GotoDuration;

    }

    /// <summary>
    /// TimeLine执行事件
    /// 其中TimeLine对象为TimeLineNode的父对象
    /// </summary>
    public delegate void TimeLineEvent(TimeLine timeLine, params List<object>[] parmas);
}