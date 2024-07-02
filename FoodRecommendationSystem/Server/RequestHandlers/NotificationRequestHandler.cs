using DataAcessLayer.Helpers.IHelpers;
using Serilog;

namespace Server.RequestHandlers
{
    public class NotificationRequestHandler : IRequestHandler<NotificationRequestHandler>
    {
        private readonly INotificationHelper _helper;

        public NotificationRequestHandler(INotificationHelper helper)
        {
            _helper = helper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"NOTI_SEND",  SendNotification },
                {"NOTI_RECEIVE", RecieveNotification },
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return "";
        }

        private string SendNotification(string request)
        {
            try
            {
                var parts = request.Split('|');
                var message = parts[1];

                _helper.AddNotification(message);

                return "Notification Sent";
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending the notification {ex.Message}");
                throw new Exception($"Error sending the notification {ex.Message}");
            }
        }

        private string RecieveNotification(string request)
        {
            try
            {
                var parts = request.Split('|');
                var email = parts[1];

                var notifications = _helper.GetUnreadNotification(email);

                string result = String.Empty;

                foreach (var notification in notifications)
                {
                    result += ($"\n {notification.Notification.NotificationMessage} \t {notification.Notification.DateTime}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error recieving the notification {ex.Message}");
                throw new Exception($"Error recieving the notification {ex.Message}");
            }
        }
    }
}
