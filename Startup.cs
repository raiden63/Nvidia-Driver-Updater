using Microsoft.Extensions.DependencyInjection;
using Client = NvidiaDriverUpdater.NvidiaClient;

namespace NvidiaDriverUpdater
{
    public class Startup
    {
        public static ServiceProvider BuildServices()
        {
            var services = new ServiceCollection();

            services.AddHttpClient<Client.INvidiaClient, Client.NvidiaClient>();

            return services.BuildServiceProvider();
        }
    }
}