using System.Collections.Generic;
using GameAbilitySystem.Buff.Unit;
using UnityEngine;

namespace GameAbilitySystem.Buff.Apply.Bullet
{
    ///<summary>
    ///子弹的发射信息，专门有个系统会处理这个发射信息，然后往地图上放置出子弹的GameObject
    ///所有脚本中，需要创建一个子弹，也应该传递这个结构作为产生子弹的参数
    ///</summary>
    public class BulletLauncher
    {
        ///<summary>
        ///要发射的子弹
        ///</summary>
        public BulletDataModel DataModel;

        ///<summary>
        ///要发射子弹的这个人的gameObject，这里就认角色（拥有ChaState的）
        ///当然可以是null发射的，但是写效果逻辑的时候得小心caster是null的情况
        ///</summary>
        public IGameAbilityUnit Caster;

        ///<summary>
        ///发射的坐标，y轴是无效的
        ///</summary>
        public Vector3 FirePosition;

        ///<summary>
        ///发射的角度，单位：角度
        ///</summary>
        public float FireDegree;

        ///<summary>
        ///子弹的初速度，单位：米/秒
        ///</summary>
        public float Speed;

        ///<summary>
        ///子弹的生命周期，单位：秒
        ///子弹应该是有个生命周期的，因为如果总是不命中，也不回收总不好
        ///当然更多的还是因为有些子弹射程非常短
        ///</summary>
        public float Duration;

        ///<summary>
        ///子弹在发射瞬间，可以捕捉一个GameObject作为目标，并且将这个目标传递给BulletTween，作为移动参数
        ///<param name="bullet">是当前的子弹GameObject，不建议公式中用到这个</param>
        ///<param name="targets">所有可以被选作目标的对象，这里是GameManager的逻辑决定的传递过来谁，比如这个游戏子弹只能捕捉角色作为对象，那就是只有角色的GameObject，当然如果需要，加入子弹也不麻烦</param>
        ///<return>在创建子弹的瞬间，根据这个函数获得一个GameObject作为followingTarget</return>
        ///</summary>
        public BulletTargettingFunction TargetFunc;

        ///<summary>
        ///子弹的轨迹函数，传入一个时间点，返回出一个Vector3，作为这个时间点的速度和方向，这是个相对于正在飞行的方向的一个偏移（*speed的）
        ///正在飞行的方向按照z轴，来算，也就是说，当你只需要子弹匀速行动的时候，你可以让这个函数只做一件事情——return Vector3.forward。
        ///如果这个值是null，就会跟return Vector3.forward一样处理，性能还高一些。
        ///虽然是vector3，但是y坐标是无效的，只是为了统一单位
        ///比如手榴弹这种会一跳一跳的可不得y变化吗？是要变化，但是这个变化归我管，这是render的事情
        ///简单地说就是做一个跳跳的Component，update（而非fixedupdate）里面去管理跳吧
        ///<param name="t">子弹飞行了多久的时间点，单位秒。</param>
        ///<return>返回这一时间点上的速度和偏移，Vector3就是正常速度正常前进</return>
        ///</summary>
        public BulletTween Tween = null;

        ///<summary>
        ///子弹的移动轨迹是否严格遵循发射出来的角度
        ///如果是true，则子弹每一帧Tween返回的角度是按照fireDegree来偏移的
        ///如果是false，则会根据子弹正在飞的角度(transform.rotation)来算下一帧的角度
        ///</summary>
        public bool UseFireDegreeForever = false;

        ///<summary>
        ///子弹创建后多久是没有碰撞的，这样比如子母弹之类的，不会在创建后立即命中目标，但绝大多子弹还应该是0的
        ///单位：秒
        ///</summary>
        public float CanHitAfterCreated = 0;

        ///<summary>
        ///子弹的一些特殊逻辑使用的参数，可以在创建子的时候传递给子弹
        ///</summary>
        public Dictionary<string, object> Param;

        public BulletLauncher(
            BulletDataModel dataModel, IGameAbilityUnit caster, Vector3 firePos, float degree, float speed, float duration,
            float canHitAfterCreated = 0,
            BulletTween tween = null, BulletTargettingFunction targetFunction = null, bool useFireDegree = false,
            Dictionary<string, object> param = null
        )
        {
            this.DataModel = dataModel;
            this.Caster = caster;
            this.FirePosition = firePos;
            this.FireDegree = degree;
            this.Speed = speed;
            this.Duration = duration;
            this.Tween = tween;
            this.UseFireDegreeForever = useFireDegree;
            this.TargetFunc = targetFunction;
            this.Param = param;
        }
    }



    ///<summary>
    ///子弹命中纪录
    ///</summary>
    public class BulletHitRecord
    {
        ///<summary>
        ///角色的GameObject
        ///</summary>
        public IGameAbilityUnit target;

        ///<summary>
        ///多久之后还能再次命中，单位秒
        ///</summary>
        public float timeToCanHit;

        public BulletHitRecord(IGameAbilityUnit character, float timeToCanHit)
        {
            this.target = character;
            this.timeToCanHit = timeToCanHit;
        }
    }

    ///<summary>
    ///子弹被创建的事件
    ///</summary>
    public delegate void BulletOnCreate(IGameAbilityUnit bullet);

    ///<summary>
    ///子弹命中目标的时候触发的事件
    ///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
    ///<param name="target">被击中的角色</param>
    ///<summary>
    public delegate void BulletOnHit(IGameAbilityUnit bullet, IGameAbilityUnit target);

    ///<summary>
    ///子弹在生命周期消耗殆尽之后发生的事件，生命周期消耗殆尽是因为BulletState.duration<=0，或者是因为移动撞到了阻挡。
    ///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
    ///</summary>
    public delegate void BulletOnRemoved(IGameAbilityUnit bullet);

    ///<summary>
    ///子弹的轨迹函数，传入一个时间点，返回出一个Vector3，作为这个时间点的速度和方向，这是个相对于正在飞行的方向的一个偏移（*speed的）
    ///正在飞行的方向按照z轴，来算，也就是说，当你只需要子弹匀速行动的时候，你可以让这个函数只做一件事情——return Vector3.forward。
    ///<param name="t">子弹飞行了多久的时间点，单位秒。</param>
    ///<param name="bullet">是当前的子弹GameObject，不建议公式中用到这个</param>
    ///<param name="following">是正在跟踪的对象的GameObject，除非要做“跟踪弹”不然不建议使用</param>
    ///<return>返回这一时间点上的速度和偏移，Vector3就是正常速度正常前进</return>
    ///</summary>
    public delegate Vector3 BulletTween(float t, IGameAbilityUnit bullet, IGameAbilityUnit target);

    ///<summary>
    ///子弹在发射瞬间，可以捕捉一个GameObject作为目标，并且将这个目标传递给BulletTween，作为移动参数
    ///<param name="bullet">是当前的子弹GameObject，不建议公式中用到这个</param>
    ///<param name="targets">所有可以被选作目标的对象，这里是GameManager的逻辑决定的传递过来谁，比如这个游戏子弹只能捕捉角色作为对象，那就是只有角色的GameObject，当然如果需要，加入子弹也不麻烦</param>
    ///<return>在创建子弹的瞬间，根据这个函数获得一个GameObject作为followingTarget</return>
    ///</summary>
    public delegate Node BulletTargettingFunction(IGameAbilityUnit bullet, IGameAbilityUnit[] targets);
}