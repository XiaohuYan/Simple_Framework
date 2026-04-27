namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 视图接口
    /// 定义 UI 视图的核心功能
    /// </summary>
    public interface IUIView
    {
        /// <summary>
        /// 刷新视图
        /// 根据模型数据更新 UI 显示
        /// </summary>
        /// <param name="model">UI 模型数据</param>
        void RefreshView(IUIModel model);

        /// <summary>
        /// 进入视图
        /// 面板显示时调用
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 退出视图
        /// 面板隐藏时调用
        /// </summary>
        void OnExit();

        /// <summary>
        /// 恢复视图
        /// 面板从暂停状态恢复时调用
        /// </summary>
        void OnResume();

        /// <summary>
        /// 暂停视图
        /// 面板被其他面板覆盖时调用
        /// </summary>
        void OnPause();
    }
}
