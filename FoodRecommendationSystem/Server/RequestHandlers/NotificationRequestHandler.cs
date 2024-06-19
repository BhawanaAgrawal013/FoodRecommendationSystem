using DataAcessLayer;
using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using DataAcessLayer.Repository.IRepository;
using DataAcessLayer.Service.IService;

namespace Server.RequestHandlers
{
    public class NotificationRequestHandler
    {
        private readonly INotificationHelper _helper;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IRepository<User> _user;

        public NotificationRequestHandler(INotificationHelper helper)
        {
            _helper = helper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"NOTI_SEND",  SendNotification },
                {"NOTI_RECIEVE", RecieveNotification },
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return "Invalid request.";
        }

        private string SendNotification(string request)
        {
            var parts = request.Split('|');
            var message = parts[1];

            _helper.AddNotification(message);

            return "Notification Sent";
        }

        private string RecieveNotification(string request)
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
    }
}
