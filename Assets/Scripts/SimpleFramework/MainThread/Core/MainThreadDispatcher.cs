using System.Collections.Concurrent;
using UnityEngine.Events;
using SimpleFramework.Entry;
using SimpleFramework.ActionMono;

namespace SimpleFramework.MainThread
{
    public class MainThreadDispatcher : IMainthreadDispatcher
    {
        /// <summary>
        /// 在主线程调用的队列
        /// </summary>
        private readonly ConcurrentQueue<UnityAction<float>> executionQueue = new ConcurrentQueue<UnityAction<float>>();

        /// <summary>
        /// 加入主线程队列
        /// </summary>
        /// <param name="action">需要在主线程调用的方法</param>
        public void EnqueueMainThread(UnityAction<float> action)
        {
            executionQueue.Enqueue(action);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="deltaTime">每帧时间</param>
        public void Execute(float deltaTime)
        {
            if (executionQueue.Count > 0)
            {
                executionQueue.TryDequeue(out var action);
                action?.Invoke(deltaTime);
            }
        }

        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {
            // mono 特殊处理过,所以此处 GetManager 无法调用接口形式
            GameFacade.Instance.GetManager<ActionMonoManager>().AddUpdate(Execute);
        }


        public void OnManagerDestroy()
        {
            GameFacade.Instance.GetManager<ActionMonoManager>().RemoveUpdate(Execute);
        }

    }
}