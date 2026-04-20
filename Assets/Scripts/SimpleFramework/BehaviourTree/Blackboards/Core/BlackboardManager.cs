using SimpleFramework.Common;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    public class BlackboardManager : IManager
    {
        BlackboardData blackboardData;
        readonly Blackboard blackboard = new Blackboard();
        readonly Arbiter arbiter = new Arbiter();


        public void OnManagerInit()
        {
            blackboardData.SetValuesOnBlackboard(blackboard);
        }

        public void AfterManagerInit()
        {

        }

        public void OnManagerDestroy()
        {
            
        }

        public Blackboard GetBlackboard()
        {
            return blackboard;
        }

        public void RegisterExpert(IExpert expert)
        {
            arbiter.RegisterExpert(expert);
        }

        void Update()
        {
            foreach(var action in arbiter.BlackboardIteration(blackboard))
            {
                action();
            }
        }

    }
}