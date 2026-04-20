using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.GameStateEnum.Start)]
    public class GameState_Start : State<GameManager.GameStateEnum>
    {
        public GameState_Start(FSMStateMachine<GameManager.GameStateEnum> machine) : base(machine) { }

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