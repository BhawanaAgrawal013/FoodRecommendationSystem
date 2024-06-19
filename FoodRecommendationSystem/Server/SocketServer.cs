using DataAcessLayer.Service.IService;
using Server.RequestHandlers;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

public class SocketServer
{
    private readonly MenuRequestHandler _menuRequestHandler;
    private readonly IMealNameService _mealNameService;
    private readonly MealMenuRequestHandler _mealMenuRequestHandler;
    private readonly IRecommendationEngineService _service;
    private readonly ILoginService _loginService;
    private readonly LoginRequestHandler _loginRequestHandler;
    private TcpListener _listener;

    public SocketServer(IMealNameService mealNameService, IRecommendationEngineService service, ILoginService loginService)
    {
        _mealNameService = mealNameService;
        _service = service;
        _loginService = loginService;
        _menuRequestHandler = new MenuRequestHandler(_mealNameService);
        _mealMenuRequestHandler = new MealMenuRequestHandler(_service);
        _loginRequestHandler = new LoginRequestHandler(_loginService);
    }

    public void Start()
    {
        _listener = new TcpListener(IPAddress.Any, 5000);
        _listener.Start();
        Console.WriteLine("Socket server started on port 5000.");

        while (true)
        {
            try
            {
                var client = _listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    private void HandleClient(TcpClient client)
    {
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var response = ProcessRequest(request);
                var responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
        }
    }

    private string ProcessRequest(string request)
    {
        if(request.StartsWith("MENU"))
        {
            return _menuRequestHandler.HandleRequest(request);   
        }
        if(request.StartsWith("MEAL"))
        {
            return _mealMenuRequestHandler.HandleRequest(request);
        }
        if(request.StartsWith("LOGIN"))
        {
            return _loginRequestHandler.HandleRequest(request);
        }

        return "";
    }
}
