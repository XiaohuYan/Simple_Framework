namespace SimpleFramework.BehaviourTree.Node
{
    public interface IPolicy
    {
        /// <summary>
        /// 是否需要返回状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>是否需要返回</returns>
        bool ShouldReturn(Node.Status status);
    }
}