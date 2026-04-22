using SimpleFramework.Common;
using UnityEngine;

namespace SimpleFramework.Persistence
{
    public interface IPersistenceManager:IManager
    {
        /// <summary>
        /// 保存 PlayerPrefs 整数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        void SaveIntPlayerPrefs(string key, int value);

        /// <summary>
        /// 获取 PlayerPrefs 整数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 整数</returns>
        int GetIntPlayerPrefs(string key, int defaultValue = 0);

        /// <summary>
        /// 保存 PlayerPrefs 浮点数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        void SaveFloatPlayerPrefs(string key, float value);

        /// <summary>
        /// 获取 PlayerPrefs 浮点数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 浮点数</returns>
        float GetFloatPlayerPrefs(string key, float defaultValue = 0f);

        /// <summary>
        /// 保存 PlayerPrefs 字符串
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        void SaveStringPlayerPrefs(string key, string value);

        /// <summary>
        /// 获取 PlayerPrefs 字符串
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 字符串</returns>
        string GetStringPlayerPrefs(string key, string defaultValue = "");

        /// <summary>
        /// 获取 PlayerPrefs 布尔值
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        void SaveBoolPlayerPrefs(string key, bool value);

        /// <summary>
        /// 获取 PlayerPrefs 布尔值
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 布尔值</returns>
        bool GetBoolPlayerPrefs(string key, bool defaultValue = false);

        /// <summary>
        /// 检查键知否存在
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <returns>键是否存在</returns>
        bool HasPlayerPrefsKey(string key);

        /// <summary>
        /// 删除 PlayerPrefs 对应的键
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        void DeletePlayerPrefsKey(string key);

        /// <summary>
        /// 清空所有 PlayerPrefs 数据
        /// </summary>
        void DeleteAllPlayerPrefs();

        /// <summary>
        /// 保存所有 PlayerPrefs 数据
        /// </summary>
        void SaveAllPlayerPrefs();

        /// <summary>
        /// 存储为 JSON 格式
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        /// <param name="useEncryption">是否加密</param>
        void SaveToJson<T>(string fileName, T data, bool useEncryption = false) where T : SaveDataBase;

        /// <summary>
        ///  从 JSON 文件加载对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="useEncryption">是否加密过</param>
        /// <returns>数据</returns>
        T LoadFromJson<T>(string fileName, bool useEncryption = false) where T : SaveDataBase;

        /// <summary>
        /// 保存为二进制
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        void SaveToBinary<T>(string fileName, T data) where T : SaveDataBase;

        /// <summary>
        /// 从二进制文件加载
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns>数据</returns>
        T LoadFromBinary<T>(string fileName) where T : SaveDataBase;

        /// <summary>
        /// 获取持久化目录路径
        /// </summary>
        /// <returns>持久化目录路径</returns>
        string GetPersistencePath();

        /// <summary>
        /// 获取文件字节大小
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>字节大小</returns>
        long GetFileSize(string fileName);

        /// <summary>
        /// 获取所有保存的文件
        /// </summary>
        /// <returns>所有保存文件路径</returns>
        string[] GetAllFiles();

        /// <summary>
        /// 清空所有持久化数据
        /// </summary>
        void ClearAllPersistenceFiles();
    }
}