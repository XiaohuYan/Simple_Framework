using UnityEngine;

namespace SimpleFramework.Player
{
    public class PlayerManager : IPlayerManager
    {
        private GameObject player;

        /// <summary>
        /// 获取到玩家
        /// </summary>
        /// <returns>玩家</returns>
        public GameObject GetPlayer()
        {
            if (player != null)
            {
                return player;
            }
            else
            {
                player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    return player;
                }
                player = GameObject.Find("Player");
                if (player != null)
                {
                    return player;
                }
            }
            Debug.LogWarning("无法通过Player标签或Player名找到玩家");
            return null;          
        }
        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {

        }

        public void OnManagerDestroy()
        {
            player = null;
        }
    }
}