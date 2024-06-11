using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Chef App started...");
        while (true)
        {
            Console.Write("Enter notification message: ");
            string message = Console.ReadLine();

            SendNotification(message);
        }
    }

    static void SendNotification(string message)
    {
        TcpClient client = new TcpClient("127.0.0.1", 9000);
        NetworkStream stream = client.GetStream();

        byte[] buffer = Encoding.ASCII.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);

        buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine(response);

        client.Close();
    }
}