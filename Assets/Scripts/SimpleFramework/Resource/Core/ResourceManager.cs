using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleFramework.Resource
{
    public class ResourceManager : IResourceManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 同步加载Resource资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源</returns>
        public T LoadResource<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="callback">加载完成后处理资源的回调</param>
        public async Task LoadResourceAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            callback(request.asset as T);
        }

        #region IManager 接口

        public void OnManagerInit() { }

        public void AfterManagerInit() { }

        public void OnManagerDestroy() { }

        #endregion

    }
}