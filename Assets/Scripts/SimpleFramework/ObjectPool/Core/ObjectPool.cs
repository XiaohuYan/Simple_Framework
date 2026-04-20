using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SimpleFramework.ObjectPool
{
    public class ObjectPool<T> : ObjectPoolBase where T : class, new()
    {
        private readonly Queue<T> pool = new Queue<T>();
        private readonly int maxSize;

        // 回调事件
        private UnityAction<T> onCreate;
        private UnityAction<T> onGet;
        private UnityAction<T> onReturn;
        private UnityAction<T> onDestroy;

        public ObjectPool(int maxSize = 100)
        {
            this.maxSize = maxSize;
        }

        public ObjectPool(int maxSize = 100,
            UnityAction<T> onCreate = null,
            UnityAction<T> onGet = null,
            UnityAction<T> onReturn = null,
            UnityAction<T> onDestroy = null)
        {
            this.maxSize = maxSize;
            this.onCreate = onCreate;
            this.onGet = onGet;
            this.onReturn = onReturn;
            this.onDestroy = onDestroy;
        }

        public override int Count => pool.Count;

        public override Type ElementType => typeof(T);

        public override void Clear()
        {
            if (onDestroy != null)
            {
                foreach (var obj in pool)
                {
                    onDestroy(obj);
                }
            }
            pool.Clear();
        }

        /// <summary>
        /// 获取对象（类型安全）
        /// </summary>
        public T Get()
        {
            T obj;
            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
            }
            else
            {
                // TODO 创建
                obj = default(T);
                onCreate?.Invoke(obj);
            }

            onGet?.Invoke(obj);
            return obj;
        }

        /// <summary>
        /// 归还对象（类型安全）
        /// </summary>
        public void Return(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (pool.Count < maxSize)
            {
                onReturn?.Invoke(obj);
                pool.Enqueue(obj);
            }
            else
            {
                onDestroy?.Invoke(obj);
            }
        }

        /// <summary>
        /// 设置创建回调
        /// </summary>
        public void SetOnCreateCallback(UnityAction<T> callback)
        {
            onCreate = callback;
        }

        /// <summary>
        /// 设置获取回调
        /// </summary>
        public void SetOnGetCallback(UnityAction<T> callback)
        {
            onGet = callback;
        }

        /// <summary>
        /// 设置归还回调
        /// </summary>
        public void SetOnReturnCallback(UnityAction<T> callback)
        {
            onReturn = callback;
        }

        /// <summary>
        /// 设置销毁回调
        /// </summary>
        public void SetOnDestroyCallback(UnityAction<T> callback)
        {
            onDestroy = callback;
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