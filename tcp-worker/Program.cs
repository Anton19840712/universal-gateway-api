using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tcp_worker;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<TCPServer>(); // Регистрируем TCPServer
                services.AddHostedService<Worker>();
            });
}