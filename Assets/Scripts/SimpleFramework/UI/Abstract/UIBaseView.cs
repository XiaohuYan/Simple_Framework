using UnityEngine;

namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 视图基类
    /// 所有 UI 面板视图都应该继承此类
    /// </summary>
    public abstract class UIBaseView : MonoBehaviour, IUIView
    {
        /// <summary>
        /// 刷新视图
        /// 根据模型数据更新 UI 显示
        /// </summary>
        /// <param name="model">UI 模型数据</param>
        public abstract void RefreshView(IUIModel model);

        /// <summary>
        /// 进入视图
        /// 面板显示时调用
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 退出视图
        /// 面板隐藏时调用
        /// </summary>
        public virtual void OnExit()
        {
            Removelistener();
        }

        /// <summary>
        /// 恢复视图
        /// 面板从暂停状态恢复时调用
        /// </summary>
        public abstract void OnResume();

        /// <summary>
        /// 暂停视图
        /// 面板被其他面板覆盖时调用
        /// </summary>
        public abstract void OnPause();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        protected abstract void Addlistener();

        /// <summary>
        /// 移除事件监听
        /// </summary>
        protected abstract void Removelistener();
    }
}
