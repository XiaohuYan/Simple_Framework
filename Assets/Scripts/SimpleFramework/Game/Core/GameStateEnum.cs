namespace SimpleFramework.Game
{
    public partial class GameManager : IGameManager
    {
        public enum GameStateEnum : byte
        {
            Start = 0,      // 启动
            Loading = 1,    // 加载中
            Run = 2,        // 运行
            Pause = 3,      // 暂停
            End = 4,        // 结束
        }
    }
}