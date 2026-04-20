
namespace SimpleFramework.Game
{
    public partial class GameManager : IGameManager
    {
        /// <summary>
        /// 游戏状态
        /// </summary>
        private GameStateEnum gameState;

        /// <summary>
        /// 获取到游戏状态
        /// </summary>
        /// <returns>游戏状态</returns>
        public GameStateEnum GetGameState()
        {
            return gameState;
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