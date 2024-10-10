using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace tcp_worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TCPServer _tcpServer;

        public Worker(ILogger<Worker> logger, TCPServer tcpServer)
        {
            _logger = logger;
            _tcpServer = tcpServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker запущен в: {time}", DateTimeOffset.Now);

            // Запускаем TCP-сервер
            await _tcpServer.StartServerAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
