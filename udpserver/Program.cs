using System.Net.Sockets;
using System.Net;
using System.Text;


//Это код сервера
class Program
{
    static void Main(string[] args)
    {
        // создаем клиента, чтобы получить данные
        UdpClient listener = new UdpClient(5000);
        IPEndPoint EP = new IPEndPoint(IPAddress.Any, 5000);

        try
        {
            Console.WriteLine("Waiting...");
            byte[] receivedbytes = listener.Receive(ref EP);

            string data = Encoding.ASCII.GetString(receivedbytes, 0, receivedbytes.Length);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Было получено сообщение с клиента {data}");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        listener.Close();
    }
}