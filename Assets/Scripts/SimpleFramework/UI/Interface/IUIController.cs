namespace SimpleFramework.UI
{
    /// <summary>
    /// UI控制器基接口
    /// </summary>
    /// <typeparam name="TModel">模型类型</typeparam>
    /// <typeparam name="TView">视图类型</typeparam>
    public interface IUIController<TModel, TView> where TModel : IUIModel, new() where TView : IUIView
    {
        TModel Model { get; }
        TView View { get; }

        /// <summary>
        /// 初始化控制器
        /// </summary>
        void InitController();
    }
}
