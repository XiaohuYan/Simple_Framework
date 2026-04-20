using System.Collections.Generic;

namespace SimpleFramework.BehaviourTree.Node
{
    public abstract class Node
    {
        /// <summary>
        /// 节点状态
        /// </summary>
        public enum Status { Success, Failure, Running }

        /// <summary>
        /// 节点名
        /// </summary>
        public readonly string name;

        /// <summary>
        /// 所有子节点
        /// </summary>
        public readonly List<Node> children = new List<Node>();

        /// <summary>
        /// 当前正在运行子节点的索引
        /// </summary>
        protected int currentChild;

        /// <summary>
        /// 优先级
        /// </summary>
        public readonly int priority;

        public Node(string name, int priority = 0)
        {
            this.name = name;
            this.priority = priority;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child">子节点</param>
        /// <returns>返回当前节点方便链式添加子节点</returns>
        public Node AddChild(Node child)
        {
            children.Add(child);
            return this;
        }

        /// <summary>
        /// 运行当前需要运行的子节点并返回状态
        /// </summary>
        /// <returns>当前运行子节点运行状态</returns>
        public virtual Status Process()
        {
            return children[currentChild].Process();
        }

        /// <summary>
        /// 重置节点
        /// </summary>
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }
}