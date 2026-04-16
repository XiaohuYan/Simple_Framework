namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 选择器节点
    /// </summary>
    public class Selector : Node
    {
        public Selector(string name, int priority = 0) : base(name, priority) { }

        /// <summary>
        /// 从第一个节点开始操作直到找到一个成功的节点
        /// </summary>
        /// <returns>节点状态</returns>
        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        currentChild++;
                        return Status.Running;

                }
            }
            Reset();
            return Status.Failure;
        }
    }

}