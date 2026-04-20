using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleFramework.BehaviourTree.Node
{
    //public class PrioritySelector : Node
    //{
    //    List<Node> sortedChildren;

    //    public PrioritySelector(string name, int priority = 0) : base(name, priority) { }

    //    protected virtual List<Node> SortedChildren()
    //    {
    //        if (sortedChildren != null)
    //        {
    //            return sortedChildren;
    //        }
    //        return children.OrderByDescending((child) =>
    //        {
    //            return child.priority;
    //        }).ToList();
    //    }

    //    public override void Reset()
    //    {
    //        base.Reset();
    //        sortedChildren = null;
    //    }

    //    public override Status Process()
    //    {
    //        foreach (var child in SortedChildren())
    //        {
    //            switch (child.Process())
    //            {
    //                case Status.Success:
    //                    Reset();
    //                    return Status.Success;
    //                case Status.Running:
    //                    return Status.Running;
    //                default:
    //                    continue;
    //            }
    //        }
    //        Reset();
    //        return Status.Failure;
    //    }
    //}

    /// <summary>
    /// 优先级排序选择器
    /// </summary>
    public class PrioritySelector : ConditionSelector
    {
        public PrioritySelector(string name, int priority = 0) : base(name, priority) { }

        protected override Action<List<Node>> SortCondition
        {
            get => (children) =>
            {
                children.Sort((x, y) => y.priority.CompareTo(x.priority)); // 降序：优先级大的在前
            };
        }

    }
}