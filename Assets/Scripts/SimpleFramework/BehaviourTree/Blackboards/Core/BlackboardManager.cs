using SimpleFramework.ActionMono;
using SimpleFramework.Common;
using SimpleFramework.Entry;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    public class BlackboardManager : IManager
    {
        private int priority = 0;

        public int Priority => priority;

        private BlackboardData blackboardData;
        private readonly Blackboard blackboard = new Blackboard();
        private readonly Arbiter arbiter = new Arbiter();

        public Blackboard GetBlackboard()
        {
            return blackboard;
        }

        public void RegisterExpert(IExpert expert)
        {
            arbiter.RegisterExpert(expert);
        }

        private void Update(float deltaTime)
        {
            foreach (var action in arbiter.BlackboardIteration(blackboard))
            {
                action();
            }
        }


        public void OnManagerInit()
        {
            blackboardData.SetValuesOnBlackboard(blackboard);
        }

        public void AfterManagerInit()
        {
            GameFacade.Instance.GetManager<ActionMonoManager>().AddUpdate(Update);
        }

        public void OnManagerDestroy()
        {
            GameFacade.Instance.GetManager<ActionMonoManager>().RemoveUpdate(Update);
        }
    }
}