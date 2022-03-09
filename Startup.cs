using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Client = NvidiaDriverUpdater.NvidiaClient.V2;

namespace NvidiaDriverUpdater
{
    public class Startup
    {
        public static ServiceProvider BuildServices(IConfiguration config)
        {
            var services = new ServiceCollection();
            var appSettings = config.Get<AppSettings>();

            services.AddSingleton<AppSettings>(appSettings);

            services.AddSingleton<IConfiguration>(config);

            services.AddHttpClient<Client.INvidiaClient, Client.NvidiaClient>(
                client => {
                    client.BaseAddress = new Uri(appSettings.Nvidia.BaseUri);
                }
            );

            services.AddSingleton<ILogger>(BuildSerilogLogger(config));

            return services.BuildServiceProvider();
        }

        private static ILogger BuildSerilogLogger(IConfiguration config)
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console(theme: SystemConsoleTheme.Colored)
            ;

            return loggerConfig.CreateLogger();
        }

        public static IConfiguration BuildConfig()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.json", optional: false);
            configBuilder.AddJsonFile("appsettings.user.json", optional: true);

            return configBuilder.Build();
        }
    }
}