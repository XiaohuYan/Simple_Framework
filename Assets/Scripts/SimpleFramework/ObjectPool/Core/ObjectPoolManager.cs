using System;
using System.Collections.Generic;

namespace SimpleFramework.ObjectPool
{
    public class ObjectPoolManager : IObjectPoolManager
    {
        private readonly Dictionary<ObjectPoolKey, ObjectPoolBase> objectPoolDic = new Dictionary<ObjectPoolKey, ObjectPoolBase>();

        /// <summary>
        /// 创建对象池
        /// </summary>
        public void CreatePool<T>(int maxSize = 100) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (!objectPoolDic.ContainsKey(key))
            {
                objectPoolDic.Add(key, new ObjectPool<T>(maxSize));
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public T Get<T>() where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));

            if (!objectPoolDic.TryGetValue(key, out var poolBase))
            {
                // 自动创建池子
                CreatePool<T>();
                poolBase = objectPoolDic[key];
            }

            // 类型安全的转换
            if (poolBase is ObjectPool<T> typedPool)
            {
                return typedPool.Get();
            }

            throw new InvalidOperationException($"对象池类型错误：期望 {typeof(T)}，实际 {poolBase.ElementType}");
        }

        /// <summary>
        /// 归还对象
        /// </summary>
        public void Return<T>(T obj) where T : class, new()
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var key = new ObjectPoolKey(typeof(T));

            if (objectPoolDic.TryGetValue(key, out var poolBase))
            {
                if (poolBase is ObjectPool<T> typedPool)
                {
                    typedPool.Return(obj);
                }
                else
                {
                    throw new InvalidOperationException($"对象池类型错误：期望 {typeof(T)}，实际 {poolBase.ElementType}");
                }
            }
            else
            {
                // 没有对应的池子，对象被丢弃
                UnityEngine.Debug.LogWarning($"没有找到类型 {typeof(T)} 的对象池，对象将被丢弃");
            }
        }

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        public bool HasPool<T>() where T : class, new()
        {
            return objectPoolDic.ContainsKey(new ObjectPoolKey(typeof(T)));
        }

        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {

        }

        public void OnManagerDestroy()
        {
            foreach (var value in objectPoolDic.Values)
            {
                value.Clear();
            }
            objectPoolDic.Clear();
        }

    }
}