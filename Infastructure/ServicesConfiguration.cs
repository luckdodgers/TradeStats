using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using TradeStats.Infastructure.Persistance;
using TradeStats.ViewModel.MainWindow;
using TradeStats.Views.Main;
using Unity;

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

            // Viewmodels
            container.RegisterType<MainWindowViewModel>();
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
    }
}
