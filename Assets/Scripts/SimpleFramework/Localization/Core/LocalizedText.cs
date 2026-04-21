using SimpleFramework.Entry;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleFramework.Localization
{
    /// <summary>
    /// 本地化文本组件(需要本地化的文件挂载此脚本)
    /// 自动根据当前语言设置文本
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        /// <summary>
        /// 本地化键名
        /// </summary>
        [Tooltip("本地化键名")]
        [SerializeField]
        private string LocalizationKey;

        /// <summary>
        /// 是否在启动时自动设置文本
        /// </summary>
        [Tooltip("启动时自动设置")]
        [SerializeField]
        private bool SetTextOnStart = true;

        /// <summary>
        /// 格式化参数
        /// </summary>
        [Tooltip("格式化参数")]
        public string[] FormatArgs;

        /// <summary>
        /// 当前文字文本组件
        /// </summary>
        private Text m_textComponent;

        /// <summary>
        /// 本地化管理器
        /// </summary>
        private ILocalizationManager localizationManager;


        private void Awake()
        {
            m_textComponent = GetComponent<Text>();
        }

        private void Start()
        {
            localizationManager = GameFacade.Instance.GetManager<LocalizationManager>();
            if (SetTextOnStart)
            {
                RefreshText();
            }
            // 注册到本地化管理器
            localizationManager.RegisterLocalizedText(this);
        }


        /// <summary>
        /// 刷新文本
        /// </summary>
        public void RefreshText()
        {
            if (string.IsNullOrEmpty(LocalizationKey))
            {
                Debug.LogWarning($"[LocalizedText] {gameObject.name} 未设置 LocalizationKey");
                return;
            }

            string text;

            if (FormatArgs != null && FormatArgs.Length > 0)
            {
                text = GameFacade.Instance.GetManager<LocalizationManager>().GetText(LocalizationKey, (object[])FormatArgs);
            }
            else
            {
                text = GameFacade.Instance.GetManager<LocalizationManager>().GetText(LocalizationKey);
            }

            if (m_textComponent != null)
            {
                m_textComponent.text = text;
            }
        }

        /// <summary>
        /// 设置本地化键
        /// </summary>
        public void SetLocalizationKey(string key)
        {
            LocalizationKey = key;
            RefreshText();
        }

        /// <summary>
        /// 设置格式化参数
        /// </summary>
        public void SetFormatArgs(params string[] args)
        {
            FormatArgs = args;
            RefreshText();
        }


        /// <summary>
        /// 直接设置文本（不通过本地化）
        /// </summary>
        public void SetTextDirect(string text)
        {
            if (m_textComponent != null)
            {
                m_textComponent.text = text;
            }
        }

        private void OnDestroy()
        {
            // 注销
            localizationManager?.UnregisterLocalizedText(this);
        }

    }
}
