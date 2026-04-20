using SimpleFramework.Common;
using UnityEngine.Events;

namespace SimpleFramework.Scene
{
    public interface ISceneManager : IManager
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="name">场景名字</param>
        /// <param name="action">完成加载后的回调</param>
        void LoadScene(string name, UnityAction callback);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="action">加载完成后需要调用的方法</param>
        /// <param name="OnProgressUpdate">进度条更新方法</param>
        void LoadSceneAsync(string name, UnityAction callback, UnityAction<float> OnProgressUpdateCallback);
    }
}