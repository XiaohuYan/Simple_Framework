using System;

namespace SimpleFramework.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        /// <summary>
        /// 数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 类型
        /// </summary>
        public abstract Type ElementType { get; }

        /// <summary>
        /// 清空
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 获取对象（非泛型，内部使用）
        /// </summary>
        protected abstract object GetObjInternal();

        /// <summary>
        /// 归还对象（非泛型，内部使用）
        /// </summary>
        protected abstract void ReturnObjInternal(object obj);
    }
}