using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks; // Не забудьте добавить этот using для Task

Console.Title = "Client"; // Устанавливаем заголовок консольного окна

try
{
    // Создаем новый экземпляр TcpClient для подключения к серверу
    var clientSocket = new TcpClient();

    // Подключаемся к серверу на локальном хосте по порту 5000
    clientSocket.Connect("localhost", 5000);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 2 Клиент говорит, что подключился к серверу: " + clientSocket.Connected.ToString().ToLower()); // Логируем статус соединения
    Console.WriteLine();
    // Получаем поток для чтения и записи данных
    var stream = clientSocket.GetStream();

    // Устанавливаем тайм-ауты для отправки и получения данных
    clientSocket.SendTimeout = 30;
    clientSocket.ReceiveTimeout = 30;

    // Подготавливаем сообщение для отправки (здесь оно простое, для примера)
    var header = Encoding.ASCII.GetBytes("client message");

      // Отправляем байты в поток
    stream.Write(header, 0, header.Length);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 4 --> отправили с клиента на сервер: {Encoding.ASCII.GetString(header)}"); // Логируем отправленное сообщение и время


    await Task.Delay(1000);

    // Буфер для получения данных от сервера
    byte[] bt = new byte[1024];

    // Получаем данные от сервера:
    int bytesread = clientSocket.Client.Receive(bt);

    // Декодируем полученные байты в строку
    string result = Encoding.ASCII.GetString(bt, 0, bytesread);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 7 <-- получили с сервера на клиент: {result}"); // Логируем полученное сообщение
}
catch (SocketException ex)
{
    // Обрабатываем ошибки сокета и выводим сообщение об ошибке
    Console.WriteLine($"Ошибка сокета: {ex.Message}");
}
catch (Exception ex)
{
    // Обрабатываем другие ошибки и выводим сообщение об ошибке
    Console.WriteLine($"Ошибка: {ex.Message}");
}
finally
{
    // Сообщаем о закрытии клиента
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 8 Закрыли клиент.");
    Console.ReadKey(); // Ждем нажатия клавиши перед закрытием консоли
}
