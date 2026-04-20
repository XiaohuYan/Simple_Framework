using SimpleFramework.StateMachine.FSM;

namespace SimpleFramework.Game
{
    [State((int)GameManager.GameStateEnum.End)]
    public class GameState_End : State<GameManager.GameStateEnum>
    {
        public GameState_End(FSMStateMachine<GameManager.GameStateEnum> machine) : base(machine) { }

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