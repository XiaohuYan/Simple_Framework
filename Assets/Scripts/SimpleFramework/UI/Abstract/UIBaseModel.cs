using System;

namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 模型基类
    /// 所有 UI 模型都应该继承此类
    /// </summary>
    public abstract class UIBaseModel : IUIModel
    {
        /// <summary>
        /// 数据变化事件
        /// </summary>
        public event Action DataChangeEvent;

        /// <summary>
        /// 初始化模型
        /// 在模型创建时调用
        /// </summary>
        public abstract void InitModel();
    }
}

 
