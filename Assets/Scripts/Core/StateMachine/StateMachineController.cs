using System;
using System.Collections.Generic;
using Core.QFrameWork;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// 首先在需要状态机的对象下创建状态机实例
    /// 定义该对象特有的各种状态（用户控制）
    /// 将使用到的状态设置为状态机的子节点
    /// </summary>
    public partial class StateMachineController : BaseController
    {
        /// <summary>
        /// 子状态字典
        /// </summary>
        private readonly Dictionary<string, IState> mChildrenState = new(StringComparer.OrdinalIgnoreCase);
    
        /// <summary>
        /// 当前状态
        /// </summary>
        private IState mCurState;

        /// <summary>
        /// 设置默认执行的状态_childrenState
        /// </summary>
        public void SetDefaultState(string stateName)
        {
            if (mChildrenState.TryGetValue(stateName, out var value))
            {
                mCurState = value;
                mCurState?.EnterState();
            }
        }

        public void AddState(string stateName, IState state)
        {
            if (!mChildrenState.TryAdd(stateName, state))
            {
                return;
            }

            state.Owner = transform;
            state.OnChangedStateEvent += OnChangedState;
        }

        /// <summary>
        /// 状态改变信号处理
        /// </summary>
        private void OnChangedState(IState lastState, string nextStateName)
        {
            if (lastState != mCurState)
            {
                return;
            }

            if (mChildrenState.TryGetValue(nextStateName, out var nextState) == false) return;
        
            mCurState?.ExitState();
            nextState?.EnterState();
            mCurState = nextState;
        }

        private void Update()
        {
            mCurState?.Update();
        }

        private void FixedUpdate()
        {
            mCurState?.FixUpdate();
        }
    }
}