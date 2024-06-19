using DataAcessLayer.ModelDTOs;
using DataAcessLayer.Service.IService;
using Newtonsoft.Json;

namespace Server.RequestHandlers
{
    public class LoginRequestHandler
    {
        private readonly ILoginService _service;

        public LoginRequestHandler(ILoginService service)
        {
            _service = service;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"LOGIN",  AuthenticateUser }
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

        private string AuthenticateUser(string request)
        {
            var parts = request.Split('|');
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(parts[1]);

            string result = _service.Login(userDTO);

            return result;
        }
    }
}
