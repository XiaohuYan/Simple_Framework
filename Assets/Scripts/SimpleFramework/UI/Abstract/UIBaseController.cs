namespace SimpleFramework.UI
{
    /// <summary>
    /// UI控制器基类（所有UI控制器的父类）
    /// </summary>
    /// <typeparam name="TModel">模型类型</typeparam>
    /// <typeparam name="TView">视图类型</typeparam>
    public abstract class UIBaseController<TModel, TView> : UIControllerCore, IUIController<TModel, TView>
    where TModel : UIBaseModel, new()
        where TView : UIBaseView
    {
        public TModel Model { get; protected set; }
        public TView View { get; protected set; }

        public override void InitController(UIBaseView view)
        {
            TView targetView = view as TView;
            if (targetView == null)
            {
                return;
            }

            Model = new TModel();
            InitModel();
            View = targetView;
            RegisterViewEvents();
        }

        /// <summary>
        /// 初始化控制器（无参数重载）
        /// </summary>
        public void InitController()
        {
            if (View == null)
            {
                UnityEngine.Debug.LogError("视图未赋值，初始化失败");
                return;
            }
            InitController(View);
        }

        /// <summary>
        /// 初始化模型数据
        /// </summary>
        protected abstract void InitModel();

        /// <summary>
        /// 注册视图事件
        /// </summary>
        protected abstract void RegisterViewEvents();

        /// <summary>
        /// 卸载事件
        /// </summary>
        protected abstract void UnregisterViewEvents();

        /// <summary>
        /// 更新视图数据
        /// </summary>
        protected virtual void UpdateView()
        {
            View.RefreshView(Model);
        }

        /// <summary>
        /// 销毁控制器
        /// </summary>
        public override void DisposeController()
        {
            UnregisterViewEvents();
            Model = null;
            View = null;
        }
    }
}