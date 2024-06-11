using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    private static TcpListener _listener;
    static void Main(string[] args)
    {
        Console.WriteLine("Employee App started...");

        _listener = new TcpListener(IPAddress.Any, 9000);
        _listener.Start();
        Console.WriteLine("Notification Server started...");

        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            Thread thread = new Thread(HandleClient);
            thread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        Console.WriteLine($"Notification received: {message}");

        byte[] response = Encoding.ASCII.GetBytes("Notification sent to employees");
        stream.Write(response, 0, response.Length);

        client.Close();
    }
}

