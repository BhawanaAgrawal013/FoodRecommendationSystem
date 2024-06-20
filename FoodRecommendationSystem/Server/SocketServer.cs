using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.Service.IService;
using Server.RequestHandlers;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer
{
    private readonly MenuRequestHandler _menuRequestHandler;
    private readonly IMealNameService _mealNameService;
    private readonly MealMenuRequestHandler _mealMenuRequestHandler;
    private readonly IRecommendationEngineService _service;
    private readonly ILoginService _loginService;
    private readonly LoginRequestHandler _loginRequestHandler;
    private readonly NotificationRequestHandler _notificationHandler;
    private readonly INotificationHelper _notificationHelper;
    private readonly IChefHelper _chefHelper;
    private readonly IEmployeeHelper _employeeHelper;
    private readonly FeedbackRequestHandler _feedbackRequestHelper;
    private readonly IFeedbackHelper _feedbackHelper;

    private TcpListener _listener;

    public SocketServer(IMealNameService mealNameService, IRecommendationEngineService service, 
                        ILoginService loginService, INotificationHelper notificationHelper, IChefHelper chefHelper, 
                        IEmployeeHelper employeeHelper, IFeedbackHelper feedbackHelper)
    {
        _mealNameService = mealNameService;
        _service = service;
        _loginService = loginService;
        _notificationHelper = notificationHelper;
        _chefHelper = chefHelper;
        _employeeHelper = employeeHelper;
        _notificationHandler = new NotificationRequestHandler(_notificationHelper);
        _menuRequestHandler = new MenuRequestHandler(_mealNameService);
        _mealMenuRequestHandler = new MealMenuRequestHandler(_service, _chefHelper, _employeeHelper);
        _loginRequestHandler = new LoginRequestHandler(_loginService);
        _employeeHelper = employeeHelper;
        _feedbackHelper = feedbackHelper;
        _feedbackRequestHelper = new FeedbackRequestHandler(_feedbackHelper);
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
        if(request.StartsWith("NOTI"))
        {
            return _notificationHandler.HandleRequest(request);
        }
        if(request.StartsWith("FEEDBACK"))
        {
            return _feedbackRequestHelper.HandleRequest(request);
        }

        return "";
    }
}
