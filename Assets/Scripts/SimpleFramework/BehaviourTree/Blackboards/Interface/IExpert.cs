namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    public interface IExpert
    {
        /// <summary>
        /// 引起仲裁者关注的重要程度
        /// </summary>
        /// <param name="blackboard">黑板</param>
        /// <returns>引起仲裁者关注的重要程度大小</returns>
        int GetInsistence(Blackboard blackboard);

        /// <summary>
        /// 执行行为
        /// </summary>
        /// <param name="blackboard">黑板</param>
        void Execute(Blackboard blackboard);
    }

}