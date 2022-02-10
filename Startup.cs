using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Client = NvidiaDriverUpdater.NvidiaClient;

namespace NvidiaDriverUpdater
{
    public class Startup
    {
        public static ServiceProvider BuildServices(IConfiguration config)
        {
            var services = new ServiceCollection();

            services.AddHttpClient<Client.INvidiaClient, Client.NvidiaClient>(
                client => {
                    client.BaseAddress = new Uri(config["Nvidia:BaseUri"]);
                }
            );

            return services.BuildServiceProvider();
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