using System.Net.Sockets;
using System.Net;
using System.Text;

// важно сначала запускать сначала сервер, потом уже клиент
// tcp sample client
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Client";

        int port = 54321;
        IPAddress address = IPAddress.Parse("127.0.0.1");
        // создаем список сообщений:
        var messages = new string[] {
            "Hello server | return this payload to sender!",
            "To the server | send this payload back to me!",
            "Server header | another returned message.",
            "Header value | payload to be returned",
            "TERMINATE"
        };
        foreach (var message in messages)
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(address, port);
                if (client.Connected)
                {
                    Console.WriteLine("We have connected from the client");
                }

                // кодируем сообщение в байты:
                var bytes = Encoding.UTF8.GetBytes(message);
                using (var requestStream = client.GetStream())
                {
                    // пишем байты в стрим
                    await requestStream.WriteAsync(bytes, 0, bytes.Length);
                    var responseBytes = new byte[256];

                    await requestStream.ReadAsync(responseBytes, 0, responseBytes.Length);
                    var responseMessage = Encoding.UTF8.GetString(responseBytes);

                    Console.WriteLine();
                    Console.WriteLine("Response received from server");
                    // декодируем байты в сообщение, чтобы понять, что за сообщение мы отправили на сервер
                    Console.WriteLine(responseMessage);
                    Console.WriteLine();
                }
            }
            if (message.Equals("TERMINATE")) {
                break;
            }

            var sleepDuration = new Random().Next(2000, 10000);
            //Новое сообщение будет сгенерировано через такое-то количество секунд
            Console.WriteLine($"Generating a new request in {sleepDuration / 1000} seconds");

            //засыпаем до чтения следующего сообщения
            Thread.Sleep(sleepDuration);
        }
    }
}
