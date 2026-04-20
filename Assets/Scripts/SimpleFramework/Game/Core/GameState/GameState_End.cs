using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.EGameState.End)]
    public class GameState_End : State<GameManager.EGameState>
    {
        public GameState_End(FSMStateMachine<GameManager.EGameState> machine) : base(machine) { }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Update(float deltaTime)
        {

        }
    }
}