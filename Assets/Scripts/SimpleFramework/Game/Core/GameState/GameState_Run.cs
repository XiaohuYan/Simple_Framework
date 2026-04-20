using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{

    [State((int)GameManager.GameStateEnum.Run)]
    public class GameState_Run : State<GameManager.GameStateEnum>
    {
        public GameState_Run(FSMStateMachine<GameManager.GameStateEnum> machine) : base(machine) { }

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