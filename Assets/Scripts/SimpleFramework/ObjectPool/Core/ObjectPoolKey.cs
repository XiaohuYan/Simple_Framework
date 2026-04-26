using SimpleFramework.Common.TypeNameKey;
using System;

namespace SimpleFramework.ObjectPool
{
    public readonly struct ObjectPoolKey
    {
        public readonly TypeNameKey typeNameKey;

        public ObjectPoolKey(Type type)
        {
            typeNameKey = new TypeNameKey(type);
        }

        public ObjectPoolKey(Type type, string name)
        {
            typeNameKey = new TypeNameKey(type, name);
        }
    }
}