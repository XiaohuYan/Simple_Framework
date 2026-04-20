using System;

namespace SimpleFramework.StateMachine.FSM
{
    /// <summary>
    ///  ×´Ì¬»ú×´Ì¬
    /// </summary>
    public abstract class State<TState> where TState : Enum
    {
        /// <summary>
        /// ×´Ì¬»ú
        /// </summary>
        protected FSMStateMachine<TState> machine;

        public State(FSMStateMachine<TState> machine)
        {
            this.machine = machine;
        }

        public abstract void Enter();
        public abstract void Update(float deltaTime);
        public abstract void Exit();
    }

}