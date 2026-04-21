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
        private readonly FSMStateMachine<EGameState> fSMStateMachine = new FSMStateMachine<EGameState>();

        /// <summary>
        /// 获取到游戏状态
        /// </summary>
        /// <returns>游戏状态</returns>
        public EGameState GetGameState()
        {
            return gameState;
        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <param name="gameState"></param>
        private void ChangeGameState(EGameState gameState)
        {
            fSMStateMachine.ChangeState(gameState,OnChangState);
        }

        /// <summary>
        /// 游戏状态切换后的回调
        /// </summary>
        /// <param name="gameState"></param>
        private void OnChangState(EGameState gameState)
        {

        }

        public void OnManagerInit()
        {
            gameState = default;
        }


        public void AfterManagerInit()
        {
            
        }

        public void OnManagerDestroy()
        {
            gameState = default;
        }
    }
}