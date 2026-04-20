using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.UI
{
    public class UIManager : IUIManager
    {
        /// <summary>
        /// 所有实例化的游戏物体面板
        /// </summary>
        private Dictionary<string, UIBaseView> panelDict = new();

        /// <summary>
        /// 所有实例化的游戏物体面板控制器
        /// </summary>
        private Dictionary<string, UIControllerCore> panelControllerDict = new();

        /// <summary>
        /// ui 父物体
        /// </summary>
        private Transform canvasTransform;

        /// <summary>
        /// UI 栈
        /// </summary>
        private Stack<UIBaseView> panelStack = new Stack<UIBaseView>();

        /// <summary>
        /// ui 入栈
        /// </summary>
        /// <param name="name"></param>
        public void PushPanel(string name, UIControllerCore controller = default)
        {
            UIBaseView panel = GetPanel(name);
            if (panel != null)
            {
                if (!panelControllerDict.ContainsKey(name))
                {
                    panelControllerDict.Add(name, controller);
                    controller?.InitController(panel);
                }
                if (panelStack.Count > 0)
                {
                    panelStack.Peek().OnPause();
                }
                panel.OnEnter();
                panelStack.Push(panel);
            }
        }

        /// <summary>
        /// ui 出栈
        /// </summary>
        public void PopPanel()
        {
            if (panelStack.Count > 0)
            {
                panelDict.Remove(panelStack.Peek().name);
                panelStack.Pop().OnExit();
                if (panelControllerDict.TryGetValue(panelStack.Peek().name, out var controller))
                {
                    controller.DisposeController();
                }
                panelControllerDict.Remove(panelStack.Peek().name);
            }
            if (panelStack.Count > 0)
            {
                panelStack.Peek().OnResume();
            }
        }

        /// <summary>
        /// 获取到 UI
        /// </summary>
        /// <param name="name">ui名</param>
        /// <returns></returns>
        private UIBaseView GetPanel(string name)
        {
            UIBaseView panel;
            if (!panelDict.TryGetValue(name, out panel))
            {
                GameObject panelInstance = GameObject.Instantiate(Resources.Load<GameObject>($"UI/{name}Panel"));
                if (canvasTransform == null)
                {
                    canvasTransform = GameObject.Find("Canvas").transform;
                }
                panelInstance.transform.SetParent(canvasTransform, false);
                panel = panelInstance.GetComponent<UIBaseView>();
                panelDict.Add(name, panel);
            }
            return panel;
        }

        public void OnManagerInit()
        {
            canvasTransform = GameObject.Find("Canvas").transform;
        }

        public void AfterManagerInit()
        {

        }

        public void OnManagerDestroy()
        {
            panelStack.Clear();
            panelDict.Clear();
            panelControllerDict.Clear();
        }

    }
}

