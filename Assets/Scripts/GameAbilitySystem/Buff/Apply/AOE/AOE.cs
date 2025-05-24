using System.Collections.Generic;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace GameAbilitySystem.Buff.Apply.AOE
{
    ///<summary>
    ///AoE发射器，创建aoe依赖的数据都在这里了
    ///</summary>
    public class AoeLauncher
    {
        ///<summary>
        ///要释放的aoe
        ///</summary>
        public AoeDataModel DataModel;

        ///<summary>
        ///释放的中心坐标
        ///</summary>
        public Vector3 Position;

        ///<summary>
        ///释放aoe的角色的GameObject，当然可能是null的
        ///</summary>
        public IGameAbilityUnit Caster;

        ///<summary>
        ///aoe的半径，单位：米
        ///目前这游戏的设计中，aoe只有圆形，所以只有一个半径，也不存在角度一说，如果需要可以扩展
        ///</summary>
        public float Radius;

        ///<summary>
        ///aoe存在的时间，单位：秒
        ///</summary>
        public float Duration;

        ///<summary>
        ///aoe的角度
        ///</summary>
        public float Degree;

        ///<summary>
        ///aoe移动轨迹函数
        ///</summary>
        public AoeTween Tween;

        public object[] TweenParam = new object[0];

        ///<summary>
        ///aoe的传入参数，比如可以吸收次数之类的
        ///</summary>
        public Dictionary<string, object> Param = new Dictionary<string, object>();

        public AoeLauncher(
            AoeDataModel dataModel, IGameAbilityUnit caster, Vector3 position, float radius, float duration, float degree,
            AoeTween tween = null, object[] tweenParam = null, Dictionary<string, object> aoeParam = null
        )
        {
            this.DataModel = dataModel;
            this.Caster = caster;
            this.Position = position;
            this.Radius = radius;
            this.Duration = duration;
            this.Degree = degree;
            this.Tween = tween;
            if (aoeParam != null) this.Param = aoeParam;
            if (tweenParam != null) this.TweenParam = tweenParam;
        }

        public AoeLauncher Clone()
        {
            return new AoeLauncher(
                this.DataModel,
                this.Caster,
                this.Position,
                this.Radius,
                this.Duration,
                this.Degree,
                this.Tween,
                this.TweenParam,
                this.Param
            );
        }
    }



    ///<summary>
    ///aoe的移动信息
    ///</summary>
    public class AoeMoveInfo
    {
        ///<summary>
        ///此时此刻的移动方式
        ///</summary>
        public MoveType moveType;

        ///<summary>
        ///此时aoe移动的力量，在这个游戏里，y坐标依然无效，如果要做手雷一跳一跳的，请使用其他的component绑定到特效的gameobject上，而非aoe的
        ///</summary>
        public Vector3 velocity;

        ///<summary>
        ///aoe的角度变成这个值
        ///</summary>
        public float rotateToDegree;

        public AoeMoveInfo(MoveType moveType, Vector3 velocity, float rotateToDegree)
        {
            this.moveType = moveType;
            this.velocity = velocity;
            this.rotateToDegree = rotateToDegree;
        }
    }

    ///<summary>
    ///aoe创建时的事件
    ///<param name="aoe">被创建出来的aoe的gameObject</param>
    ///</summary>
    public delegate void AoeOnCreate(GameObject aoe);

    ///<summary>
    ///aoe移除时候的事件
    ///<param name="aoe">被创建出来的aoe的gameObject</param>
    ///</summary>
    public delegate void AoeOnRemoved(GameObject aoe);

    ///<summary>
    ///aoe每一跳的事件
    ///<param name="aoe">被创建出来的aoe的gameObject</param>
    ///</summary>
    public delegate void AoeOnTick(GameObject aoe);

    ///<summary>
    ///当有角色进入aoe范围的时候触发
    ///<param name="aoe">被创建出来的aoe的gameObject</param>
    ///<param name="cha">进入aoe范围的那些角色，他们现在还不在aoeState的角色列表里</param>
    ///</summary>
    public delegate void AoeOnCharacterEnter(GameObject aoe, List<GameObject> cha);

    ///<summary>
    ///当有角色离开aoe范围的时候
    ///<param name="aoe">离开aoe的gameObject</param>
    ///<param name="cha">离开aoe范围的那些角色，他们现在已经不在aoeState的角色列表里</param>
    ///</summary>
    public delegate void AoeOnCharacterLeave(GameObject aoe, List<GameObject> cha);

    ///<summary>
    ///当有子弹进入aoe范围的时候
    ///<param name="aoe">被创建出来的aoe的gameObject</param>
    ///<param name="bullet">离开aoe范围的那些子弹，他们现在已经不在aoeState的子弹列表里</param>
    ///</summary>
    public delegate void AoeOnBulletEnter(GameObject aoe, List<GameObject> bullet);

    ///<summary>
    ///当有子弹离开aoe范围的时候
    ///<param name="aoe">离开的aoe的gameObject</param>
    ///<param name="bullet">离开aoe范围的那些子弹，他们现在已经不在aoeState的子弹列表里</param>
    ///</summary>
    public delegate void AoeOnBulletLeave(GameObject aoe, List<GameObject> bullet);

    ///<summary>
    ///aoe的移动轨迹函数
    ///<param name="aoe">要执行的aoeObj</param>
    ///<param name="t">这个tween在aoe中运行了多久了，单位：秒</param>
    ///<return>aoe在这时候的移动信息</param>
    public delegate AoeMoveInfo AoeTween(GameObject aoe, float t);
}