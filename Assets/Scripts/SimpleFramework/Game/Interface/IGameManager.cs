using SimpleFramework.Common;
using  SimpleFramework.Game;

namespace SimpleFramework.Game
{
    public interface IGameManager : IManager
    {
        /// <summary>
        /// 获取到游戏状态
        /// </summary>
        /// <returns>游戏状态</returns>
        GameManager.EGameState GetGameState();
    }
}