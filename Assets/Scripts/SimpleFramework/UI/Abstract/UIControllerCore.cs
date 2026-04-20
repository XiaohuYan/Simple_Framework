namespace SimpleFramework.UI
{
    public abstract class UIControllerCore
    {
        /// <summary>
        /// 非泛型版本的 Init 方法
        /// </summary>
        /// <param name="view">视图</param>
        public abstract void InitController(UIBaseView view);

        /// <summary>
        /// 销毁控制器
        /// </summary>
        public abstract void DisposeController();
    }
}