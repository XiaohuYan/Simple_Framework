using SimpleFramework.Common;
using UnityEngine.Events;

namespace SimpleFramework.Resource
{
    public interface IResourceManager : IManager
    {
        /// <summary>
        /// 同步加载Resource资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源</returns>
        T LoadResource<T>(string path) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="callback">加载完成后处理资源的回调</param>
        void LoadResourceAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object;
    }
}