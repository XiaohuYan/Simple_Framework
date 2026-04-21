using System;

namespace SimpleFramework.Localization
{
    /// <summary>
    /// 支持的语言枚举
    /// </summary>
    public enum SupportedLanguage
    {
        /// <summary> 英语 </summary>
        English = 0,
        
        /// <summary> 简体中文 </summary>
        ChineseSimplified = 1,
        
        /// <summary> 繁体中文 </summary>
        ChineseTraditional = 2,
        
        /// <summary> 日语 </summary>
        Japanese = 3,
        
        /// <summary> 韩语 </summary>
        Korean = 4,
        
        /// <summary> 法语 </summary>
        French = 5,
        
        /// <summary> 德语 </summary>
        German = 6,
        
        /// <summary> 西班牙语 </summary>
        Spanish = 7,
        
        /// <summary> 俄语 </summary>
        Russian = 8
    }

    /// <summary>
    /// 语言扩展方法
    /// </summary>
    public static class LanguageExtensions
    {
        /// <summary>
        /// 获取语言代码（如：en, zh-CN, zh-TW）
        /// </summary>
        public static string GetLanguageCode(this SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.English:
                    return "en";
                case SupportedLanguage.ChineseSimplified:
                    return "zh-CN";
                case SupportedLanguage.ChineseTraditional:
                    return "zh-TW";
                case SupportedLanguage.Japanese:
                    return "ja";
                case SupportedLanguage.Korean:
                    return "ko";
                case SupportedLanguage.French:
                    return "fr";
                case SupportedLanguage.German:
                    return "de";
                case SupportedLanguage.Spanish:
                    return "es";
                case SupportedLanguage.Russian:
                    return "ru";
                default:
                    return "en";
            }
        }

        /// <summary>
        /// 获取语言名称
        /// </summary>
        public static string GetLanguageName(this SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.English:
                    return "English";
                case SupportedLanguage.ChineseSimplified:
                    return "简体中文";
                case SupportedLanguage.ChineseTraditional:
                    return "繁體中文";
                case SupportedLanguage.Japanese:
                    return "日本語";
                case SupportedLanguage.Korean:
                    return "한국어";
                case SupportedLanguage.French:
                    return "Français";
                case SupportedLanguage.German:
                    return "Deutsch";
                case SupportedLanguage.Spanish:
                    return "Español";
                case SupportedLanguage.Russian:
                    return "Русский";
                default:
                    return "English";
            }
        }
    }
}
