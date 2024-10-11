using System.Net.Sockets;
using System.Net;
using System.Text;

class Program
{
    // это код клиента
    static void Main(string[] args)
    {
        bool exception_thrown = false;

        // открываем сокет
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress sendto = IPAddress.Parse("127.0.0.1");
        IPEndPoint endpoint = new IPEndPoint(sendto, 5000);

        string texttosend = "test-message-from-client";

        byte[] sendbuffer = Encoding.ASCII.GetBytes(texttosend);

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Trying to send to: 127.0.0.1 port: 5000 message like {texttosend}");

        try
        {
            socket.SendTo(sendbuffer, endpoint);
        }
        catch (Exception e)
        {
            exception_thrown = true;
            Console.WriteLine(" Exception {0}", e.Message);
        }
        if (exception_thrown == false)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Sent message to a server");
        }
        else
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] The exception indicates the message was not sent.");
        }

        Console.ReadKey();
    }
}
