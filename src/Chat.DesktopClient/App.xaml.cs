using System.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using Chat.DesktopClient.Views;
using Chat.DesktopClient.ViewModels;

namespace Chat.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
