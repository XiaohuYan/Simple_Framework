using System.Collections.Generic;

namespace SimpleFramework.StateMachine.HSM
{
    public class StateMachine
    {
        /// <summary>
        /// 状态根节点
        /// </summary>
        public readonly State Root;

        /// <summary>
        /// 过渡序列器
        /// </summary>
        public readonly TransitionSequencer Sequencer;

        /// <summary>
        /// 是否启动
        /// </summary>
        bool started;

        public StateMachine(State root)
        {
            Root = root;
            Sequencer = new TransitionSequencer(this);
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        public void Start()
        {
            if (started)
            {
                return;
            }
            started = true;
            Root.Enter();
        }

        /// <summary>
        /// 每帧调用更新方法
        /// </summary>
        /// <param name="deltaTime">每帧时间</param>
        public void Tick(float deltaTime)
        {
            if (!started)
            {
                Start();
            }
            InternalTick(deltaTime);
        }

        /// <summary>
        /// 帧更新
        /// </summary>
        /// <param name="deltaTime">每帧时间</param>
        internal void InternalTick(float deltaTime)
        {
            Root.Update(deltaTime);
        }

        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null)
            {
                return;
            }

            // 找到第一个公共祖先
            State lca = TransitionSequencer.Lca(from, to);

            // 从低到高依次调用退出方法
            for (State s = from; s != lca; s = s.Parent)
            {
                // 疑问？每个节点exit会调用两次
                s.Exit();
            }

            // 从高到低依次调用进入
            var stack = new Stack<State>();
            for (State s = to; s != lca; s = s.Parent)
            {
                stack.Push(s);
            }
            while (stack.Count > 0)
            {
                // 疑问？如果活跃子节点不为null就会调用多次enter
                stack.Pop().Enter();
            }
        }

    }
}
