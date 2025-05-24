using GameAbilitySystem.Buff.Unit;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAbilitySystem.Buff.Apply.Bullet
{
    ///<summary>
    ///子弹的模板，也是策划填表的东西，当然游戏过程中所有的子弹模板，未必都得由策划填表，也可以运行的脚本逻辑产生
    ///值得注意的是，这些信息只是构成“一个子弹”，也就是描述了这个子弹是怎样的，因此有很多数据并不属于这个结构
    ///比如子弹的飞行速度、轨迹等，这些数据其实都是子弹的发射环境决定的，同一个导弹，可能被不同的人、地形、其他任何东西发射出来
    ///这些子弹的性质是一样的，就是被填表的这些内容，但是他们可能轨迹之类都不同。
    ///</summary>
    
    [CreateAssetMenu(menuName = "DataModel/Buff/BulletDataModel")]
    public class BulletDataModel : ScriptableObject
    {
        public string Id;

        ///<summary>
        ///子弹需要用的prefab，默认是Resources/Prefabs/Bullet/下的，所以这个string需要省略前半部分
        ///比如是BlueRocket0，就会创建自Resources/Prefabs/Bullet/BlueRocket0这个prefab
        ///</summary>
        public Sprite sprite;

        ///<summary>
        ///子弹的碰撞半径，单位：米。这个游戏里子弹在逻辑世界都是圆形的，当然是这个游戏设定如此，实际策划的需求未必只能是圆形。
        ///</summary>
        public float Radius;

        public float Speed;

        public float Duration = 5.0f;
        
        ///<summary>
        ///子弹可以碰触的次数，每次碰到合理目标-1，到0的时候子弹就结束了。
        ///</summary>
        //public int HitTimes;

        ///<summary>
        ///子弹碰触同一个目标的延迟，单位：秒，最小值是Time.fixedDeltaTime（每帧发生一次）
        ///</summary>
        //public float SameTargetDelay;

        ///<summary>
        ///子弹被创建的事件
        ///<param name="bullet">要创建的子弹</param>
        ///</summary>
        public string OnCreate;

        //public object[] OnCreateParam;

        ///<summary>
        ///子弹命中目标时候发生的事情
        ///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
        ///<param name="target">被击中的角色</param>
        ///</summary>
        public string OnHit;

        ///<summary>
        ///OnHit的参数
        ///</summary>
        //public object[] OnHitParams;

        ///<summary>
        ///子弹生命周期结束时候发生的事情
        ///<param name="bullet">发生碰撞的子弹，应该是个bulletObj，但是在unity的逻辑下，他就是个GameObject，具体数据从GameObject拿了</param>
        ///</summary>
        public string OnRemoved;

        ///<summary>
        ///OnRemoved的参数
        ///</summary>
        //public object[] OnRemovedParams;

        ///<summary>
        ///子弹的移动方式，一般来说都是飞行的，也有部分是手雷这种属于走路的
        ///</summary>
        //public MoveType MoveType;

        ///<summary>
        ///子弹的是否碰到障碍物就爆炸，不会的话会沿着障碍物移动
        ///</summary>
        //public bool RemoveOnObstacle;

        ///<summary>
        ///子弹是否会命中敌人
        ///</summary>
        //public bool HitFoe;

        ///<summary>
        ///子弹是否会命中盟军
        ///</summary>
        // public bool HitAlly;
    }
}