using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SimpleFramework.ObjectPool
{
    public class ObjectPoolManager : IObjectPoolManager
    {
        private readonly Dictionary<ObjectPoolKey, ObjectPoolBase> objectPoolDic = new Dictionary<ObjectPoolKey, ObjectPoolBase>();

        /// <summary>
        /// 创建对象池（基础版本）
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
        /// 创建对象池（带回调版本）
        /// </summary>
        public void CreatePoolWithCallback<T>(
            int maxSize = 100,
            UnityAction<T> onCreate = null,
            UnityAction<T> onGet = null,
            UnityAction<T> onReturn = null,
            UnityAction<T> onDestroy = null) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (!objectPoolDic.ContainsKey(key))
            {
                objectPoolDic.Add(key, new ObjectPool<T>(maxSize, onCreate, onGet, onReturn, onDestroy));
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

        /// <summary>
        /// 设置对象池的创建回调
        /// </summary>
        public void SetOnCreateCallback<T>(UnityAction<T> onCreate) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (objectPoolDic.TryGetValue(key, out var poolBase) && poolBase is ObjectPool<T> typedPool)
            {
                typedPool.SetOnCreateCallback(onCreate);
            }
        }

        /// <summary>
        /// 设置对象池的获取回调
        /// </summary>
        public void SetOnGetCallback<T>(UnityAction<T> onGet) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (objectPoolDic.TryGetValue(key, out var poolBase) && poolBase is ObjectPool<T> typedPool)
            {
                typedPool.SetOnGetCallback(onGet);
            }
        }

        /// <summary>
        /// 设置对象池的归还回调
        /// </summary>
        public void SetOnReturnCallback<T>(UnityAction<T> onReturn) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (objectPoolDic.TryGetValue(key, out var poolBase) && poolBase is ObjectPool<T> typedPool)
            {
                typedPool.SetOnReturnCallback(onReturn);
            }
        }

        /// <summary>
        /// 设置对象池的销毁回调
        /// </summary>
        public void SetOnDestroyCallback<T>(UnityAction<T> onDestroy) where T : class, new()
        {
            var key = new ObjectPoolKey(typeof(T));
            if (objectPoolDic.TryGetValue(key, out var poolBase) && poolBase is ObjectPool<T> typedPool)
            {
                typedPool.SetOnDestroyCallback(onDestroy);
            }
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