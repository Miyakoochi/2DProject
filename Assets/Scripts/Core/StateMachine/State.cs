
using System;
using Core.QFrameWork;
using QFramework;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// 状态机状态接口
    /// 用户继承该接口自定义状态
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 对应控制的对象
        /// </summary>
        public Transform Owner { get; set; }

        /// <summary>
        /// 进入状态
        /// </summary>
        public void EnterState();

        /// <summary>
        /// 退出状态
        /// </summary>
        public void ExitState();

        public void Update();

        public void FixUpdate();

        public Action<IState, string> OnChangedStateEvent { get; set; }
    }
    
    public abstract class StateObject : IState
    {
        public Transform Owner { get; set; }
        public void EnterState()
        {
            OnEnterState();
        }

        protected virtual void OnEnterState(){}

        public void ExitState()
        {
            OnExitState();
        }
        protected virtual void OnExitState(){}

        public void Update()
        {
            OnUpdate();
        }
        
        protected virtual void OnUpdate(){}

        public void FixUpdate()
        {
            OnFixUpdate();
        }

        public Action<IState, string> OnChangedStateEvent { get; set; }

        protected virtual void OnFixUpdate(){}

        protected void SendChangeEvent(string nextStateName)
        {
            OnChangedStateEvent?.Invoke(this, nextStateName);
        }
    }

    public class StateController : StateObject, IController
    {
        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }
    }
}
