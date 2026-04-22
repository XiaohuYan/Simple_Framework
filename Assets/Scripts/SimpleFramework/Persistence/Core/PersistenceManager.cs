using System;
using System.IO;
using UnityEngine;

namespace SimpleFramework.Persistence
{
    /// <summary>
    /// 数据持久化管理器
    /// 提供多种数据存储方式
    /// </summary>
    public class PersistenceManager : IPersistenceManager
    {
        /// <summary>
        /// 持久化文件目录名
        /// </summary>
        private const string PersistenceDirectoryName = "SaveData";

        /// <summary>
        /// 是否加密保存
        /// </summary>
        private bool enableEncryption = false;

        /// <summary>
        /// 加密密钥（简单 XOR 加密）
        /// </summary>
        private const string EncryptionKey = "SimpleFramework2024";

        /// <summary>
        /// 持久化目录路径
        /// </summary>
        private string persistencePath;

        /// <summary>
        /// 文件格式
        /// </summary>
        private enum FileType : sbyte
        {
            JSON = 0, // Json 文件
            BIN = 1 // 二进制文件
        }

        #region 初始化

        public void OnManagerInit()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            // 设置持久化路径
            persistencePath = Path.Combine(Application.persistentDataPath, PersistenceDirectoryName);

            // 确保目录存在
            if (!Directory.Exists(persistencePath))
            {
                Directory.CreateDirectory(persistencePath);
            }
#if UNITY_EDITOR
            Debug.Log($"[PersistenceManager] 初始化完成，路径：{persistencePath}");
#endif
        }

        #endregion

        #region PlayerPrefs 封装

        /// <summary>
        /// 保存 PlayerPrefs 整数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        public void SaveIntPlayerPrefs(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// 获取 PlayerPrefs 整数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        /// <returns>PlayerPrefs 整数</returns>
        public int GetIntPlayerPrefs(string key, int defaultValue = 0)
        {
            return HasPlayerPrefsKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
        }

        /// <summary>
        /// 保存 PlayerPrefs 浮点数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        public void SaveFloatPlayerPrefs(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        /// <summary>
        /// 获取 PlayerPrefs 浮点数
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        /// <returns>PlayerPrefs 浮点数</returns>
        public float GetFloatPlayerPrefs(string key, float defaultValue = 0f)
        {
            return HasPlayerPrefsKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
        }

        /// <summary>
        /// 获取 PlayerPrefs 字符串
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        public void SaveStringPlayerPrefs(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// 获取 PlayerPrefs 字符串
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 字符串</returns>
        public string GetStringPlayerPrefs(string key, string defaultValue = "")
        {
            return HasPlayerPrefsKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
        }

        /// <summary>
        /// 获取 PlayerPrefs 布尔值
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="value">PlayerPrefs 值</param>
        public void SaveBoolPlayerPrefs(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        /// <summary>
        /// 获取 PlayerPrefs 布尔值
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <param name="defaultValue">如果获取不到得到的默认值</param>
        /// <returns>PlayerPrefs 布尔值</returns>
        public bool GetBoolPlayerPrefs(string key, bool defaultValue = false)
        {
            return HasPlayerPrefsKey(key) ? PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1 : defaultValue;
        }

        /// <summary>
        /// 检查键知否存在
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        /// <returns>键是否存在</returns>
        public bool HasPlayerPrefsKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// 删除 PlayerPrefs 对应的键
        /// </summary>
        /// <param name="key">PlayerPrefs 键</param>
        public void DeletePlayerPrefsKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        /// <summary>
        /// 清空所有 PlayerPrefs 数据
        /// </summary>
        public void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 保存所有 PlayerPrefs 数据
        /// </summary>
        public void SaveAllPlayerPrefs()
        {
            PlayerPrefs.Save();
        }

        #endregion

        #region JSON 文件存储

        /// <summary>
        /// 存储为 JSON 格式
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        /// <param name="useEncryption">是否加密</param>
        public void SaveToJson<T>(string fileName, T data, bool useEncryption = false) where T:SaveDataBase
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                string filePath = GetFilePath(fileName,FileType.JSON);

                if (useEncryption || enableEncryption)
                {
                    json = Encrypt(json);
                }

                File.WriteAllText(filePath, json);
#if UNITY_EDITOR
                Debug.Log($"[PersistenceManager] 保存成功：{fileName}");
#endif
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"[PersistenceManager] 保存失败：{fileName}\n{e.Message}");
#endif
            }
        }

        /// <summary>
        ///  从 JSON 文件加载对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="useEncryption">是否加密过</param>
        /// <returns>数据</returns>
        public T LoadFromJson<T>(string fileName, bool useEncryption = false) where T : SaveDataBase
        {
            try
            {
                string filePath = GetFilePath(fileName, FileType.JSON);

                if (!File.Exists(filePath))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"[PersistenceManager] 文件不存在：{fileName}");
#endif
                    return null;
                }

                string json = File.ReadAllText(filePath);

                if (useEncryption || enableEncryption)
                {
                    json = Decrypt(json);
                }

                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"[PersistenceManager] 加载失败：{fileName}\n{e.Message}");
#endif
                return null;
            }
        }

        /// <summary>
        /// 删除 JSON 文件
        /// </summary>
        public void DeleteJsonFile(string fileName)
        {
            try
            {
                string filePath = GetFilePath(fileName, FileType.JSON);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    UnityEngine.Debug.Log($"[PersistenceManager] 已删除：{fileName}");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[PersistenceManager] 删除失败：{fileName}\n{e.Message}");
            }
        }

        /// <summary>
        /// 检查 JSON 文件是否存在
        /// </summary>
        public bool JsonFileExists(string fileName)
        {
            string filePath = GetFilePath(fileName, FileType.JSON);
            return File.Exists(filePath);
        }

        #endregion

        #region 二进制存储

        /// <summary>
        /// 保存为二进制
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public void SaveToBinary<T>(string fileName, T data) where T : SaveDataBase
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                string filePath = GetFilePath(fileName,FileType.BIN);

                if (enableEncryption)
                {
                    bytes = EncryptBytes(bytes);
                }

                File.WriteAllBytes(filePath, bytes);
#if UNITY_EDITOR
                Debug.Log($"[PersistenceManager] 二进制保存成功：{fileName}");
#endif
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"[PersistenceManager] 二进制保存失败：{fileName}\n{e.Message}");
#endif
            }
        }

        /// <summary>
        /// 从二进制文件加载
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns>数据</returns>
        public T LoadFromBinary<T>(string fileName) where T : SaveDataBase
        {
            try
            {
                string filePath = GetFilePath(fileName, FileType.BIN);

                if (!File.Exists(filePath))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"[PersistenceManager] 二进制文件不存在：{fileName}");
#endif
                    return null;
                }

                byte[] bytes = File.ReadAllBytes(filePath);

                if (enableEncryption)
                {
                    bytes = DecryptBytes(bytes);
                }

                string json = System.Text.Encoding.UTF8.GetString(bytes);
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"[PersistenceManager] 二进制加载失败：{fileName}\n{e.Message}");
#endif
                return null;
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取文件完整路径
        /// </summary>
        private string GetFilePath(string fileName, FileType type)
        {
            // 确保文件名包含扩展名
            switch (type)
            {
                case FileType.JSON:
                    fileName = fileName.EndsWith(".json") ? fileName : fileName + ".json";
                    break;
                case FileType.BIN:
                    fileName = fileName.EndsWith(".bin") ? fileName : fileName + ".bin";
                    break;
            }
            return Path.Combine(persistencePath, fileName);
        }

        /// <summary>
        /// 简单加密（XOR）
        /// </summary>
        private string Encrypt(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            bytes = EncryptBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        private string Decrypt(string encryptedText)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);
            bytes = DecryptBytes(bytes);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 加密字节
        /// </summary>
        private byte[] EncryptBytes(byte[] bytes)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(EncryptionKey);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= keyBytes[i % keyBytes.Length];
            }
            return bytes;
        }

        /// <summary>
        /// 解密字节
        /// </summary>
        private byte[] DecryptBytes(byte[] bytes)
        {
            // XOR 加密的解密与加密相同
            return EncryptBytes(bytes);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取持久化目录路径
        /// </summary>
        /// <returns>持久化目录路径</returns>
        public string GetPersistencePath()
        {
            return persistencePath;
        }

        /// <summary>
        /// 获取文件字节大小
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>字节大小</returns>
        public long GetFileSize(string fileName)
        {
            string filePath = GetFilePath(fileName, FileType.BIN);
            if (File.Exists(filePath))
            {
                FileInfo info = new FileInfo(filePath);
                return info.Length;
            }
            return 0;
        }

        /// <summary>
        /// 获取所有保存的文件
        /// </summary>
        /// <returns>所有保存文件路径</returns>
        public string[] GetAllFiles()
        {
            if (Directory.Exists(persistencePath))
            {
                return Directory.GetFiles(persistencePath, "*.*", SearchOption.TopDirectoryOnly);
            }
            return null;
        }

        /// <summary>
        /// 清空所有持久化数据
        /// </summary>
        public void ClearAllPersistenceFiles()
        {
            try
            {
                if (Directory.Exists(persistencePath))
                {
                    Directory.Delete(persistencePath, true);
                    Directory.CreateDirectory(persistencePath);
#if UNITY_EDITOR
                    Debug.Log("[PersistenceManager] 已清空所有数据");
#endif
                }
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"[PersistenceManager] 清空数据失败：{e.Message}");
#endif
            }
        }

        #endregion

        public void AfterManagerInit() { }

        public void OnManagerDestroy() { }
    }
}
