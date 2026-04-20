using System.Collections.Generic;

namespace SimpleFramework.StateMachine.HSM
{
    /// <summary>
    /// 状态类
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// 状态机
        /// </summary>
        public readonly StateMachine Machine;

        /// <summary>
        /// 父状态
        /// </summary>
        public readonly State Parent;

        /// <summary>
        /// 活跃子状态
        /// </summary>
        public State ActiveChild { get; private set; }

        public State(StateMachine machine, State parent = null)
        {
            Machine = machine;
            Parent = parent;
        }

        /// <summary>
        /// 获取进入该状态时的默认活跃子状态
        /// </summary>
        /// <returns>默认活跃子状态，null代表当前为叶子节点</returns>
        protected virtual State GetInitialState()
        {
            return null;
        }

        /// <summary>
        /// 当前帧需要切换到的状态
        /// </summary>
        /// <returns>切换后的状态，null表示停留在此状态</returns>
        protected virtual State GetTransition()
        {
            return null;
        }

        #region 生命周期
        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnUpdate(float deltaTime);
        #endregion

        /// <summary>
        /// 进入
        /// </summary>
        public void Enter()
        {
            // 类先序遍历进入
            if (Parent != null)
            {
                Parent.ActiveChild = this;
            }
            OnEnter();
            State init = GetInitialState();
            if (init != null)
            {
                init.Enter();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Exit()
        {
            // 深度优先退出
            if (ActiveChild != null)
            {
                ActiveChild.Exit();
            }
            ActiveChild = null;
            OnExit();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime">每帧消耗时间</param>
        public void Update(float deltaTime)
        {
            State to = GetTransition();
            if (to != null)
            {
                // 如果需要转移则转移状态
                Machine.Sequencer.RequestTransition(this, to);
                return;
            }

            // 深度优先更新
            if (ActiveChild != null)
            {
                ActiveChild.Update(deltaTime);
            }
            OnUpdate(deltaTime);
        }

        /// <summary>
        /// 获取到状态叶子节点
        /// </summary>
        /// <returns>状态叶子节点</returns>
        public State Leaf()
        {
            State state = this;
            while (ActiveChild != null)
            {
                state = state.ActiveChild;
            }
            return state;
        }

        /// <summary>
        /// 从当前节点迭代返回父节点
        /// </summary>
        /// <returns>迭代父节点</returns>
        public IEnumerable<State> PathRoot()
        {
            for (State s = this; s != null; s = s.Parent)
            {
                yield return s;
            }
        }
    }
}