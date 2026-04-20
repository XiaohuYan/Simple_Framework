using SimpleFramework.Common;
using UnityEngine.Events;

namespace SimpleFramework.ActionMono
{
    public interface IActionMonoManager
    {
        void AddUpdate(UnityAction<float> action);

        void RemoveUpdate(UnityAction<float> action);

        void AddFixedUpdate(UnityAction<float> action);

        void RemoveFixedUpdate(UnityAction<float> action);

        void AddLateUpdate(UnityAction<float> action);

        void RemoveLateUpdate(UnityAction<float> action);
    }
}