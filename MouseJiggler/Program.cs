using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MouseJiggler
{


    public class Program
    {
        public static async Task Main(string[] args)
        {

            IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "MouseJiggler Service";
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}