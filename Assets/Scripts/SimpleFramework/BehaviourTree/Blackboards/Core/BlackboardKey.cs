using SimpleFramework.Extension;
using System;
using SimpleFramework.Common.TypeNameKey;

namespace SimpleFramework.BehaviourTree.BlackboardSystem
{
    /// <summary>
    /// 类型安全的黑板键，防止任何 string 都能传入
    /// </summary>
    [Serializable]
    public readonly struct BlackboardKey 
    {
        public readonly TypeNameKey typeNameKey;

        public BlackboardKey(Type type)
        {
            typeNameKey = new TypeNameKey(type);
        }

        public BlackboardKey(Type type,string name)
        {
            typeNameKey = new TypeNameKey(type, name);
        }
    }
}