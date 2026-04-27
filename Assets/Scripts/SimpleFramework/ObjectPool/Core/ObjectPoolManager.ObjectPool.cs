using System.Collections.Generic;

namespace SimpleFramework.ObjectPool
{
    public partial class ObjectPoolManager : IObjectPoolManager
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            /// <summary> 自动释放的时间 </summary>
            private float autoReleaseIntervel;

            /// <summary> 自动释放计时器 </summary>
            private float autoRealseTimer;

            /// <summary> 对象池容量 </summary>
            private int capacity;

            /// <summary> 不使用时对象最大存在时间  </summary>
            private float expireTime;

            /// <summary> 对象名字和对象队列对应的字典 </summary>
            private readonly Dictionary<string, Queue<T>> objectPoolDic = new();

            /// <summary> 实例化的对象和对象对应的字典 </summary>
            private readonly Dictionary<object, T> objectMap = new();

            /// <summary>
            /// 获取对象池容量
            /// </summary>
            public int Capacity => capacity;

            /// <summary>
            /// 获取对象池中对象数量
            /// </summary>
            public int Count => objectMap.Count;

            public ObjectPool(string name, int capacity, float autoReleaseIntervel, float expireTime) : base(name)
            {
                this.capacity = capacity;
                this.autoReleaseIntervel = autoReleaseIntervel;
                this.expireTime = expireTime;
                autoRealseTimer = 0;
            }

            /// <summary>
            /// 用于判断是否需要释放对象
            /// </summary>
            /// <param name="deltaTime">帧时间</param>
            public override void Update(float deltaTime)
            {
                autoRealseTimer += deltaTime;
                if (autoRealseTimer > autoReleaseIntervel)
                {
                    Release();
                    autoRealseTimer = 0;
                }
            }

            /// <summary>
            /// 释放对象池中的对象
            /// </summary>
            public override void Release()
            {
                Release(Count - Capacity);
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="releaseCount">释放的数量</param>
            public override void Release(int releaseCount)
            {
                if (releaseCount <= 0)
                {
                    // 超过最大持续时间
                    foreach (var items in objectPoolDic.Values)
                    {
                        foreach (var item in items)
                        {
                            if ((System.DateTime.UtcNow - item.LasetUseTime).TotalSeconds > expireTime)
                            {
                                ReleaseObject(item.RealObject);
                            }
                        }
                    }
                }
                else
                {
                    // 按照时间使用顺序进行排序释放
                    List<ObjectBase> objectBases = new List<ObjectBase>(Count);
                    foreach (var items in objectPoolDic.Values)
                    {
                        foreach (var item in items)
                        {
                            objectBases.Add(item);
                        }
                    }
                    objectBases.Sort((a, b) => { return a.LasetUseTime.CompareTo(b.LasetUseTime); });
                    for (int i = 0; i < releaseCount; i++)
                    {
                        ReleaseObject(objectBases[i]);
                    }
                }
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <returns>是否释放成功</returns>
            /// <exception cref="System.ArgumentNullException">释放空对象错误</exception>
            public bool ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new System.ArgumentNullException("释放的对象为空");
                }
                return ReleaseObject(obj.RealObject);
            }

            /// <summary>
            /// 释放对象
            /// </summary>
            /// <param name="realObject">实例化后的对象</param>
            /// <returns>是否释放成功</returns>
            /// <exception cref="System.ArgumentNullException">释放空对象错误</exception>
            public bool ReleaseObject(object realObject)
            {
                if (realObject == null)
                {
                    throw new System.ArgumentNullException("释放的对象为空");
                }

                T obj = GetObject(realObject);
                if (obj == null)
                {
                    return false;
                }

                objectPoolDic.Remove(obj.Name);
                objectMap.Remove(realObject);
                obj.Release();
                return true;
            }

            /// <summary>
            /// 从对象表中获取对象
            /// </summary>
            /// <param name="realObject">实例化后的对象</param>
            /// <returns>是否找到对象</returns>
            /// <exception cref="System.ArgumentNullException">空对象错误</exception>
            private T GetObject(object realObject)
            {
                if (realObject == null)
                {
                    throw new System.ArgumentNullException("对象为空");
                }
                objectMap.TryGetValue(realObject, out var obj);
                return obj;
            }

            /// <summary>
            /// 创建对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <exception cref="System.ArgumentNullException">对象空错误</exception>
            public void Register(T obj)
            {
                if (obj == null)
                {
                    throw new System.ArgumentNullException("对象为空");
                }
                if (!objectPoolDic.ContainsKey(obj.Name))
                {
                    objectPoolDic.Add(obj.Name, new Queue<T>());
                }
                objectPoolDic[obj.Name].Enqueue(obj);
                objectMap.Add(obj.RealObject, obj);
            }

            /// <summary>
            /// 从对象池中取对象
            /// </summary>
            /// <returns>对象</returns>
            public T Get()
            {
                return Get(string.Empty);
            }

            /// <summary>
            /// 从对象池中取对象
            /// </summary>
            /// <param name="name">对象名字</param>
            /// <returns>对象</returns>
            public T Get(string name)
            {
                if (objectPoolDic.TryGetValue(name, out var value))
                {
                    return value.Dequeue();
                }
                return null;
            }

            /// <summary>
            /// 对象放回对象池
            /// </summary>
            /// <param name="obj">对象</param>
            /// <exception cref="System.ArgumentException">空对象错误</exception>
            public void Return(T obj)
            {
                if (obj == null)
                {
                    throw new System.ArgumentException("对象为空,无法回收");
                }
                if (objectPoolDic.ContainsKey(obj.Name) && objectPoolDic[obj.Name] != null)
                {
                    objectPoolDic[obj.Name].Enqueue(obj);
                }
                else
                {
                    throw new System.ArgumentException($"找不到{obj.Name}对应的对象池");
                }
            }

            /// <summary>
            /// 释放所有对象
            /// </summary>
            public override void ReleaseAll()
            {
                foreach (var items in objectPoolDic.Values)
                {
                    foreach (var item in items)
                    {
                        item.Release();
                    }
                }
                objectPoolDic.Clear();
                objectMap.Clear();
            }
        }
    } 
}