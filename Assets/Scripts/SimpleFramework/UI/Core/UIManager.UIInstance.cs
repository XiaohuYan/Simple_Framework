using SimpleFramework.ObjectPool;
using UnityEngine;

namespace SimpleFramework.UI
{
    public partial class UIManager : IUIManager
    {
        /// <summary>
        /// 对象池管理的 ui 实例
        /// </summary>
        private sealed class UIInstance : ObjectBase
        {
            /// <summary> 加载的 ui 预制体 </summary>
            private object uiAsset;

            public UIInstance(string name, object uiAsset, object realObj) : base(name)
            {
                this.uiAsset = uiAsset;
                Initial(realObj);
            }

            public override void Release()
            {
                GameObject.Destroy(RealObject as UnityEngine.Object);
                Clear();
            }

            public override void Clear()
            {
                base.Clear();
                uiAsset = null;
            }
        }
    }
}