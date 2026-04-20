using SimpleFramework.Common;

namespace SimpleFramework.UI
{
    public interface IUIManager : IManager
    {
        void PushPanel(string name, UIControllerCore controller = default);

        void PopPanel();
    }
}