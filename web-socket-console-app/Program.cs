// Тебе нужно сделать главным приложение web-socket-sample
// Потом правой кнопкой мыши выбрать проект web-socket-console-app,
// Выбрать debug и запустить instance приложения... тогда в приложении, которое слушает на порту 52928 будет передано 
// Отсюда сообщение test

using System.Net.WebSockets;
using System.Text;

// Создаем новый клиент WebSocket
var wb = new ClientWebSocket();

// Подключаемся к WebSocket-серверу
// Который у нас создается на фронте socketpage.cshtml

await wb.ConnectAsync(new Uri("ws://localhost:52928/ws"), CancellationToken.None);

// Далее отправляем туда сообщение
// Отправка сообщения в фоне
_ = Task.Run(async () =>
{
    bool off = false;
    while (!off)
    {
        if (wb.State == WebSocketState.Open)
        {
            // Посылаем сообщение в web socket
            await wb.SendAsync(
                Encoding.ASCII.GetBytes("tst"),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
            off = true;
        }
    }
});

// Получение сообщения
bool receiveOff = false;
while (!receiveOff)
{
    if (wb.State == WebSocketState.Open)
    {
        byte[] buffer = new byte[1024];
        var result = await wb.ReceiveAsync(buffer, CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Text)
        {
            // Читаем сообщение из буфера и печатаем его.
            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
        }
    }
}

Console.ReadKey();
