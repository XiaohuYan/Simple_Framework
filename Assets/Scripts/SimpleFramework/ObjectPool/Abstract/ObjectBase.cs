using System;

namespace SimpleFramework.ObjectPool
{
    public abstract class ObjectBase
    {
        /// <summary> 上次使用时间 </summary>
        private DateTime lastUseTime;

        /// <summary> 存储的实际对象(在 unity 中对应实例化后的对象) </summary>
        private object realObject;

        /// <summary> 名字 </summary>
        private string name;

        /// <summary>
        /// 获取对象名称。
        /// </summary>
        public string Name => name;

        /// <summary>
        /// 上次使用时间
        /// </summary>
        public DateTime LasetUseTime => lastUseTime;

        /// <summary>
        /// 实例化后的实际对象
        /// </summary>
        public object RealObject => realObject;

        public ObjectBase(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="realObject">关联的对象</param>
        protected void Initial(object realObject)
        {
            Initial(realObject, null);
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="realObject">关联的对象</param>
        /// <param name="name">对象名</param>
        protected void Initial(object realObject, string name)
        {
            this.realObject = realObject;
            this.name = name;
            lastUseTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 清空
        /// </summary>
        public virtual void Clear()
        {
            realObject = null;
            name = null;
            lastUseTime = default;
        }
    }
}