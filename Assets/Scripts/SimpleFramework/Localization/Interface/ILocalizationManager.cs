using SimpleFramework.Common;
using System.Collections.Generic;

namespace SimpleFramework.Localization
{
    public interface ILocalizationManager : IManager
    {
        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="language">语言枚举</param>
        void SetLanguage(SupportedLanguage language);

        /// <summary>
        /// 切换到下一种语言
        /// </summary>
        void NextLanguage();

        /// <summary>
        /// 获取本地化文本
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <param name="defaultValue">默认文本</param>
        /// <returns>文本</returns>
        string GetText(string key, string defaultValue = "");

        /// <summary>
        /// 获取本地化文本并带占位符
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <param name="args">占位符</param>
        /// <returns>文本</returns>
        string GetText(string key, params object[] args);

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        /// <param name="key">文本的key</param>
        /// <returns>是否包含key</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 注册本地化文本组件
        /// </summary>
        /// <param name="text">文本组件</param>
        void RegisterLocalizedText(LocalizedText text);

        /// <summary>
        /// 注销本地化文本组件
        /// </summary>
        /// <param name="text">文本组件</param>
        void UnregisterLocalizedText(LocalizedText text);

        /// <summary>
        /// 获取所有支持的语言
        /// </summary>
        /// <returns>所有支持语言的列表</returns>
        List<SupportedLanguage> GetSupportedLanguages();

        /// <summary>
        /// 根据枚举获取语言名称
        /// </summary>
        /// <param name="language">语言枚举类型</param>
        /// <returns>语言名称</returns>
        string GetLanguageName(SupportedLanguage language);

        /// <summary>
        /// 获取当前语言名称
        /// </summary>
        /// <returns>语言名称</returns>
        string GetCurrentLanguageName();

        /// <summary>
        /// 是否已加载
        /// </summary>
        /// <returns>是否已加载</returns>
        bool IsLoaded();

        /// <summary>
        /// 当前语言代码
        /// </summary>
        /// <returns>当前语言代码</returns>
        string CurrentLanguageCode();
    }
}