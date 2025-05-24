using System.Numerics;

namespace GameAbilitySystem.Buff.Unit
{
    ///<summary>
    ///预约了多少时间内【匀速直线】移动往某个方向多远
    ///</summary>
    public class MovePreorder
    {
        ///<summary>
        ///想要移动的方向和距离
        ///</summary>
        public Vector3 Velocity;

        ///<summary>
        ///多久完成，单位秒
        ///</summary>
        private float _inTime;

        ///<summary>
        ///还有多久移动完成，单位：秒，如果小于1帧的时间但还大于0，就会当做1帧来执行
        ///</summary>
        public float Duration;
        public MovePreorder(Vector3 velocity, float duration)
        {
            this.Velocity = velocity;
            this.Duration = duration;
            this._inTime = duration;
        }

        ///<summary>
        ///运行了一段时间，返回这段时间内的移动力
        ///<param name="time">运行的时间，单位：秒</param>
        ///<return>移动力</return>
        public Vector3 VeloInTime(float time)
        {
            if (time >= Duration)
            {
                this.Duration = 0;
            }
            else
            {
                this.Duration -= time;
            }
            return _inTime <= 0 ? Velocity : (Velocity / _inTime);
        }
    }

    public enum MoveType
    {
        ground = 0,
        fly = 1
    }
}