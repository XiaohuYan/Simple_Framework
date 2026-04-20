using SimpleFramework.Common;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleFramework.ObjectPool
{
    public interface IObjectPoolManager : IManager
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        T Get<T>() where T : class, new();

        /// <summary>
        /// 归还对象
        /// </summary>
        void Return<T>(T obj) where T : class, new();

        /// <summary>
        /// 创建对象池（基础版本）
        /// </summary>
        void CreatePool<T>(int maxSize = 100) where T : class, new();

        /// <summary>
        /// 创建对象池（带回调版本）
        /// </summary>
        void CreatePoolWithCallback<T>(
            int maxSize = 100,
            UnityAction<T> onCreate = null,
            UnityAction<T> onGet = null,
            UnityAction<T> onReturn = null,
            UnityAction<T> onDestroy = null) where T : class, new();

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        bool HasPool<T>() where T : class, new();

        /// <summary>
        /// 设置对象池的创建回调
        /// </summary>
        void SetOnCreateCallback<T>(UnityAction<T> onCreate) where T : class, new();

        /// <summary>
        /// 设置对象池的获取回调
        /// </summary>
        void SetOnGetCallback<T>(UnityAction<T> onGet) where T : class, new();

        /// <summary>
        /// 设置对象池的归还回调
        /// </summary>
        void SetOnReturnCallback<T>(UnityAction<T> onReturn) where T : class, new();

        /// <summary>
        /// 设置对象池的销毁回调
        /// </summary>
        void SetOnDestroyCallback<T>(UnityAction<T> onDestroy) where T : class, new();
    }
}