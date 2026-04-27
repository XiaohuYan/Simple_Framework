using System;
using Unity.VisualScripting;

namespace SimpleFramework.UI
{
    public partial class UIManager : IUIManager
    {
        /// <summary>
        /// UI MVC ºÍ config
        /// </summary>
        private class UIInformation : IEquatable<UIInformation>
        {
            public UIBaseView View { get; private set; }
            public UIBaseModel Model { get; private set; }
            public UIControllerCore Controller { get; private set; } = null;

            public UIConfig Config { get; private set; }

            public int Id { get; private set; }

            public UIInformation(int id)
            {
                Id = id;
            }

            public UIInformation SetView(UIBaseView view)
            {
                View = view;
                return this;
            }

            public UIInformation SetController(UIControllerCore controller)
            {
                Controller = controller;
                Model = controller.GetModel<UIBaseModel>();
                return this;
            }

            public UIInformation SetConfig(UIConfig config)
            {
                Config = config;
                return this;
            }

            public void Clear()
            {
                Controller = null;
                View = null;
                Model = null;
                Config = null;
            }

            public static bool operator ==(UIInformation a, UIInformation b)
            {
                return a.Id == b.Id;
            }

            public static bool operator !=(UIInformation a, UIInformation b)
            {
                return a.Id != b.Id;
            }

            public bool Equals(UIInformation other)
            {
                return Id.Equals(other.Id);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as UIInformation);
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public override string ToString()
            {
                return Config.PanelName;
            }
        }
    }
}