using SimpleFramework.ActionMono;
using SimpleFramework.Entry;
using System.Collections.Generic;


namespace SimpleFramework.ObjectPool
{
    /// <summary>
    /// 对象池管理类
    /// </summary>
    public partial class ObjectPoolManager : IObjectPoolManager
    {
        /// <summary> 所有的对象池 </summary>
        private readonly Dictionary<ObjectPoolKey, ObjectPoolBase> objectPools = new();

        /// <summary> 释放优先级 </summary>
        private int priority = 0;

        public void Update(float deltaTime)
        {
            foreach (var pool in objectPools.Values)
            {
                pool.Update(deltaTime);
            }
        }

        /// <summary>
        /// 获取释放优先级
        /// </summary>
        public int Priority => priority;

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <typeparam name="T">对象池对象类型</typeparam>
        /// <param name="name">对象池名字</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="existTime">对象池里的对象不使用时最大存在时间</param>
        /// <param name="autoReleaseIntervel">对象池自动释放时间</param>
        /// <returns>创建好的对象池</returns>
        /// <exception cref="System.ArgumentException">创建已经存在的对象池错误</exception>
        public IObjectPool<T> CreatePool<T>(string name, int capacity, float existTime, float autoReleaseIntervel) where T : ObjectBase
        {
            ObjectPoolKey key = new ObjectPoolKey(typeof(T), name);
            if (HasObjectPool<T>(key))
            {
                throw new System.ArgumentException($"{key.ToString()}对象池已经存在");
            }
            ObjectPool<T> objectPool = new ObjectPool<T>(name, capacity, existTime, autoReleaseIntervel);
            objectPools.Add(key, objectPool);
            return objectPool;
        }

        /// <summary>
        /// 是否含有对象池
        /// </summary>
        /// <typeparam name="T">对象池里的对象类型</typeparam>
        /// <param name="name">对象池名字</param>
        /// <returns>是否含有对象池</returns>
        private bool HasObjectPool<T>(ObjectPoolKey key)
        {
            return objectPools.ContainsKey(key);
        }

        /// <summary>
        /// 卸载所有对象池
        /// </summary>
        public void ClearAllPools()
        {
            foreach(var pool in objectPools.Values)
            {
                pool.ReleaseAll();
            }
            objectPools.Clear();
        }

        public void AfterManagerInit()
        {
            GameFacade.Instance.GetManager<ActionMonoManager>().AddUpdate(Update);
        }

        public void OnManagerDestroy()
        {
            GameFacade.Instance.GetManager<ActionMonoManager>().RemoveUpdate(Update);
            ClearAllPools();
        }

        public void OnManagerInit() { }
    }
}