namespace SimpleFramework.ObjectPool
{
    public interface IObjectPool<T> where T:ObjectBase
    {
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>是否释放成功</returns>
        bool ReleaseObject(T obj);

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="realObject">实例化后的对象</param>
        /// <returns>是否释放成功</returns>
        bool ReleaseObject(object realObject);

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="obj">对象</param>
        void Register(T obj);

        /// <summary>
        /// 从对象池中取对象
        /// </summary>
        /// <returns>对象</returns>
        T Get();

        /// <summary>
        /// 从对象池中取对象
        /// </summary>
        /// <param name="name">对象名字</param>
        /// <returns>对象</returns>
        T Get(string name);

        /// <summary>
        /// 对象放回对象池
        /// </summary>
        /// <param name="obj">对象</param>
        void Return(T obj);
    }
}