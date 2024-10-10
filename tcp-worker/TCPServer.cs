using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace tcp_worker
{
    public class TCPServer
    {
        private readonly ILogger<TCPServer> _logger;
        private TcpListener? _tcpListener;

        public TCPServer(ILogger<TCPServer> logger)
        {
            _logger = logger;
        }

        public async Task StartServerAsync(CancellationToken cancellationToken)
        {
            var port = 13000;
            var hostAddress = IPAddress.Parse("127.0.0.1");

            _tcpListener = new TcpListener(hostAddress, port);
            _tcpListener.Start();
            _logger.LogInformation("TCP Server запущен на {Address}:{Port}", hostAddress, port);

            byte[] buffer = new byte[256];

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    using TcpClient client = await _tcpListener.AcceptTcpClientAsync(cancellationToken);
                    _logger.LogInformation("Клиент подключен: {Client}", client.Client.RemoteEndPoint);

                    var tcpStream = client.GetStream();
                    int readTotal;

                    while ((readTotal = await tcpStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
                    {
                        string incomingMessage = Encoding.UTF8.GetString(buffer, 0, readTotal).Replace("\0", string.Empty);

                        if (!string.IsNullOrWhiteSpace(incomingMessage))
                        {
                            _logger.LogInformation("Получено сообщение: {Message}", incomingMessage);
                        }
                    }

                    _logger.LogInformation("Клиент отключен: {Client}", client.Client.RemoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в TCP-сервере: {Message}", ex.Message);
            }
            finally
            {
                _tcpListener?.Stop();
                _logger.LogInformation("TCP Server остановлен.");
            }
        }
    }
}
