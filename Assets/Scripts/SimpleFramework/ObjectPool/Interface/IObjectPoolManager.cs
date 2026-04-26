using SimpleFramework.Common;

namespace SimpleFramework.ObjectPool
{
    public interface IObjectPoolManager : IManager
    {
        ObjectPool<T> CreatePool<T>(string name, int capacity, float existTime, float autoReleaseIntervel) where T : ObjectBase;

        bool HasObjectPool<T>(ObjectPoolKey key);

        void ClearAllPools();
    }
}