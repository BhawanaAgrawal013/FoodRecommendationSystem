using Serilog;
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
            int delay = 2000;

            while (retryCount < maxRetries)
            {
                try
                {
                    _client = new TcpClient(ipAddress, port);
                    _stream = _client.GetStream();
                    Console.WriteLine("Connected to server.");
                    Log.Information("Connected to server.");
                    break;
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Attempt {retryCount + 1} - Error connecting to server: {ex.Message}");
                    Log.Information($"Attempt {retryCount + 1} - Error connecting to server: {ex.Message}");

                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        Thread.Sleep(delay);
                    }
                    else
                    {
                        Console.WriteLine("Max retry attempts reached. Unable to connect to server. Please restart your system");
                        Log.Warning("Max retry attempts reached. Unable to connect to server.");
                    }
                }
            }
        }

        public void CloseClient()
        {
            try
            {
                _client.Close();
                Log.Information("Closing the client connection");
                Console.WriteLine("Closed Server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing the client: {ex.Message}");
                ResetConsole();
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
                Log.Information($"Sending {message} request to the server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                ResetConsole();
            }
        }

        public string RecieveMessage()
        {
            byte[] buffer = new byte[4096];
            StringBuilder message = new StringBuilder();
            int bytesRead;

            do
            {
                bytesRead = _stream.Read(buffer, 0, buffer.Length);
                message.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            } while (_stream.DataAvailable);

            var result = message.ToString();

            if (result.StartsWith("Error"))
            {
                throw new Exception(result);
            }

            return result;
        }

        private void ResetConsole()
        {
            Console.WriteLine("Resetting console...");
            Console.Clear();
        }
    }
}