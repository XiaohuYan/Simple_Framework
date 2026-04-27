namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 控制器基类
    /// 所有 UI 控制器都应该继承此类(直接用泛型会导致报错，协变作为输入参数有问题所以不能完全解决，因此写了一个基类)
    /// </summary>
    public abstract class UIControllerCore
    {
        /// <summary>
        /// 初始化控制器
        /// </summary>
        public abstract void InitController();

        /// <summary>
        /// 初始化控制器
        /// 在面板创建时调用
        /// </summary>
        /// <param name="view">UI 视图实例</param>
        public abstract void InitController(UIBaseView view);

        /// <summary>
        /// 销毁控制器
        /// 在面板销毁时调用
        /// </summary>
        public abstract void DisposeController();

        public abstract T GetModel<T>() where T : UIBaseModel;

        public abstract T GetView<T>() where T : UIBaseView;
    }
}