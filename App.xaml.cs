using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using TradeStats.Infastructure;
using TradeStats.Views;
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

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.Configure();
            //_serviceProvider = serviceCollection.BuildServiceProvider();
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
