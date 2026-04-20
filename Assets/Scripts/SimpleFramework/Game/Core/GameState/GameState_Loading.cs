using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.EGameState.Loading)]
    public class GameState_Loading : State<GameManager.EGameState>
    {
        public GameState_Loading(FSMStateMachine<GameManager.EGameState> machine) : base(machine) { }

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