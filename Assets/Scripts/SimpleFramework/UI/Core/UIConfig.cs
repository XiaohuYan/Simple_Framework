using System;

namespace SimpleFramework.UI
{
    /// <summary>
    /// UI 关闭方法
    /// </summary>
    public enum UICloseType : sbyte
    {
        /// <summary> 设置大小为0 </summary>
        SetVectorZero,

        /// <summary> 设置 Active </summary>
        SetActiveFalse,

        /// <summary> 设置渲染组件 </summary>
        SetCanvasRendererFalse,

        /// <summary> 设置透明度为0 </summary>
        SetCanvasGroupAlphaZero,
    }

    /// <summary>
    /// UI 动画类型
    /// </summary>
    public enum UIAnimationType : sbyte
    {
        /// <summary> 无动画 </summary>
        None,

        /// <summary> 淡入淡出 </summary>
        Fade,

        /// <summary> 缩放 </summary>
        Scale,

        /// <summary> 滑动 </summary>
        Slide,

        /// <summary> 旋转 </summary>
        Rotate,

        /// <summary> 自定义 </summary>
        Custom
    }

    /// <summary>
    /// UI 配置数据(可以配置)
    /// </summary>
    [Serializable]
    public class UIConfig
    {
        /// <summary>
        /// 面板名称
        /// </summary>
        public string PanelName { get; private set; }

        /// <summary>
        /// 关闭模式
        /// </summary>
        public UICloseType CloseType { get; private set; } = UICloseType.SetCanvasRendererFalse;

        /// <summary>
        /// 打开动画类型
        /// </summary>
        public UIAnimationType OpenAnimation { get; private set; } = UIAnimationType.None;

        /// <summary>
        /// 关闭动画类型
        /// </summary>
        public UIAnimationType CloseAnimation { get; private set; } = UIAnimationType.None;

        /// <summary>
        /// UI 层级
        /// </summary>
        public UILayerType LayerType { get; private set; } = UILayerType.Normal;

        /// <summary>
        /// 动画持续时间（秒）
        /// </summary>
        public float AnimationDuration { get; private set; } = 0.3f;

        /// <summary>
        /// 是否覆盖上一个 UI
        /// </summary>
        public bool isCoveredLastView { get; private set; } = true;

        /// <summary>
        /// 用户数据
        /// </summary>
        public object UserData { get; private set; }

        public UIConfig(string name)
        {
            PanelName = name;
            UserData = null;
        }

        public UIConfig(string name, object userData)
        {
            PanelName = name;
            UserData = userData;
        }

        public UIConfig SetIsCoveredLastView(bool value)
        {
            isCoveredLastView = value;
            return this;
        }

        public UIConfig SetAnimationDuration(float value)
        {
            AnimationDuration = value;
            return this;
        }

        public UIConfig SetUILayerType(UILayerType value)
        {
            LayerType = value;
            return this;
        }

        public UIConfig SetUIOpenAnimationType(UIAnimationType value)
        {
            OpenAnimation = value;
            return this;
        }

        public UIConfig SetUICloseAnimationType(UIAnimationType value)
        {
            CloseAnimation = value;
            return this;
        }

        public UIConfig SetUICloseType(UICloseType value)
        {
            CloseType = value;
            return this;
        }

        public void Clear()
        {
            UserData = null;
            PanelName = null;
        }
    }
}
