namespace SimpleFramework.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        /// <summary> 对象池名字  </summary>
        private string name;

        public ObjectPoolBase(string name)
        {
            this.name = name;
        }

        public string Name => name;

        /// <summary>
        /// 用于判断是否需要释放对象
        /// </summary>
        /// <param name="deltaTime">帧时间</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// 释放对象池中的对象
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="releaseCount">释放的数量</param>
        public abstract void Release(int releaseCount);

        /// <summary>
        /// 清空对象池
        /// </summary>
        public abstract void Clear();
    }
}