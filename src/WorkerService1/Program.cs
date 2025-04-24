using KafkaBatchConsumerApp.Services;

namespace WorkerService1
{
    public class Program
    {
        public async static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
                services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
                // Add other services you might need
            })
            .Build();

            await host.RunAsync();
        }
    }
}