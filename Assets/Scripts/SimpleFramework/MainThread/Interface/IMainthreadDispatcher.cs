using SimpleFramework.Common;
using UnityEngine.Events;

namespace SimpleFramework.MainThread
{
    public interface IMainthreadDispatcher : IManager
    {
        /// <summary>
        /// 加入主线程队列
        /// </summary>
        /// <param name="action">需要在主线程调用的方法</param>
        void EnqueueMainThread(UnityAction<float> action);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="deltaTime">每帧时间</param>
        void Execute(float deltaTime);
    }
}