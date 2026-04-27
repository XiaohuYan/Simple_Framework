using SimpleFramework.Common;

namespace SimpleFramework.ObjectPool
{
    public interface IObjectPoolManager : IManager
    {
        IObjectPool<T> CreatePool<T>(string name, int capacity, float existTime, float autoReleaseIntervel) where T : ObjectBase;

        void ClearAllPools();
    }
}