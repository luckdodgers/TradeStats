using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Linq;
using System.Windows;
using TradeStats.Infastructure;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;
using TradeStats.Views;
using Unity;
using Serilog;

namespace TradeStats
{
    public partial class App : PrismApplication
    {
        private readonly IUnityContainer _container;
        private readonly ITradesContext _tradesContext;
        private readonly IUpdateCachedData<Account> _curAccountCache;
        private readonly ILogger _logger;

        public App()
        {
            _container = new UnityContainer();
            _container.Configure();

            _curAccountCache = _container.Resolve<IUpdateCachedData<Account>>();
            _tradesContext = _container.Resolve<ITradesContext>();
            _logger = _container.Resolve<ILogger>();

            InitOnAppStart();
        }

        protected override Window CreateShell()
        {
            return _container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Error($"Unhandled exception: {e.ExceptionObject}");
        }

        private void InitOnAppStart()
        {
            var activeAccount = _tradesContext.Accounts.FirstOrDefault(a => a.IsActive == true);

            if (activeAccount != null)
                _curAccountCache.UpdateCache(activeAccount);
        }
    }
}
