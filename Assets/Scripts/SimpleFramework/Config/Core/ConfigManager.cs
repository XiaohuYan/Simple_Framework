using System.Collections.Generic;
using System;
using UnityEngine;
using SimpleFramework.Common;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置表管理器
    /// 使用工厂模式 + 策略模式管理配置表的加载和解析
    /// </summary>
    public class ConfigManager:IManager
    {
        /// <summary>
        /// 已加载的配置表缓存
        /// Key: 配置表名称
        /// Value: 配置数据基类
        /// </summary>
        private readonly Dictionary<string, object> m_loadedConfigs = new Dictionary<string, object>();

        /// <summary>
        /// 配置表加载回调
        /// </summary>
        public event Action<string, bool> OnConfigLoaded;


        /// <summary>
        /// 加载配置表
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="configName">配置表名称（不含扩展名）</param>
        /// <param name="content">文件内容</param>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>配置数据结果</returns>
        public ConfigDataResult<T> LoadConfig<T>(string configName, string content, string fileExtension)
            where T : ConfigDataBase, new()
        {
            if (string.IsNullOrEmpty(configName))
            {
                Debug.LogError("[ConfigManager] 配置表名称不能为空");
                return null;
            }

            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"[ConfigManager] 配置表内容为空：{configName}");
                return null;
            }

            try
            {
                // 通过工厂获取对应的解析器策略
                var parser = ConfigParserFactory.GetParser(fileExtension);

                // 使用策略解析器解析配置
                var result = parser.Parse<T>(configName + fileExtension, content);

                if (result.Success)
                {
                    // 缓存配置数据
                    m_loadedConfigs[configName] = result;
                    Debug.Log($"[ConfigManager] 成功加载配置表：{configName}{fileExtension}, 数据数量：{result.Count}");
                }
                else
                {
                    Debug.LogError($"[ConfigManager] 加载配置表失败：{configName}{fileExtension}\n{result.ErrorMessage}");
                }

                // 触发加载完成事件
                OnConfigLoaded?.Invoke(configName, result.Success);

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[ConfigManager] 加载配置表异常：{configName}{fileExtension}\n{e.Message}\n{e.StackTrace}");
                OnConfigLoaded?.Invoke(configName, false);
                return null;
            }
        }

        /// <summary>
        /// 从 Resources 目录加载配置表
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="configPath">Resources 中的相对路径（不含扩展名）</param>
        /// <returns>配置数据结果</returns>
        public ConfigDataResult<T> LoadConfigFromResources<T>(string configPath) where T : ConfigDataBase, new()
        {
            if (string.IsNullOrEmpty(configPath))
            {
                Debug.LogError("[ConfigManager] 配置表路径不能为空");
                return null;
            }

            // 获取文件扩展名
            var extension = System.IO.Path.GetExtension(configPath);
            var configName = System.IO.Path.GetFileNameWithoutExtension(configPath);

            // 从 Resources 加载文本
            var textAsset = Resources.Load<TextAsset>(configPath);
            if (textAsset == null)
            {
                Debug.LogError($"[ConfigManager] 无法从 Resources 加载：{configPath}");
                return null;
            }

            // 解析配置
            var result = LoadConfig<T>(configName, textAsset.text, extension);

            // 释放资源
            Resources.UnloadAsset(textAsset);

            return result;
        }

        /// <summary>
        /// 从文件加载配置表
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="filePath">文件完整路径</param>
        /// <returns>配置数据结果</returns>
        public ConfigDataResult<T> LoadConfigFromFile<T>(string filePath) where T : ConfigDataBase, new()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("[ConfigManager] 文件路径不能为空");
                return null;
            }

            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError($"[ConfigManager] 文件不存在：{filePath}");
                return null;
            }

            var configName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            var extension = System.IO.Path.GetExtension(filePath);

            try
            {
                var content = System.IO.File.ReadAllText(filePath);
                return LoadConfig<T>(configName, content, extension);
            }
            catch (Exception e)
            {
                Debug.LogError($"[ConfigManager] 读取文件失败：{filePath}\n{e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 异步加载配置表（从 Resources）
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="configPath">Resources 中的相对路径（不含扩展名）</param>
        /// <param name="callback">加载完成回调</param>
        public void LoadConfigFromResourcesAsync<T>(string configPath, Action<ConfigDataResult<T>> callback)
            where T : ConfigDataBase, new()
        {
            if (string.IsNullOrEmpty(configPath))
            {
                Debug.LogError("[ConfigManager] 配置表路径不能为空");
                callback?.Invoke(null);
                return;
            }

            var extension = System.IO.Path.GetExtension(configPath);
            var configName = System.IO.Path.GetFileNameWithoutExtension(configPath);

            var request = Resources.LoadAsync<TextAsset>(configPath);
            request.completed += (op) =>
            {
                var textAsset = op as ResourceRequest;
                if (textAsset?.asset == null)
                {
                    Debug.LogError($"[ConfigManager] 无法从 Resources 加载：{configPath}");
                    callback?.Invoke(null);
                    return;
                }

                var result = LoadConfig<T>(configName, ((TextAsset)textAsset.asset).text, extension);
                Resources.UnloadAsset(textAsset.asset);
                callback?.Invoke(result);
            };
        }

        /// <summary>
        /// 获取已加载的配置
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="configName">配置表名称（不含扩展名）</param>
        /// <returns>配置数据结果</returns>
        public ConfigDataResult<T> GetConfig<T>(string configName) where T : ConfigDataBase, new()
        {
            if (m_loadedConfigs.TryGetValue(configName, out var config))
            {
                return config as ConfigDataResult<T>;
            }

            Debug.LogWarning($"[ConfigManager] 配置表未加载：{configName}");
            return null;
        }

        /// <summary>
        /// 根据 ID 获取配置数据
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="configName">配置表名称</param>
        /// <param name="id">配置 ID</param>
        /// <returns>配置数据</returns>
        public T GetConfigById<T>(string configName, int id) where T : ConfigDataBase, new()
        {
            var config = GetConfig<T>(configName);
            return config?.GetById(id);
        }

        /// <summary>
        /// 检查配置表是否已加载
        /// </summary>
        /// <param name="configName">配置表名称</param>
        /// <returns>是否已加载</returns>
        public bool IsConfigLoaded(string configName)
        {
            return m_loadedConfigs.ContainsKey(configName);
        }

        /// <summary>
        /// 卸载配置表
        /// </summary>
        /// <param name="configName">配置表名称</param>
        /// <param name="unloadAll">是否卸载所有配置</param>
        public void UnloadConfig(string configName, bool unloadAll = false)
        {
            if (unloadAll)
            {
                m_loadedConfigs.Clear();
                Debug.Log("[ConfigManager] 已卸载所有配置表");
            }
            else
            {
                if (m_loadedConfigs.Remove(configName))
                {
                    Debug.Log($"[ConfigManager] 已卸载配置表：{configName}");
                }
            }
        }

        /// <summary>
        /// 清空所有配置缓存
        /// </summary>
        public void ClearAllConfigs()
        {
            m_loadedConfigs.Clear();
            Debug.Log("[ConfigManager] 已清空所有配置缓存");
        }

        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {
         
        }

        public void OnManagerDestroy()
        {
         
        }

        /// <summary>
        /// 获取已加载配置表数量
        /// </summary>
        public int LoadedConfigCount => m_loadedConfigs.Count;
    }
}