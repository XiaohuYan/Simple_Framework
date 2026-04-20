using UnityEngine;

namespace SimpleFramework.Common
{
    public abstract class BaseMonoManager : MonoBehaviour, IManager
    {
        public virtual void OnManagerInit()
        {
            // ·ÀÖ¹ÇÐ»»³¡¾°ºóÏú»Ù
            DontDestroyOnLoad(gameObject);
        }

        public virtual void OnManagerDestroy()
        {
            Destroy(gameObject);
        }

        public abstract void AfterManagerInit();
    }
}