using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using TradeStats.Infastructure.Persistance;
using TradeStats.Models.Domain;
using TradeStats.Services.Cache;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Persistance;
using TradeStats.ViewModel.MainWindow;
using TradeStats.ViewModel.ManageAccounts;
using TradeStats.Views;
using Unity;
using TradeStats.Services.Mappings;
using AutoMapper;
using TradeStats.Services.ExternalData;
using System.IO;

namespace TradeStats.Infastructure
{
    static class ServicesConfiguration
    {
        public static void Configure(this IUnityContainer container)
        {
            // Serilog
            var dtFormat = @"dd-MM-yyyy";
            var filename = Path.Combine(@"D:", @"Logs", $"trade_stats_log-{DateTime.Now.ToString(dtFormat)}.txt");
            var logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .Enrich.FromLogContext()
                    .WriteTo.File(path: filename, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                    .CreateLogger();

            container.RegisterInstance(typeof(ILogger), logger);

            // Mappings
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            container.RegisterInstance<IConfigurationProvider>(config);
            container.RegisterInstance<IMapper>(config.CreateMapper());

            // Settings
            container.RegisterSingleton<ISettingsProvider, JsonSettingsProvider>();

            // Cache
            container.RegisterSingleton<CurrentAccountCache>();
            container.RegisterFactory<ICachedData<Account>>((obj) => container.Resolve<CurrentAccountCache>());
            container.RegisterFactory<IUpdateCachedData<Account>>((obj) => container.Resolve<CurrentAccountCache>());

            // DbContext
            container.RegisterType<ITradesContext, TradesContext>();
            container.RegisterType<ICurrentAccountTradeContext, CurrentAccountTradeContext>();

            // Windows
            container.RegisterType<MainWindow>();
            container.RegisterType<ManageAccountsWindow>();

            // Viewmodels
            container.RegisterType<MainWindowViewModel>();
            container.RegisterType<ManageAccountsViewModel>();

            // Data load
            container.RegisterType<ICsv, Csv>();
            container.RegisterType<IExternalDataManager, ExternalDataManager>();
            container.RegisterType<IOpenTradesLoader, OpenTradesLoader>();
        }

        private static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypesAssignableFrom(typeof(T));
        }

        private static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
        {
            List<Type> result = new List<Type>();

            foreach (var type in assembly.DefinedTypes)
            {
                if (compareType.IsAssignableFrom(type) && compareType != type)
                {
                    result.Add(type);
                }
            }

            return result;
        }
    }
}
