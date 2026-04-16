namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// AI 行为策略接口（策略模式）
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>执行状态</returns>
        Node.Status Process();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}