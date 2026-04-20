using SimpleFramework.Common;
using UnityEngine;

namespace SimpleFramework.Player
{
    public interface IPlayerManager : IManager
    {
        /// <summary>
        /// 获取到玩家
        /// </summary>
        /// <returns>玩家</returns>
        GameObject GetPlayer();
    }
}