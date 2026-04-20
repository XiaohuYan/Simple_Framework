using SimpleFramework.Common;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleFramework.ActionMono
{
    public class ActionMonoManager : BaseMonoManager, IActionMonoManager
    {

        private UnityAction<float> updateAction;
        private UnityAction<float> fixedUpdateAction;
        private UnityAction<float> lateUpdateAction;
        /// <summary>
        /// 添加帧更新事件
        /// </summary>
        public void AddUpdate(UnityAction<float> action)
        {
            updateAction += action;
        }

        /// <summary>
        /// 移除帧更新事件
        /// </summary>
        public void RemoveUpdate(UnityAction<float> action)
        {
            updateAction -= action;
        }

        public void AddFixedUpdate(UnityAction<float> action)
        {
            fixedUpdateAction += action;
        }

        public void RemoveFixedUpdate(UnityAction<float> action)
        {
            fixedUpdateAction -= action;
        }

        public void AddLateUpdate(UnityAction<float> action)
        {
            lateUpdateAction += action;
        }

        public void RemoveLateUpdate(UnityAction<float> action)
        {
            lateUpdateAction -= action;
        }

        private void Update()
        {
            updateAction?.Invoke(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            fixedUpdateAction?.Invoke(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            lateUpdateAction?.Invoke(Time.deltaTime);
        }

        public override void AfterManagerInit()
        {

        }

        public override void OnManagerDestroy()
        {
            updateAction = null;
            fixedUpdateAction = null;
            lateUpdateAction = null;
            base.OnManagerDestroy();
        }
    }
}