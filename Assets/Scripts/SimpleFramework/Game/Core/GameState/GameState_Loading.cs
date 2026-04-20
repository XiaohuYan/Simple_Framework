using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.GameStateEnum.Loading)]
    public class GameState_Loading : State<GameManager.GameStateEnum>
    {
        public GameState_Loading(FSMStateMachine<GameManager.GameStateEnum> machine) : base(machine) { }

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