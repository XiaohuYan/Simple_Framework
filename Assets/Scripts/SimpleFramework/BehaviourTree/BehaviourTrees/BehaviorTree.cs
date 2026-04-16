namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 行为树进入节点
    /// </summary>
    public class BehaviourTree : Node
    {
        /// <summary>
        /// 用于确定什么时候需要返回值（策略模式）
        /// </summary>
        private readonly IPolicy policy;

        public BehaviourTree(string name, IPolicy policy = null) : base(name) { }

        /// <summary>
        /// 开始处理行为（update里调用）
        /// </summary>
        /// <returns>状态</returns>
        //public override Status Process()
        //{
        //    while (currentChild < children.Count)
        //    {
        //        var status = children[currentChild].Process();
        //        if (status != Status.Success)
        //        {
        //            return status;
        //        }
        //        currentChild++;
        //    }
        //    return Status.Success;
        //}

        /// <summary>
        /// 开始处理行为（update里调用）
        /// </summary>
        /// <returns>状态</returns>
        public override Status Process()
        {
            Status status = children[currentChild].Process();
            if (policy.ShouldReturn(status))
            {
                return status;
            }

            currentChild = (currentChild + 1) % children.Count;
            return Status.Running;
        }
    }
}