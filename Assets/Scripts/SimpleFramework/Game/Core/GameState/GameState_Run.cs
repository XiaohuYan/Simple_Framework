using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{

    [State((int)GameManager.EGameState.Run)]
    public class GameState_Run : State<GameManager.EGameState>
    {
        public GameState_Run(FSMStateMachine<GameManager.EGameState> machine) : base(machine) { }

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