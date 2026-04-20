using SimpleFramework.StateMachine.FSM;
namespace SimpleFramework.Game
{
    public partial class GameManager : IGameManager
    {
        /// <summary>
        /// 游戏状态
        /// </summary>
        private EGameState gameState;

        /// <summary>
        /// 游戏状态状态机
        /// </summary>
        private FSMStateMachine<EGameState> fSMStateMachine;

        /// <summary>
        /// 获取到游戏状态
        /// </summary>
        /// <returns>游戏状态</returns>
        public EGameState GetGameState()
        {
            return gameState;
        }


        public void OnManagerInit()
        {
            gameState = default;
        }


        public void AfterManagerInit()
        {
            fSMStateMachine = new FSMStateMachine<EGameState>();
        }

        public void OnManagerDestroy()
        {
            gameState = default;
        }
    }
}