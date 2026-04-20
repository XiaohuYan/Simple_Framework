using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.EGameState.Start)]
    public class GameState_Start : State<GameManager.EGameState>
    {
        public GameState_Start(FSMStateMachine<GameManager.EGameState> machine) : base(machine) { }

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