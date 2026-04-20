using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    /// <summary>
    /// 黑板类
    /// </summary>
    [Serializable]
    public class Blackboard
    {
        /// <summary>
        /// 存储黑板键和对应的值
        /// </summary>
        Dictionary<BlackboardKey, object> entries = new Dictionary<BlackboardKey, object>();

        /// <summary>
        /// 用于快速查找字符串对应的黑板键是否存在
        /// </summary>
        Dictionary<string, BlackboardKey> KeyRegistry = new Dictionary<string, BlackboardKey>();

        #region 专家相干的操作列表

        public List<Action> PassedActions { get; } = new List<Action>();

        public void AddAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException($"添加空方法");
            }
            PassedActions.Add(action);
        }

        public void ClearActions() => PassedActions.Clear();

        #endregion

        /// <summary>
        /// 通过黑板键尝试从黑板键值对里取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">黑板键</param>
        /// <param name="value">取到的值</param>
        /// <returns>黑板键对应的值</returns>
        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            if (entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// 查看所有的黑板键值对
        /// </summary>
        //public void Debug()
        //{
        //    foreach (var entry in entries)
        //    {
        //        var entryType = entry.Value.GetType();
        //        if (entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
        //        {
        //            var valueProperty = entryType.GetProperty("Value");
        //            if (valueProperty != null)
        //            {
        //                var value = valueProperty.GetValue(entry.Value);
        //                UnityEngine.Debug.Log($"Key:{entry.Key},Value:{value}");
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 设置黑板键值对
        /// </summary>
        /// <typeparam name="T">黑板键对应值类型</typeparam>
        /// <param name="key">黑板键</param>
        /// <param name="Value">黑板键对应的值</param>
        public void SetValue<T>(BlackboardKey key,T Value)
        {
            entries[key] = new BlackboardEntry<T>(key, Value);
        }

        /// <summary>
        /// 用于快速创建黑板键
        /// </summary>
        /// <param name="type">黑板值类型</param>
        /// <param name="keyName">黑板键名</param>
        /// <returns>创建好的黑板键</returns>
        /// <exception cref="ArgumentException">黑板键名空错误</exception>
        public BlackboardKey GetOrRegisterKey(Type type, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("参数不能为 null 或空字符串", nameof(keyName));
            }

            if (!KeyRegistry.TryGetValue(keyName, out var key))
            {
                key = new BlackboardKey(type, keyName);
                KeyRegistry[keyName] = key;
            }

            return key;
        }

        /// <summary>
        /// 用于快速创建黑板键
        /// </summary>
        /// <typeparam name="T">黑板键名</typeparam>
        /// <param name="keyName">黑板值类型</param>
        /// <returns>创建好的黑板键</returns>
        /// <exception cref="ArgumentException">黑板键名空错误</exception>
        public BlackboardKey GetOrRegisterKey<T>(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("参数不能为 null 或空字符串", nameof(keyName));
            }
            if (!KeyRegistry.TryGetValue(keyName, out var key))
            {
                key = new BlackboardKey(typeof(T), keyName);
                KeyRegistry[keyName] = key;
            }
            return key;
        }

        /// <summary>
        /// 判断是否包含黑板键
        /// </summary>
        /// <param name="key">黑板键</param>
        /// <returns>是否包含布尔变量</returns>
        public bool ContainsKey(BlackboardKey key)
        {
            return entries.ContainsKey(key);
        }

        /// <summary>
        /// 移除黑板键
        /// </summary>
        /// <param name="key">黑板键</param>
        public void Remove(BlackboardKey key)
        {
            entries.Remove(key);
        }
    }
}