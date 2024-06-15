using System.Net.Sockets;
using System.Net;
using System.Text;
using DataAcessLayer.Service.IService;
using Microsoft.Extensions.DependencyInjection;

public class SocketServer
{
    private readonly TcpListener _server;
    private readonly IMealNameService _mealNameService;

    public SocketServer(IMealNameService mealNameService)
    {
        _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
        _mealNameService = mealNameService;
    }

    public void Start()
    {
        _server.Start();
        Console.WriteLine("Server started...");
        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Received: {dataReceived}");

        // Process the request and send a response
        string response = ProcessRequest(dataReceived);
        byte[] responseData = Encoding.ASCII.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);

        client.Close();
    }

    private string ProcessRequest(string request)
    {
        // Basic request processing logic
        if (request.StartsWith("GET_MENU"))
        {
            var mealName = _mealNameService.GetAllMeals();
            string meals = String.Join(' ', mealName.Select(x => x.MealName));

            return meals;
        }
        else if (request.StartsWith("ADD_FEEDBACK"))
        {
            // Parse feedback details and add to the service
            var parts = request.Split('|');

            return "Feedback added." + parts;
        }
        else
        {
            return "Invalid request.";
        }
    }
}