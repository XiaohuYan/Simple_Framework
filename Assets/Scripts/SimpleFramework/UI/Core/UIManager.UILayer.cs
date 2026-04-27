using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFramework.Extension;

namespace SimpleFramework.UI
{
    public partial class UIManager : IUIManager
    {
        /// <summary>
        /// UI 层级管理器
        /// </summary>
        private class UILayerManager
        {
            private UILayerType uiLayerType;

            private readonly LinkedList<UIInformation> panelLinkedList = new();

            private Transform uiParent;

            public int Count => panelLinkedList.Count;

            public UILayerManager(UILayerType uiLayerType, Transform uiParent)
            {
                this.uiLayerType = uiLayerType;
                this.uiParent = uiParent;
            }

            public T OpenPanel<T>(UIInformation panel) where T : UIBaseView
            {
                if (panel == null)
                {
                    throw new System.ArgumentNullException("面板空错误");
                }

                if (panel.Config.isCoveredLastView && panelLinkedList.Count != 0)
                {

                    panelLinkedList.Last.Value.View.OnPause();
                }
                SetTransform(panel);
                panel.View.OnEnter();
                panelLinkedList.AddLast(panel);
                return panel.View as T;
            }

            public void ClosePanel(UIInformation panel)
            {
                LinkedListNode<UIInformation> panelNode = panelLinkedList.Find(panel);
                if (panelNode != null)
                {
                    panelNode.Value.View.OnExit();
                    if (panelNode.Value.Config.isCoveredLastView && panelNode == panelLinkedList.Last && panelNode.Previous != null)
                    {
                        panelNode.Previous.Value.View.OnResume();
                    }
                }
                else
                {
                    throw new System.ArgumentNullException($"无法找到面板{panel.ToString()}，所以关闭无效");
                }
            }

            private void SetTransform(UIInformation uiInformation)
            {
                uiInformation.View.transform.SetParent(uiParent);
                uiInformation.View.transform.localPosition = Vector3.zero;
                uiInformation.View.transform.localScale = Vector3.one;
            }

            private void Hide(UIInformation ui)
            {
                switch (ui.Config.CloseType)
                {
                    case UICloseType.SetVectorZero:
                        ui.View.transform.localScale = Vector3.zero;
                        break;
                    case UICloseType.SetActiveFalse:
                        ui.View.gameObject.SetActive(false);
                        break;
                    case UICloseType.SetCanvasRendererFalse:
                        foreach (var cr in ui.View.GetComponentsInChildren<Graphic>())
                        {
                            cr.enabled = false;
                        }
                        break;
                    case UICloseType.SetCanvasGroupAlphaZero:
                        ui.View.gameObject.GetOrAdd<CanvasGroup>().alpha = 0f;
                        break;
                }
            }

            public void Clear()
            {
                uiLayerType = default;
                foreach(var item in panelLinkedList)
                {
                    item.Clear();
                }
                panelLinkedList.Clear();
                uiParent = null;
            }
        }
    }
}