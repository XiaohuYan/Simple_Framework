using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleFramework.BehaviourTree.Node
{
    //public class RandomSelector : PrioritySelector
    //{
    //    protected override List<Node> SortedChildren() => children.Shuffle().ToList();

    //    public RandomSelector(string name, int priority = 0) : base(name, priority) { }

    //}

    /// <summary>
    /// Ëæ»úÅÅÐòÑ¡ÔñÆ÷
    /// </summary>
    public class RandomSelector : ConditionSelector
    {
        public RandomSelector(string name, int priority = 0) : base(name, priority) { }

        protected override Action<List<Node>> SortCondition
        {
            get => (children) =>
            {
                children.Shuffle().ToList();
            };
        }
    }

}