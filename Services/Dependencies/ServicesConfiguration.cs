using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TradeStats.Infastructure.Persistance;
using TradeStats.Services.Validations;
using TradeStats.ViewModel.MainWindow;
using TradeStats.ViewModel.ManageAccounts;
using TradeStats.Views.Main;
using TradeStats.Views.ManageAccounts;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace TradeStats.Infastructure
{
    static class ServicesConfiguration
    {
        public static void Configure(this IUnityContainer container)
        {
            // Serilog
            var logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.File($"log-{DateTime.Now}.txt")
                    .CreateLogger();

            container.RegisterInstance(typeof(ILogger), logger);

            // DbContext
            var optionsBuilder = new DbContextOptionsBuilder<TradesContext>();
            optionsBuilder.UseSqlite("Data Source=trades.db");
            optionsBuilder.UseLazyLoadingProxies();
            container.RegisterInstance(optionsBuilder.Options);
            container.RegisterType(typeof(ITradesContext), typeof(TradesContext));

            // Windows
            container.RegisterType<MainWindow>();
            container.RegisterType<ManageAccountsWindow>();

            // Viewmodels
            container.RegisterType<MainWindowViewModel>();
            container.RegisterType<ManageAccountsViewModel>();
        }

        public static void Configure(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.File($"log-{DateTime.Now}.txt")
                    .CreateLogger();

                builder.AddSerilog(logger);
            });

            services.AddDbContext<TradesContext>(options =>
            {
                options.UseSqlite("Data Source=trades.db");
                options.UseLazyLoadingProxies();
            });

            services.AddTransient(typeof(MainWindow));
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
