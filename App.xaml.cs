using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System.ComponentModel;
using System.Windows;
using TradeStats.Infastructure;
using TradeStats.Services.Validations;
using TradeStats.Views;
using TradeStats.Views.Main;
using Unity;

namespace TradeStats
{
    public partial class App : PrismApplication
    {
        private readonly IUnityContainer _container;

        public App()
        {
            _container = new UnityContainer();
            _container.Configure();
        }

        protected override Window CreateShell()
        {
            return _container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
