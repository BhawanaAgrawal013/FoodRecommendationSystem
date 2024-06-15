using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketClient
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public SocketClient(string ipAddress, int port)
        {
            int retryCount = 0;
            int maxRetries = 5;
            int delay = 2000; // 2 seconds

            while (retryCount < maxRetries)
            {
                try
                {
                    _client = new TcpClient(ipAddress, port);
                    _stream = _client.GetStream();
                    Console.WriteLine("Connected to server.");
                    break;
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Attempt {retryCount + 1} - Error connecting to server: {ex.Message}");
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        Thread.Sleep(delay);
                    }
                    else
                    {
                        Console.WriteLine("Max retry attempts reached. Unable to connect to server.");
                    }
                }
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {response}");
        }
    }
}
