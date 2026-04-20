using SimpleFramework.Common;
using UnityEngine;

namespace SimpleFramework.ObjectPool
{
    public interface IObjectPoolManager : IManager
    { /// <summary>
      /// 获取对象
      /// </summary>
        T Get<T>() where T : class, new();

        /// <summary>
        /// 归还对象
        void Return<T>(T obj) where T : class, new();

        /// <summary>
        /// 创建对象池
        /// </summary>
        void CreatePool<T>(int maxSize = 100) where T : class, new();

        /// <summary>
        /// 检查是否存在对象池
        /// </summary>
        bool HasPool<T>() where T : class, new();
    }
}