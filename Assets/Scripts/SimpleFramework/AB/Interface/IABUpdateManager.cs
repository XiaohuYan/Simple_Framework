using SimpleFramework.Common;
using UnityEngine.Events;

namespace SimpleFramework.AB
{
    public interface IABUpdateManager : IManager
    {
        /// <summary>
        /// 下载入口
        /// </summary>
        /// <param name="overCallBack">下载完成调用</param>
        void CheckUpdate(UnityAction<bool> overCallBack);
    }
}