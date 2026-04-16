using System;

namespace SimpleFramework.StateMachine.FSM
{
    /// <summary>
    /// 创建特性，用于标记状态对应的枚举值
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class StateAttribute : Attribute
    {
        public int StateEnum { get; private set; }

        public StateAttribute(int stateEnum)
        {
            StateEnum = stateEnum;
        }
    }
}