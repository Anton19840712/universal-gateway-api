using System.Net.WebSockets;
using System.Text;

// Основное приложение
class Program
{
    static async Task Main(string[] args)
    {
        // Создаем новый клиент WebSocket
        var wb = new ClientWebSocket();

        // Подключаемся к WebSocket-серверу
        await wb.ConnectAsync(new Uri("ws://localhost:52928/ws"), CancellationToken.None);

        // Отправка сообщения в фоне
        _ = Task.Run(async () =>
        {
            while (wb.State == WebSocketState.Open)
            {
                // Отправляем тестовое сообщение
                await wb.SendAsync(
                    Encoding.ASCII.GetBytes("test"),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
            }
        });

        // Получение сообщения
        while (wb.State == WebSocketState.Open)
        {
            byte[] buffer = new byte[1024];
            var result = await wb.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
            }
        }

        Console.ReadKey();
    }
}
