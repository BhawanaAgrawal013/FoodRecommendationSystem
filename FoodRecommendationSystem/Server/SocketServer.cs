using Serilog;
using Server.RequestHandlers;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer
{
    private readonly IRequestHandler<MenuRequestHandler> _menuRequestHandler;
    private readonly IRequestHandler<MealMenuRequestHandler> _mealMenuRequestHandler;
    private readonly IRequestHandler<LoginRequestHandler> _loginRequestHandler;
    private readonly IRequestHandler<NotificationRequestHandler> _notificationHandler;
    private readonly IRequestHandler<FeedbackRequestHandler> _feedbackRequestHelper;
    private readonly IRequestHandler<DiscardedMenuRequestHandler> _discardedMenuRequestHandler;

    private TcpListener _listener;

    public SocketServer(IRequestHandler<MenuRequestHandler> menuRequestHandler,
                        IRequestHandler<MealMenuRequestHandler> mealMenuHandler,
                        IRequestHandler<LoginRequestHandler> loginRequestHandler, 
                        IRequestHandler<DiscardedMenuRequestHandler> discardRequestHandler, 
                        IRequestHandler<FeedbackRequestHandler> feedbackHandler,
                        IRequestHandler<NotificationRequestHandler> notificationHandler)
    {
        _notificationHandler = notificationHandler;
        _menuRequestHandler = menuRequestHandler;
        _mealMenuRequestHandler = mealMenuHandler;
        _loginRequestHandler = loginRequestHandler;
        _feedbackRequestHelper = feedbackHandler;
        _discardedMenuRequestHandler = discardRequestHandler;
    }

    public void Start()
    {
        _listener = new TcpListener(IPAddress.Any, 5000);
        _listener.Start();
        Console.WriteLine("Socket server started on port 5000.");
        Log.Information("Socket server started on port 5000.");

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
        try
        {
            using (var stream = client.GetStream())
            {
                var buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    try
                    {
                        var response = ProcessRequest(request);
                        if(String.IsNullOrEmpty(response))
                        {
                            Log.Error("No data available");
                            throw new Exception("Error: No data is available");
                        }

                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Exception occured {ex.Message}");

                        var response = ex.Message;
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
            Log.Error(ex.Message);
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
        if(request.StartsWith("DISCARD"))
        {
            return _discardedMenuRequestHandler.HandleRequest(request);
        }

        return "";
    }
}
