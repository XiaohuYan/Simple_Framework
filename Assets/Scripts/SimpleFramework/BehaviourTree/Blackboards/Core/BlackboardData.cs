using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    [CreateAssetMenu(fileName = "New Blackboard Data", menuName = "Blackboard/Blackboard Data")]
    public class BlackboardData : ScriptableObject
    {
        /// <summary>
        /// 存储所有的黑板键值对列表（unity无法序列化字典）
        /// </summary>
        public List<BlackboardEntryData> entries = new List<BlackboardEntryData>();

        /// <summary>
        /// 存储所有用户输入的键值对到黑板（用于在外部调用初始化黑板键值对）
        /// </summary>
        /// <param name="blackboard">黑板</param>
        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }
    }

    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver
    {
        public string keyName;

        public AnyValue.ValueType valueType;

        public AnyValue value;

        /// <summary>
        /// 设置键值对入口
        /// </summary>
        /// <param name="blackboard">黑板</param>
        public void SetValueOnBlackboard(Blackboard blackboard)
        {
            BlackboardKey key;
            switch (valueType)
            {
                case AnyValue.ValueType.Bool:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<bool>(keyName);
                    break;
                case AnyValue.ValueType.Float:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<float>(keyName);
                    break;
                case AnyValue.ValueType.Vector3:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<Vector3>(keyName);
                    break;
                case AnyValue.ValueType.String:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<string>(keyName);
                    break;
                case AnyValue.ValueType.Int:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<int>(keyName);
                    break;
                default:
                    // 注册键
                    key = blackboard.GetOrRegisterKey<object>(keyName);
                    break;
            }
            // 注册键值对
            setValueDispatchTable[value.type](blackboard, key, value);
        }

        /// <summary>
        /// 根据不用的类型获取到处理不同类型的回调
        /// </summary>
        static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> setValueDispatchTable = new()
        {
           { AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetValue<int>(key, anyValue) },
            { AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetValue<float>(key, anyValue) },
            { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue<bool>(key, anyValue) },
            { AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetValue<string>(key, anyValue) },
            { AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetValue<Vector3>(key, anyValue) },
        };

        /// <summary>
        /// 序列化后调用
        /// </summary>
        public void OnAfterDeserialize()
        {
            value.type = valueType;
        }

        /// <summary>
        /// 序列化前调用
        /// </summary>
        public void OnBeforeSerialize() { }

    }

    /// <summary>
    /// 存储黑板键值，使用 struct 存储所有黑板值类型，转换防止装箱操作
    /// </summary>
    [Serializable]
    public struct AnyValue
    {
        public enum ValueType : byte { Int, Float, Bool, String, Vector3 }
        public ValueType type;

        #region 不同类型的值的存储空间

        public int intValue;
        public float floatValue;
        public bool boolValue;
        public string stringValue;
        public Vector3 vector3Value;

        #endregion

        #region 不同类型的隐式转换

        /// <summary>
        /// 允许 AnyValue 直接当作 bool 使用
        /// </summary>
        /// <param name="value">值</param>
        public static implicit operator bool(AnyValue value)
        {
            return value.ConvertValue<bool>();
        }

        public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
        public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();
        public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
        public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
        #endregion

        #region 隐式转换逻辑

        T ConvertValue<T>()
        {
            return type switch
            {
                ValueType.Int => AsInt<T>(intValue),
                ValueType.Float => AsFloat<T>(floatValue),
                ValueType.Bool => AsBool<T>(boolValue),
                ValueType.String => (T)(object)stringValue,
                ValueType.Vector3 => AsVector3<T>(vector3Value),
                _ => throw new NotSupportedException($"不支持值类型：{typeof(T)}")
            };
        }

        #endregion

        #region 辅助隐式转换逻辑（用于在不进行装箱操作的情况下对值类型进行安全类型转换的辅助方法）
        // 已测试 value is T ，值类型时未装箱
        T AsBool<T>(bool value)
        {
            return (typeof(T) == typeof(bool) && value is T correctType) ? correctType : default;
        }
        T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
        T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        T AsVector3<T>(Vector3 value) => typeof(T) == typeof(Vector3) && value is T correctType ? correctType : default;
        #endregion
    }

}