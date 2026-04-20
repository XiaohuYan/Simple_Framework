using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleFramework.Common;
using UnityEngine;

namespace SimpleFramework.Entry
{
    /// <summary>
    /// 管理所有的manager，外观模式
    /// </summary>
    public class GameFacade
    {
        #region GameFacade单例
        private static GameFacade instance;

        private GameFacade()
        {
            InitManagers();
            AfterInitManagers();
        }

        public static GameFacade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameFacade();
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 存储所有的manager方便统一初始化和销毁
        /// </summary>
        private readonly Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

        /// <summary>
        /// 初始化所有Manager并保存
        /// </summary>
        private void InitManagers()
        {
            // 找到所有继承自IManager的Manager并且要求不是抽象方法或者接口
            var managerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IManager).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface && !t.IsGenericType);
            foreach (var type in managerTypes)
            {

                IManager managerInstance = null;

                // 特殊处理 MonoManager 及其子类
                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    // 创建一个根对象来挂载所有需要 MonoBehaviour 的管理器
                    GameObject monoManagerRoot = new GameObject(type.Name);
                    // 如果是 MonoBehaviour 类型，通过 AddComponent 创建
                    managerInstance = monoManagerRoot.AddComponent(type) as IManager;
                }
                else
                {
                    // 普通 C# 类使用 Activator 创建
                    managerInstance = Activator.CreateInstance(type) as IManager;
                }

                if (managerInstance != null)
                {
                    managerInstance.OnManagerInit();
                    managers.Add(type, managerInstance);
                }
                else
                {
                    Debug.LogWarning($"无法创建管理器实例: {type.Name}");
                }
            }
        }

        /// <summary>
        /// 初始化所有Managers之后
        /// </summary>
        private void AfterInitManagers()
        {
            foreach (var value in managers.Values)
            {
                value.AfterManagerInit();
            }
        }

        /// <summary>
        /// 根据类型获取 manager
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T GetManager<T>() where T : class, IManager
        {
            if (managers.Count == 0)
            {
                InitManagers();
                AfterInitManagers();
            }
            Type type = typeof(T);
            if (managers.ContainsKey(type))
            {
                return managers[type] as T;
            }
            UnityEngine.Debug.LogWarning($"[GameFacade] 错误：找不到类型为 {type.Name} 的管理器！");
            return null;
        }

        /// <summary>
        /// 销毁所有Manager
        /// </summary>
        public void Destory()
        {
            foreach (var manager in managers.Values)
            {
                manager.OnManagerDestroy();
            }
            managers.Clear();

            instance = null;
        }
    }
}