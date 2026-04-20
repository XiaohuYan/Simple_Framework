using SimpleFramework.Common;
using System;

namespace SimpleFramework.Config
{
    public interface IConfigManager:IManager
    {
        ConfigDataResult<T> LoadConfig<T>(string configName, string content, string fileExtension) where T : ConfigDataBase, new();
        ConfigDataResult<T> LoadConfigFromResources<T>(string configPath) where T : ConfigDataBase, new();
        ConfigDataResult<T> LoadConfigFromFile<T>(string filePath) where T : ConfigDataBase, new();
        void LoadConfigFromResourcesAsync<T>(string configPath, Action<ConfigDataResult<T>> callback)  where T : ConfigDataBase, new();
        ConfigDataResult<T> GetConfig<T>(string configName) where T : ConfigDataBase, new();
        T GetConfigById<T>(string configName, int id) where T : ConfigDataBase, new();
        bool IsConfigLoaded(string configName);
        void UnloadConfig(string configName, bool unloadAll = false);
        void ClearAllConfigs();

        public int LoadedConfigCount { get; }
    }
}