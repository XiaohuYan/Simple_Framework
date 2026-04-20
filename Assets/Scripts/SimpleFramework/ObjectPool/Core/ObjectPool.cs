using System;
using System.Collections.Generic;

namespace SimpleFramework.ObjectPool
{
    public class ObjectPool<T> : ObjectPoolBase where T : class, new()
    {
        private readonly Queue<T> pool = new Queue<T>();
        private readonly int maxSize;

        public ObjectPool(int maxSize = 100)
        {
            this.maxSize = maxSize;
        }

        public override int Count => pool.Count;

        public override Type ElementType => typeof(T);

        public override void Clear()
        {
            pool.Clear();
        }

        /// <summary>
        /// 获取对象（类型安全）
        /// </summary>
        public T Get()
        {
            if (pool.Count > 0)
                return pool.Dequeue();
            return new T();
        }

        /// <summary>
        /// 归还对象（类型安全）
        /// </summary>
        public void Return(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (pool.Count < maxSize)
                pool.Enqueue(obj);
            // 如果超过最大容量，则丢弃对象，让GC回收
        }

        protected override object GetObjInternal()
        {
            return Get();
        }

        protected override void ReturnObjInternal(object obj)
        {
            if (obj is T tObj)
                Return(tObj);
            else
                throw new ArgumentException($"对象类型必须是 {typeof(T)}，实际是 {obj?.GetType()}");
        }
    }
}