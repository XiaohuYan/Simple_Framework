using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SimpleFramework.StateMachine.FSM
{
    public class FSMStateMachine<TState> where TState : Enum
    {
        // 所有状态
        protected Dictionary<TState, State<TState>> states = new Dictionary<TState, State<TState>>();

        /// <summary>
        /// 当前状态
        /// </summary>
        private TState currentState;

        public FSMStateMachine()
        {
            InitialStateMachine();
            currentState = default;
        }

        public void Update(float deltaTime)
        {
            if (states.TryGetValue(currentState, out var state))
            {
                state.Update(deltaTime);
            }
        }

        public void InitialStateMachine()
        {
            var stateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && !t.IsInterface && typeof(State<TState>).IsAssignableFrom(t));

            foreach (var type in stateTypes)
            {
                var attribute = type.GetCustomAttribute<StateAttribute>();

                if (attribute == null)
                {
                    Debug.LogWarning($"[StateMachine] 状态类 {type.Name} 没有标记 StateAttribute，跳过注册");
                    continue;
                }

                if (attribute.StateEnum is TState stateEnum)
                {
                    if (Activator.CreateInstance(type, this) is State<TState> stateInstance)
                    {
                        RegisterState(stateEnum, stateInstance);
                    }
                    else
                    {
                        Debug.LogWarning($"[StateMachine] 状态类 {type.Name}注册{stateEnum}失败");
                    }
                }
                else
                {
                    Debug.LogWarning($"[StateMachine] 状态类 {type.Name} 的 StateAttribute 类型不匹配，期望 {typeof(TState)}，实际 {attribute.StateEnum.GetType()}");
                }
            }
        }

        /// <summary>
        ///  注册状态机
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stateInstance"></param>
        public void RegisterState(TState state, State<TState> stateInstance)
        {
            if (states.ContainsKey(state))
            {
                Debug.LogWarning($"[StateMachine] 状态 {state} 已存在，将被覆盖");
            }
            states[state] = stateInstance;
        }

        /// <summary>
        /// 更换状态
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="OnChangState"></param>
        public void ChangeState(TState newState, Action<TState> OnChangState)
        {
            if (!newState.Equals(currentState))
            {
                if (states.TryGetValue(currentState, out var old_state) && states.TryGetValue(newState, out var new_state))
                {
                    old_state.Exit();
                    new_state.Enter();
                    OnChangState(newState);
                }
            }
        }
    }
}