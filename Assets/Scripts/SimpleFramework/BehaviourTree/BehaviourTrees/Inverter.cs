namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 装饰节点，将子节点的执行结果取反。
    /// </summary>
    public class Inverter : Node
    {
        public Inverter(string name, int priority = 0) : base(name, priority) { }

        public override Status Process()
        {
            switch(children[0].Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    return Status.Success;
                default:
                    return Status.Failure;
            }
        }

    }

}