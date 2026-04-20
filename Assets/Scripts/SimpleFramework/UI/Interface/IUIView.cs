namespace SimpleFramework.UI
{
    /// <summary>
    /// UI视图基接口
    /// </summary>
    public interface IUIView
    {
        /// <summary>
        /// 根据Model刷新视图
        /// </summary>
        /// <param name="model">UI模型数据</param>
        void RefreshView(IUIModel model);

        /// <summary>
        /// 进入视图
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 退出视图
        /// </summary>
        void OnExit();

        /// <summary>
        /// 恢复视图
        /// </summary>
        void OnResume();

        /// <summary>
        /// 暂停视图
        /// </summary>
        void OnPause();
    }
}
