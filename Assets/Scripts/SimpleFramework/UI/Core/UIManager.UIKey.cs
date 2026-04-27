using SimpleFramework.Common.TypeNameKey;
using System;

namespace SimpleFramework.UI
{
    public partial class UIManager : IUIManager
    {
        /// <summary>
        /// UI ¼ü
        /// </summary>
        private readonly struct UIKey
        {
            public readonly TypeNameKey key;

            public UIKey(Type type)
            {
                key = new TypeNameKey(type, nameof(type));
            }

            public UIKey(Type type, string name)
            {
                key = new TypeNameKey(type, name);
            }
        }
    }
}