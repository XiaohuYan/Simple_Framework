using UnityEngine;

namespace SimpleFramework.UI
{
    public abstract class UIBaseView :MonoBehaviour, IUIView
    {
        /// <summary>
        /// 刷新视图
        /// </summary>
        /// <param name="model">UI模型数据</param>
        public abstract void RefreshView(IUIModel model);

        /// <summary>
        /// 进入视图
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 退出视图
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// 恢复视图
        /// </summary>
        public abstract void OnResume();

        /// <summary>
        /// 暂停视图
        /// </summary>
        public abstract void OnPause();
    }
}
