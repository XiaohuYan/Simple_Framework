using SimpleFramework.Extension;
using System;

namespace SimpleFramework.Common.TypeNameKey
{
    public readonly struct TypeNameKey : IEquatable<TypeNameKey>
    {
        /// <summary>
        /// 名字
        /// </summary>
        private readonly string name;

        /// <summary>
        /// 对应的哈希值，缓存后不用每次都重新计算效率更高
        /// </summary>
        private readonly int hashKey;

        /// <summary>
        /// 类型
        /// </summary>
        private readonly Type type;

        public TypeNameKey(Type type) : this(type, string.Empty)
        {

        }

        public TypeNameKey(Type type, string name)
        {
            this.name = name;
            this.type = type;
            hashKey = name.ComputeFNV1aHash() ^ type.GetHashCode();
        }

        public bool Equals(TypeNameKey other)
        {
            // 先比较哈希（快速路径）
            if (hashKey != other.hashKey)
                return false;
            // 哈希相等时，比较原始字符串
            return (type == other.type && name == other.name);
        }

        public override bool Equals(object obj)
        {
            return obj is TypeNameKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return hashKey;
        }

        public override string ToString()
        {
            return name;
        }

        public static bool operator ==(TypeNameKey left, TypeNameKey right)
        {
            if (left.hashKey != right.hashKey)
            {
                return false;
            }

            // 哈希相等时，必须比较原始字符串
            return left.name == right.name;
        }

        public static bool operator !=(TypeNameKey left, TypeNameKey right)
        {
            return !(left == right);
        }
    }
}