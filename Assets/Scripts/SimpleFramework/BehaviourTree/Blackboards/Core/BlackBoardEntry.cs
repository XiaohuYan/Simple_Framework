using System;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    /// <summary>
    /// 黑板字典值包装类（定义了黑板值的类型，类型安全）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    [Serializable]
    public class BlackboardEntry<T>
    {
        public T Value { get; }
        public BlackboardKey Key { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj)
        {
            return obj is BlackboardEntry<T> other && other.Key.typeNameKey == Key.typeNameKey;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}