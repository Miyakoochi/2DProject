using GameAbilitySystem.Buff.Tags;
using UnityEngine;

namespace GameAbilitySystem.Buff.Apply.AOE
{
    ///<summary>
    ///AoE的模板数据
    ///</summary>
    
    [CreateAssetMenu(menuName = "DataModel/Buff/AoeDataModel")]
    public class AoeDataModel : ScriptableObject
    {
        public string id;

        ///<summary>
        ///aoe的视觉特效，如果是空字符串，就不会添加视觉特效
        ///这里需要的是在Prefabs/下的路径，因为任何东西都可以是视觉特效
        ///</summary>
        public string prefab;

        ///<summary>
        ///aoe是否碰撞到阻挡就摧毁了（removed），如果不是，移动就是smooth的，如果移动的话……
        ///</summary>
        public bool removeOnObstacle;


        /// <summary>
        /// aoe的tag
        /// </summary>
        public GameTag[] tags;

        ///<summary>
        ///aoe每一跳的时间，单位：秒
        ///如果这个时间小于等于0，或者没有onTick，则不会执行aoe的onTick事件
        ///</summary>
        public float tickTime;

        ///<summary>
        ///aoe创建时的事件
        ///</summary>
        public string OnCreate;

        ///<summary>
        ///aoe创建的参数
        ///</summary>
        public object[] OnCreateParams;

        ///<summary>
        ///aoe每一跳的事件，如果没有，就不会发生每一跳
        ///</summary>
        public string OnTick;

        public object[] OnTickParams;

        ///<summary>
        ///aoe结束时的事件
        ///</summary>
        public string OnRemoved;

        public object[] OnRemovedParams;

        ///<summary>
        ///有角色进入aoe时的事件，onCreate时候位于aoe范围内的人不会触发这个，但是在onCreate里面会已经存在
        ///</summary>
        public string OnChaEnter;

        public object[] OnChaEnterParams;

        ///<summary>
        ///有角色离开aoe结束时的事件
        ///</summary>
        public string OnChaLeave;

        public object[] OnChaLeaveParams;

        ///<summary>
        ///有子弹进入aoe时的事件，onCreate时候位于aoe范围内的子弹不会触发这个，但是在onCreate里面会已经存在
        ///</summary>
        public string OnBulletEnter;

        public object[] OnBulletEnterParams;

        ///<summary>
        ///有子弹离开aoe时的事件
        ///</summary>
        public string OnBulletLeave;

        public object[] OnBulletLeaveParams;
    }
}