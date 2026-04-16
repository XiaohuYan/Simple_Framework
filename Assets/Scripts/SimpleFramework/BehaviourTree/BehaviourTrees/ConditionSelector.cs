using System;
using System.Collections.Generic;

namespace SimpleFramework.BehaviourTree.Node
{
    /// <summary>
    /// 条件排序的选择器
    /// </summary>
    public abstract class ConditionSelector : Selector
    {
        /// <summary>
        /// 是否已经排序过
        /// </summary>
        private bool needSort = true;

        protected ConditionSelector(string name, int priority = 0) : base(name, priority) { }

        /// <summary>
        /// 排序方法
        /// </summary>
        protected abstract Action<List<Node>> SortCondition {  get; }

        public override void Reset()
        {
            base.Reset();
            needSort = true;
        }

        public override Status Process()
        {
            if (SortCondition != null && needSort)
            {
                SortCondition(children);
                needSort = false;
            }
            return base.Process();
        }
    }
}