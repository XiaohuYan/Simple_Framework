using System;
using System.Collections.Generic;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    /// <summary>
    /// 仲裁者(仲裁者模式)
    /// </summary>
    public class Arbiter
    {
        /// <summary>
        /// 专家列表
        /// </summary>
        private readonly List<IExpert> experts = new List<IExpert>();

        /// <summary>
        /// 注册专家
        /// </summary>
        /// <param name="expert"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void RegisterExpert(IExpert expert)
        {
            if (expert == null)
            {
                throw new System.ArgumentNullException("专家为空");
            }
            experts.Add(expert);
        }

        /// <summary>
        /// 找到引起注意力最高的专家并让他写入行为
        /// </summary>
        /// <param name="blackboard">黑板</param>
        /// <returns>行为列表</returns>
        public List<Action> BlackboardIteration(Blackboard blackboard)
        {
            IExpert bestExpert = null;
            int hightestInsistence = 0;

            foreach(var expert in experts)
            {
                int insistence = expert.GetInsistence(blackboard);
                if(insistence > hightestInsistence)
                {
                    hightestInsistence = insistence;
                    bestExpert = expert;
                }
            }

            bestExpert?.Execute(blackboard);
            // 后续清空不受影响
            var actions = new List<Action>(blackboard.PassedActions);
            blackboard.ClearActions();

            return actions;
        }
    }

}