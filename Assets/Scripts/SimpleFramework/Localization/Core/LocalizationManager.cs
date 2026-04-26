using SimpleFramework.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.Localization
{
    /// <summary>
    /// 本地化管理器
    /// 管理多语言切换和文本获取
    /// </summary>
    public class LocalizationManager : ILocalizationManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 默认语言
        /// </summary>
        private SupportedLanguage DefaultLanguage = SupportedLanguage.ChineseSimplified;

        /// <summary>
        /// 是否自动检测系统语言
        /// </summary>
        private bool AutoDetectSystemLanguage = true;

        /// <summary>
        /// 本地化数据路径（Resources 目录）
        /// </summary>
        private const string LocalizationDataPath = "Localization";

        /// <summary>
        /// 语言切换事件
        /// </summary>
        public event Action<SupportedLanguage> OnLanguageChanged;

        /// <summary>
        /// 当前语言
        /// </summary>
        private SupportedLanguage currentLanguage;

        /// <summary>
        /// 是否已加载
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// 所有语言数据
        /// </summary>
        private readonly Dictionary<SupportedLanguage, LocalizationData> m_languageDataDict =   new Dictionary<SupportedLanguage, LocalizationData>();

        /// <summary>
        /// 回退数据（当翻译不存在时使用）
        /// </summary>
        private LocalizationData m_fallbackData;

        /// <summary>
        /// 所有需要刷新的本地化文本组件
        /// </summary>
        private readonly List<LocalizedText> m_localizedTexts = new List<LocalizedText>();

        public void OnManagerInit()
        {
            Initialize();
        }

        public void AfterManagerInit() { }

        #region 初始化相关

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            Debug.Log("[LocalizationManager] 初始化本地化管理器");

            // 设置默认语言
            if (AutoDetectSystemLanguage)
            {
                currentLanguage = DetectSystemLanguage();
            }
            else
            {
                currentLanguage = DefaultLanguage;
            }

            // 加载所有本地化数据
            LoadAllLocalizationData();
        }

        /// <summary>
        /// 检测系统语言
        /// </summary>
        private SupportedLanguage DetectSystemLanguage()
        {
            var systemLang = Application.systemLanguage;

            switch (systemLang)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    return SupportedLanguage.ChineseSimplified;

                case SystemLanguage.ChineseTraditional:
                    return SupportedLanguage.ChineseTraditional;

                case SystemLanguage.Japanese:
                    return SupportedLanguage.Japanese;

                case SystemLanguage.Korean:
                    return SupportedLanguage.Korean;

                case SystemLanguage.French:
                    return SupportedLanguage.French;

                case SystemLanguage.German:
                    return SupportedLanguage.German;

                case SystemLanguage.Spanish:
                    return SupportedLanguage.Spanish;

                case SystemLanguage.Russian:
                    return SupportedLanguage.Russian;

                default:
                    return DefaultLanguage;
            }
        }

        /// <summary>
        /// 加载所有本地化数据
        /// </summary>
        private void LoadAllLocalizationData()
        {
            // 从 Resources 加载所有语言的 JSON 文件
            var texts = Resources.LoadAll<TextAsset>(LocalizationDataPath);

            foreach (var text in texts)
            {
                try
                {
                    // 解析 JSON
                    var data = JsonUtility.FromJson<LocalizationData>(text.text);

                    if (data != null && data.Entries != null)
                    {
                        data.BuildLookupDict();
                        m_languageDataDict[data.Language] = data;

                        Debug.Log($"[LocalizationManager] 加载语言：{data.Language} ({data.LanguageCode}), 条目数：{data.Entries.Count}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[LocalizationManager] 加载本地化文件失败：{text.name}\n{e.Message}");
                }
            }

            // 设置回退数据（英语）
            if (m_languageDataDict.TryGetValue(SupportedLanguage.English, out var englishData))
            {
                m_fallbackData = englishData;
            }

            isLoaded = true;

            Debug.Log($"[LocalizationManager] 本地化数据加载完成，共 {m_languageDataDict.Count} 种语言");
        }

        #endregion

        #region 直接切换语言相关

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="language">语言枚举</param>
        public void SetLanguage(SupportedLanguage language)
        {
            if (currentLanguage == language)
            {
                return;
            }

            if (!m_languageDataDict.ContainsKey(language))
            {
                Debug.LogWarning($"[LocalizationManager] 不支持的语言：{language}");
                return;
            }

            currentLanguage = language;
            Debug.Log($"[LocalizationManager] 切换到语言：{language}");

            OnLanguageChanged?.Invoke(language);

            // 刷新所有本地化文本组件
            RefreshAllLocalizedTexts();
        }


        /// <summary>
        /// 刷新所有本地化文本
        /// </summary>
        private void RefreshAllLocalizedTexts()
        {
            foreach (var text in m_localizedTexts)
            {
                text.RefreshText();
            }
        }

        /// <summary>
        /// 切换到下一种语言
        /// </summary>
        public void NextLanguage()
        {
            var languages = new List<SupportedLanguage>(m_languageDataDict.Keys);
            int currentIndex = languages.IndexOf(currentLanguage);
            int nextIndex = (currentIndex + 1) % languages.Count;
            SetLanguage(languages[nextIndex]);
        }

        #endregion

        #region 文本相关

        /// <summary>
        /// 获取本地化文本
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <param name="defaultValue">默认文本</param>
        /// <returns>文本</returns>
        public string GetText(string key, string defaultValue = "")
        {
            if (!isLoaded)
            {
                Debug.LogError("[LocalizationManager] 本地化数据未加载");
                return key;
            }

            // 尝试从当前语言获取
            if (m_languageDataDict.TryGetValue(currentLanguage, out var currentData))
            {
                string text = currentData.GetText(key);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }

            // 尝试从回退数据获取
            if (m_fallbackData != null)
            {
                string text = m_fallbackData.GetText(key);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
            }

            // 返回键名或默认值
            return string.IsNullOrEmpty(defaultValue) ? key : defaultValue;
        }

        /// <summary>
        /// 获取本地化文本并带占位符
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <param name="args">占位符</param>
        /// <returns>文本</returns>
        public string GetText(string key, params object[] args)
        {
            string text = GetText(key);
            if (!string.IsNullOrEmpty(text) && args != null && args.Length > 0)
            {
                try
                {
                    return string.Format(text, args);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[LocalizationManager] 格式化文本失败：{key}\n{e.Message}");
                }
            }
            return text;
        }


        #endregion

        #region 其他

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <returns>是否包含key</returns>
        public bool ContainsKey(string key)
        {
            if (!isLoaded) return false;

            if (m_languageDataDict.TryGetValue(currentLanguage, out var currentData))
            {
                if (currentData.ContainsKey(key))
                {
                    return true;
                }
            }

            return m_fallbackData?.ContainsKey(key) ?? false;
        }

        /// <summary>
        /// 是否已加载
        /// </summary>
        /// <returns>是否已加载</returns>
        public bool IsLoaded() => isLoaded;

        /// <summary>
        /// 当前语言代码
        /// </summary>
        public string CurrentLanguageCode() => currentLanguage.GetLanguageCode();

        #endregion

        #region 文本组件的注册与卸载

        /// <summary>
        /// 注册本地化文本组件
        /// </summary>
        /// <param name="text">文本组件</param>
        public void RegisterLocalizedText(LocalizedText text)
        {
            if (!m_localizedTexts.Contains(text))
            {
                m_localizedTexts.Add(text);
            }
        }

        /// <summary>
        /// 注销本地化文本组件
        /// </summary>
        /// <param name="text">文本组件</param>
        public void UnregisterLocalizedText(LocalizedText text)
        {
            m_localizedTexts.Remove(text);
        }

        #endregion

        #region 语言类型相关
        /// <summary>
        /// 获取所有支持的语言
        /// </summary>
        /// <returns>所有支持语言的列表</returns>
        public List<SupportedLanguage> GetSupportedLanguages()
        {
            return new List<SupportedLanguage>(m_languageDataDict.Keys);
        }

        /// <summary>
        /// 根据枚举获取语言名称
        /// </summary>
        /// <param name="language">语言枚举类型</param>
        /// <returns>语言名称</returns>
        public string GetLanguageName(SupportedLanguage language)
        {
            return language.GetLanguageName();
        }

        /// <summary>
        /// 获取当前语言名称
        /// </summary>
        /// <returns>语言名称</returns>
        public string GetCurrentLanguageName()
        {
            return currentLanguage.GetLanguageName();
        }
        #endregion

        /// <summary>
        /// 清空缓存
        /// </summary>
        private void ClearCache()
        {
            foreach (var data in m_languageDataDict.Values)
            {
                data.Clear();
            }
            m_localizedTexts.Clear();
        }

        public void OnManagerDestroy()
        {
            ClearCache();
        }
    }
}
