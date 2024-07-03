using Serilog;

namespace DataAcessLayer.Helpers
{
    public class LoginHelper : ILoginHelper
    {
        private readonly ILoginService _loginService;

        public LoginHelper(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public string LoginUser(UserDTO userDTO, string roleName)
        {
            try
            {
                return _loginService.Login(userDTO, roleName);
            }
            catch (Exception ex)
            {
                Log.Error($"Error while logging in the user: {ex.Message}");
                throw new Exception($"Error while logging in the user", ex);
            }
        }

    }
}
