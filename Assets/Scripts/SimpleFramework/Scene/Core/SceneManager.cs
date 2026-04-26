using System.Threading.Tasks;
using UnityEngine.Events;

namespace SimpleFramework.Scene
{
    public class SceneManager : ISceneManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="name">场景名字</param>
        /// <param name="action">完成加载后的回调</param>
        public void LoadScene(string name, UnityAction callback)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
            callback?.Invoke();
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="action">加载完成后需要调用的方法</param>
        /// <param name="OnProgressUpdate">进度条更新方法</param>
        public async Task LoadSceneAsync(string name, UnityAction callback, UnityAction<float> OnProgressUpdateCallback)
        {
            var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

            // 禁止场景加载完成后自动激活
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                OnProgressUpdateCallback?.Invoke(asyncLoad.progress);
                await Task.Yield();
            }
            OnProgressUpdateCallback?.Invoke(1f);
            asyncLoad.allowSceneActivation = true;
            callback?.Invoke();
        }

        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {

        }

        public void OnManagerDestroy()
        {

        }
    }
}