using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.EGameState.Pause)]
    public class GameState_Pause : State<GameManager.EGameState>
    {
        public GameState_Pause(FSMStateMachine<GameManager.EGameState> machine) : base(machine) { }

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