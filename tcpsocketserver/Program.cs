using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Server"; // Устанавливаем заголовок консольного окна

        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 1 Сервер запущен и ожидает подключения клиента.");

        var client = listener.AcceptTcpClient();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 3 Сервер говорит, что клиент реально подключился.");
        Console.WriteLine();
        try
        {
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);
            writer.AutoFlush = true; // Включаем автоматическую очистку

            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (bytesRead != 0)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 5 --> получили с клиента на сервер: {dataReceived}");

                    string response = "server message";
                    Console.Write($"[{DateTime.Now:HH:mm:ss.fff}] 6 <-- отправили ответ с сервера на клиент: {response}");

                    writer.Write(response); // Отправляем ответ клиенту
                }
                else
                {
                    break; // Выходим из цикла, если ничего не получено
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}"); // Логируем ошибку
        }
        finally
        {
            client.Close(); // Закрываем соединение с клиентом
            listener.Stop(); // Останавливаем слушатель
            Console.WriteLine("Сервер остановлен.");
        }

        Console.WriteLine("1");
    }
}
