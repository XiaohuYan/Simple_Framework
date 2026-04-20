using System.Collections.Generic;

namespace SimpleFramework.StateMachine.HSM
{
    public class TransitionSequencer
    {
        /// <summary>
        /// 状态机
        /// </summary>
        public readonly StateMachine Machine;

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }

        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">转换后的状态</param>
        public void RequestTransition(State from, State to)
        {
            Machine.ChangeState(from, to);
        }

        /// <summary>
        /// 找到公共祖先
        /// </summary>
        /// <param name="a">状态a</param>
        /// <param name="b">状态b</param>
        /// <returns>a 和 b 第一个公共祖先</returns>
        public static State Lca(State a, State b)
        {
            var ap = new HashSet<State>();
            for (var s = a; s != null; s = s.Parent)
            {
                ap.Add(s);
            }
            for (var s = b; s != null; s = s.Parent)
            {
                if (ap.Contains(s))
                {
                    return s;
                }
            }
            return null;
        }
    }
}
