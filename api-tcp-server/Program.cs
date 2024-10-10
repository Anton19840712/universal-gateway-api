using System.Net.Sockets;
using System.Net;
using System.Text;

// tcp sample server
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Server";

        bool done = false;
        string DELIMITER = "|";
        string TERMINATE = "TERMINATE";
        int port = 54321;
        IPAddress address = IPAddress.Any;

        // Создаем tcp сервер
        TcpListener server = new TcpListener(address, port);
        server.Start();
        var loggedNoRequest = false;

        // Все время прослушиваем сообщения, которые может прислать нам сервер
        while (!done)
        {
            if (!server.Pending())
            {
                if (!loggedNoRequest)
                {
                    Console.WriteLine();
                    Console.WriteLine("No pending requests as of yet");
                    Console.WriteLine("Server listening...");
                    loggedNoRequest = true;
                }
            }
            else
            {
                loggedNoRequest = false;
                byte[] bytes = new byte[256];

                using (var client = await server.AcceptTcpClientAsync())
                {
                    using (var tcpStream = client.GetStream())
                    {
                        // зачитали stream в bytes
                        await tcpStream.ReadAsync(bytes, 0, bytes.Length);

                        var requestMessage = Encoding.UTF8.GetString(bytes).Replace("\0", string.Empty);

                        if (requestMessage.Equals(TERMINATE))
                        {
                            done = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Message received from client:");
                            Console.WriteLine(requestMessage);

                            var payload = requestMessage.Split(DELIMITER).Last();
                            var responseMessage = $"Greetings from the server! | {payload}";
                            var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                            await tcpStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        }
                    }
                }
            }
        }
        server.Stop();
        Thread.Sleep(10000);
    }
}