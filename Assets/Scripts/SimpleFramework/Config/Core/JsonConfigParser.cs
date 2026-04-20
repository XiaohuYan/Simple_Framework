using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.Config
{
    /// <summary>
    /// JSON 配置表解析器策略
    /// 使用 Unity 内置的 JsonUtility 进行解析
    /// </summary>
    public class JsonConfigParser : IConfigParserStrategy
    {
        public bool SupportsFormat(string extension)
        {
            return extension.Equals(".json", StringComparison.OrdinalIgnoreCase);
        }

        public ConfigDataResult<T> Parse<T>(string filePath, string content) where T : ConfigDataBase, new()
        {
            var result = new ConfigDataResult<T>();

            try
            {
                // 尝试解析为配置数组
                var wrapper = JsonUtility.FromJson<ConfigArrayWrapper<T>>(content);

                if (wrapper == null || wrapper.items == null)
                {
                    // 尝试直接解析为单个对象数组
                    var jsonFix = $"{{\"items\":{content}}}";
                    if (content.Trim().StartsWith("["))
                    {
                        wrapper = JsonUtility.FromJson<ConfigArrayWrapper<T>>(jsonFix);
                    }
                }

                if (wrapper?.items == null)
                {
                    result.Success = false;
                    result.ErrorMessage = $"[JSON Config] 解析失败：{filePath}\n错误：无效的 JSON 格式";
                    Debug.LogError(result.ErrorMessage);
                    return result;
                }

                var dataList = new List<T>();
                var dataDict = new Dictionary<int, T>();

                foreach (var item in wrapper.items)
                {
                    if (item.Validate())
                    {
                        dataList.Add(item);
                        dataDict[item.Id] = item;
                    }
                    else
                    {
                        Debug.LogWarning($"[JSON Config] 数据验证失败：{filePath}, ID={item.Id}");
                    }
                }

                result.DataList = dataList;
                result.DataDict = dataDict;
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = $"[JSON Config] 解析失败：{filePath}错误：{e.Message}";
                Debug.LogError(result.ErrorMessage);
            }

            return result;
        }
    }

    /// <summary>
    /// JSON 数组包装器（用于 JsonUtility 解析数组）
    /// </summary>
    /// <typeparam name="T">配置数据类型</typeparam>
    [Serializable]
    public class ConfigArrayWrapper<T> where T : ConfigDataBase, new()
    {
        public T[] items;
    }
}
