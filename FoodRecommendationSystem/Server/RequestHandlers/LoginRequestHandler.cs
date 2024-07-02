using DataAcessLayer;
using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Serilog;

namespace Server.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequestHandler>
    {
        private readonly ILoginHelper _loginHelper;

        public LoginRequestHandler(ILoginHelper loginHelper)
        {
            _loginHelper = loginHelper;
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

            string result = _loginHelper.LoginUser(userDTO, parts[2]);

            Log.Information($"Login Handler: {result} for user {userDTO.Email}.");

            return result;
        }
    }
}
