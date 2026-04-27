namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 控制器基类（泛型版本）
    /// 提供类型安全的 UI 控制器实现
    /// </summary>
    /// <typeparam name="TModel">模型类型</typeparam>
    /// <typeparam name="TView">视图类型</typeparam>
    public abstract class UIBaseController<TModel, TView> : UIControllerCore
        where TModel : UIBaseModel, new()
        where TView : UIBaseView
    {
        /// <summary>
        /// 模型实例
        /// </summary>
        private TModel model;

        /// <summary>
        /// 视图实例
        /// </summary>
        private TView view;

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <param name="view">UI 视图实例</param>
        public override void InitController(UIBaseView view)
        {
            TView targetView = view as TView;
            if (targetView == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"[UIBaseController] 视图类型不匹配：期望 {typeof(TView).Name}，实际 {view.GetType().Name}");
#endif
                return;
            }

            model = new TModel();
            model.InitModel();
            view = targetView;
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"[UIBaseController] 控制器已初始化：{GetType().Name}");
#endif
            model.DataChangeEvent += UpdateView;
        }

        /// <summary>
        /// 初始化控制器（无参数重载）
        /// </summary>
        public override void InitController()
        {
            if (view == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("[UIBaseController] 视图未赋值，初始化失败");
#endif
                return;
            }
            InitController(view);
        }

        /// <summary>
        /// 更新视图显示
        /// </summary>
        protected virtual void UpdateView()
        {
            view.RefreshView(model);
        }

        public override T GetModel<T>()
        {
            return model as T;
        }

        public override T GetView<T>()
        {
            return view as T;
        }

        /// <summary>
        /// 销毁控制器
        /// </summary>
        public override void DisposeController()
        {
            model = null;
            view = null;
            model.DataChangeEvent -= UpdateView;
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"[UIBaseController] 控制器已销毁：{GetType().Name}");
#endif
        }
    }
}