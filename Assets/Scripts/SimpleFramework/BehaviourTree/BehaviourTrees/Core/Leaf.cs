namespace SimpleFramework.BehaviourTree.Node
{
    public class Leaf : Node
    {
        /// <summary>
        /// AI 行为策略
        /// </summary>
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
        {
            if (strategy == null)
            {
                throw new System.ArgumentNullException($"节点{name}的策略为空");
            }
            this.strategy = strategy;
        }

        public override Status Process()
        {
            return strategy.Process();
        }

        public override void Reset()
        {
            strategy.Reset();
        }
    }
}