namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 顺序执行节点
    /// </summary>
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority) { }

        /// <summary>
        /// 按顺序执行所有节点
        /// </summary>
        /// <returns>节点状态</returns>
        //public override Status Process()
        //{
        //    if (currentChild < children.Count)
        //    {
        //        switch (children[currentChild].Process())
        //        {
        //            case Status.Running:
        //                return Status.Running;
        //            case Status.Failure:
        //                Reset();
        //                return Status.Failure;
        //            default:
        //                currentChild++;
        //                return currentChild == children.Count ? Status.Running : Status.Failure;
        //        }
        //    }

        //    Reset();
        //    return Status.Running;
        //}

        /// <summary>
        /// 按顺序执行所有节点
        /// </summary>
        /// <returns>节点状态</returns>
        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();  // 失败时重置
                        return Status.Failure;
                }

                // Success 移动到下一个子节点
                currentChild++;
            }
            // 所有子节点都成功了
            Reset();
            return Status.Success;
        }
    }
}