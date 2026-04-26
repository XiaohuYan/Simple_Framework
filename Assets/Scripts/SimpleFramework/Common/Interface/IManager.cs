namespace SimpleFramework.Common
{
    /// <summary>
    /// 所有manager的接口，统一通过GameFacade类调用
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// 优先级，主要用于最后卸载时释放顺序
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 初始化时调用
        /// </summary>
        void OnManagerInit();

        /// <summary>
        /// 初始化之后调用
        /// </summary>
        void AfterManagerInit();

        /// <summary>
        /// 销毁时调用
        /// </summary>
        void OnManagerDestroy();
    }
}