using System;
using System.Collections.Generic;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置表解析器工厂
    /// 使用工厂模式创建合适的解析器策略
    /// </summary>
    public static class ConfigParserFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, IConfigParserStrategy> s_parsers = new Dictionary<string, IConfigParserStrategy>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 静态构造函数 - 注册所有内置解析器(首次访问时执行一次)
        /// </summary>
        static ConfigParserFactory()
        {
            // 注册 XML 解析器
            RegisterParser(new XmlConfigParser());
            
            // 注册 JSON 解析器
            RegisterParser(new JsonConfigParser());
        }

        /// <summary>
        /// 注册解析器策略
        /// </summary>
        /// <param name="parser">解析器实例</param>
        public static void RegisterParser(IConfigParserStrategy parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            // 通过测试获取支持的文件扩展名
            var supportedExtensions = new[] { ".xml", ".json", ".bytes" };
            
            foreach (var ext in supportedExtensions)
            {
                if (parser.SupportsFormat(ext))
                {
                    s_parsers[ext] = parser;
                    UnityEngine.Debug.Log($"[ConfigParserFactory] 注册解析器：{parser.GetType().Name} 支持格式：{ext}");
                    break;
                }
            }
        }

        /// <summary>
        /// 根据文件扩展名获取解析器
        /// </summary>
        /// <param name="fileExtension">文件扩展名（包含点）</param>
        /// <returns>解析器实例</returns>
        /// <exception cref="NotSupportedException">不支持的文件格式</exception>
        public static IConfigParserStrategy GetParser(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentNullException(nameof(fileExtension));
            }

            // 确保扩展名以点开头
            var ext = fileExtension.StartsWith(".") ? fileExtension : "." + fileExtension;

            if (s_parsers.TryGetValue(ext, out var parser))
            {
                return parser;
            }

            throw new NotSupportedException($"[ConfigParserFactory] 不支持的文件格式：{ext}");
        }

        /// <summary>
        /// 根据文件路径获取解析器
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>解析器实例</returns>
        public static IConfigParserStrategy GetParserByPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var extension = System.IO.Path.GetExtension(filePath);
            return GetParser(extension);
        }

        /// <summary>
        /// 根据文件类型获取解析器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IConfigParserStrategy GetParserByType(EFormatType type)
        {
            return GetParser("." + type.ToString());
        }

        /// <summary>
        /// 检查是否支持指定格式
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>是否支持</returns>
        public static bool SupportsFormat(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return false;
            }

            var ext = fileExtension.StartsWith(".") ? fileExtension : "." + fileExtension;
            return s_parsers.ContainsKey(ext);
        }

        /// <summary>
        /// 移除指定扩展名的解析器
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>是否成功移除</returns>
        public static bool RemoveParser(string fileExtension)
        {
            var ext = fileExtension.StartsWith(".") ? fileExtension : "." + fileExtension;
            return s_parsers.Remove(ext);
        }

        /// <summary>
        /// 根据类型移除指定扩展名的解析器
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否成功移除</returns>
        public static bool RemoveParser(EFormatType type)
        {
            return RemoveParser("." + type.ToString());
        }

        /// <summary>
        /// 清除所有注册的解析器
        /// </summary>
        public static void ClearParsers()
        {
            s_parsers.Clear();
        }


        /// <summary>
        /// 获取所有支持的格式
        /// </summary>
        /// <returns>支持的格式列表</returns>
        public static List<string> GetSupportedFormats()
        {
            return new List<string>(s_parsers.Keys);
        }

    }
}
