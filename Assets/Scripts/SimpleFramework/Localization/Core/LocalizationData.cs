using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.Localization
{

    /// <summary>
    /// 本地化条目
    /// </summary>
    [Serializable]
    public class LocalizationEntry
    {
        /// <summary>
        /// 键名
        /// </summary>
        public string Key;

        /// <summary>
        /// 翻译后的文本
        /// </summary>
        public string Value;
    }

    /// <summary>
    /// 本地化数据表
    /// 存储特定语言的所有文本
    /// </summary>
    [Serializable]
    public class LocalizationData
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        public SupportedLanguage Language;

        /// <summary>
        /// 语言代码
        /// </summary>
        public string LanguageCode;

        /// <summary>
        /// 本地化键值对列表
        /// </summary>
        public readonly List<LocalizationEntry> Entries = new List<LocalizationEntry>();

        /// <summary>
        /// 快速查找字典
        /// </summary>
        private Dictionary<string, string> m_lookupDict;

        /// <summary>
        /// 构建查找字典(慢启动)
        /// </summary>
        public void BuildLookupDict()
        {
            m_lookupDict = new Dictionary<string, string>();
            foreach (var entry in Entries)
            {
                if (!m_lookupDict.ContainsKey(entry.Key))
                {
                    m_lookupDict[entry.Key] = entry.Value;
                }
            }
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        public string GetText(string key, string defaultValue = "")
        {
            if (m_lookupDict == null)
            {
                BuildLookupDict();
            }

            if (m_lookupDict.TryGetValue(key, out var value))
            {
                return value;
            }

            Debug.LogWarning($"[Localization] 未找到键：{key}");
            return defaultValue;
        }

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        public bool ContainsKey(string key)
        {
            if (m_lookupDict == null)
            {
                BuildLookupDict();
            }

            return m_lookupDict.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            if(m_lookupDict != null)
            {
                m_lookupDict.Clear();
                m_lookupDict = null;
            }
            Entries.Clear();
        }
    }
}
