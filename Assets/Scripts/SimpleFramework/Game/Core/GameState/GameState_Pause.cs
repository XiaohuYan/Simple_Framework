using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.GameStateEnum.Pause)]
    public class GameState_Pause : State<GameManager.GameStateEnum>
    {
        public GameState_Pause(FSMStateMachine<GameManager.GameStateEnum> machine) : base(machine) { }

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