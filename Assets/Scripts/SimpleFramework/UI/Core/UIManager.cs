using SimpleFramework.Entry;
using SimpleFramework.ObjectPool;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleFramework.UI
{
    public partial class UIManager : IUIManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 层级父节点字典
        /// </summary>
        private readonly Dictionary<UILayerType, UILayerManager> m_layerParents = new();

        /// <summary>
        /// 每个 ui 对应的所有基本信息
        /// </summary>
        private readonly Dictionary<int, UIInformation> panelInfoDic = new();

        /// <summary>
        /// ui 父物体
        /// </summary>
        private Transform canvasTransform;

        /// <summary>
        /// 加载地址
        /// </summary>
        private const string LoadPath = "UI";

        /// <summary>
        /// ui 对象池
        /// </summary>
        private IObjectPool<UIInstance> uiPool;

        /// <summary>
        /// ui Id
        /// </summary>
        private int uiId = 0;

        #region 初始化

        /// <summary>
        /// 初始化 root
        /// </summary>
        private void InitUIRootInternel()
        {
            canvasTransform = GameObject.Find("Canvas").transform;

            if (canvasTransform == null)
            {
                GameObject go = new GameObject("Canvas");
                canvasTransform = go.transform;
                // 初始化组件
                Canvas canvas = go.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                CanvasScaler scaler = go.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                go.AddComponent<GraphicRaycaster>();
            }

            if (!GameObject.Find("EventSystem"))
            {
                GameObject go = new GameObject("EventSystem");

                go.AddComponent<UnityEngine.EventSystems.EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }
        }

        /// <summary>
        /// 初始化所有 ui 层
        /// </summary>
        private void InitLayerParents()
        {
            var layers = Enum.GetValues(typeof(UILayerType));
            foreach (UILayerType layer in layers)
            {
                var go = new GameObject($"Layer_{(int)layer}");
                go.transform.SetParent(canvasTransform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                m_layerParents[layer] = new UILayerManager(layer, go.transform);
            }
        }

        #endregion

        public T OpenPanel<T>(string name) where T : UIBaseView
        {
            return OpenPanelInternal<T>(name, new UIConfig(name), null);
        }

        public T OpenPanelInternal<T>(string name, UIConfig config) where T : UIBaseView
        {
            return OpenPanelInternal<T>(name, config, null);
        }

        public T OpenPanel<T>(string name, UIConfig config, UIControllerCore uiController) where T : UIBaseView
        {
            return OpenPanelInternal<T>(name, config, uiController);
        }


        private T OpenPanelInternal<T>(string name, UIConfig config, UIControllerCore uiController) where T : UIBaseView
        {
            uiId++;
            UIInstance uiInstance = uiPool.Get(name);
            UIInformation uiInformation = new UIInformation(uiId).SetConfig(config).SetController(uiController);
            if (uiInstance == null)
            {
                // 暂时先这样加载后续改为异步(每次都加载创建也是有问题的，需要后续在资源管理器里使用对象池缓存)
                GameObject uiAsset = Resources.Load<GameObject>(Path.Combine(LoadPath, name));
                GameObject readlObj = GameObject.Instantiate(uiAsset);

                uiInstance = new UIInstance(name, uiAsset, readlObj);
                uiPool.Register(uiInstance);

                uiInformation.SetView(readlObj.GetComponent<T>());
            }
            else
            {
                uiInformation.SetView((uiInstance.RealObject as T).GetComponent<UIBaseView>());
            }
            return m_layerParents[config.LayerType].OpenPanel<T>(uiInformation);
        }

        public void ClosePanel(int id)
        {
            if (!panelInfoDic.ContainsKey(id))
            {
                throw new Exception($"无法找到id为：{id}的面板");
            }
            m_layerParents[panelInfoDic[id].Config.LayerType].ClosePanel(panelInfoDic[id]);
        }


        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            canvasTransform = null;
            uiPool = null;
            foreach (var item in m_layerParents.Values)
            {
                item.Clear();
            }
            m_layerParents.Clear();
            foreach (var item in panelInfoDic.Values)
            {
                item.Clear();
            }
            panelInfoDic.Clear();
            uiId = 0;
        }

        #region IManager 接口

        public void OnManagerInit()
        {
            InitUIRootInternel();
            InitLayerParents();
        }

        public void AfterManagerInit()
        {
            uiPool = GameFacade.Instance.GetManager<IObjectPoolManager>().CreatePool<UIInstance>(nameof(UIInstance), 10, 30, 10);
        }

        public void OnManagerDestroy()
        {
            ClearCache();
        }

        #endregion

    }
}

