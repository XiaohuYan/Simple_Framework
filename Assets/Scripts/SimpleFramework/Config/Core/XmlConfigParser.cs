using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace SimpleFramework.Config
{
    /// <summary>
    /// XML 配置表解析器策略
    /// 使用 XmlReader 进行高效解析
    /// </summary>
    public class XmlConfigParser : IConfigParserStrategy
    {
        public bool SupportsFormat(string extension)
        {
            return extension.Equals(".xml", StringComparison.OrdinalIgnoreCase);
        }

        public ConfigDataResult<T> Parse<T>(string filePath, string content) where T : ConfigDataBase, new()
        {
            var result = new ConfigDataResult<T>();
            
            try
            {
                using (var reader = XmlReader.Create(new StringReader(content), GetXmlReaderSettings()))
                {
                    var dataList = new List<T>();
                    var dataDict = new Dictionary<int, T>();

                    reader.ReadToDescendant("row");

                    do
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "row")
                        {
                            var data = new T();
                            var fieldData = new Dictionary<string, object>();

                            // 读取所有属性
                            while (reader.MoveToNextAttribute())
                            {
                                fieldData[reader.Name] = ConvertValue(reader.Value);
                            }

                            // 解析数据
                            data.Parse(fieldData);

                            // 验证数据
                            if (data.Validate())
                            {
                                dataList.Add(data);
                                dataDict[data.Id] = data;
                            }
                            else
                            {
                                UnityEngine.Debug.LogWarning($"[XML Config] 数据验证失败：{filePath}, ID={data.Id}");
                            }
                        }
                    } while (reader.Read());

                    result.DataList = dataList;
                    result.DataDict = dataDict;
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = $"[XML Config] 解析失败：{filePath}\n错误：{e.Message}";
                UnityEngine.Debug.LogError(result.ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// XML 读取器设置
        /// </summary>
        private XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true,
                DtdProcessing = DtdProcessing.Prohibit, // 禁止 DTD 以确保安全
                ValidationType = ValidationType.None,
                CloseInput = true
            };
        }

        /// <summary>
        /// 转换字符串值为合适的类型
        /// </summary>
        private object ConvertValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // 尝试解析为 int
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                return intValue;
            }

            // 尝试解析为 float
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
            {
                return floatValue;
            }

            // 尝试解析为 bool
            if (bool.TryParse(value, out var boolValue))
            {
                return boolValue;
            }

            // 默认为 string
            return value;
        }
    }
}
