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
                throw new Exception($"Error whiel logging in the user {ex.Message}");
            }
        }
    }
}
