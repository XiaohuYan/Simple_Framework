namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 重复执行子节点，直到子节点返回“失败(Failure)”才停止
    /// </summary>
    public class UntilFail : Node
    {
        public UntilFail(string name, int priority = 0) : base(name, priority) { }

        public override Status Process()
        {
            if (children[0].Process() == Status.Failure)
            {
                Reset();
                return Status.Failure;
            }
            return Status.Running;
        }

    }
}