using Serilog;

namespace DataAcessLayer.Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;

        public LoginService(IUserService userService)
        {
            _userService = userService;
        }

        public string Login(UserDTO userDTO, string roleName)
        {
            try
            {
                List<UserDTO> users = _userService.GetAllUsers();

                var user = users.SingleOrDefault(u => u.Email == userDTO.Email && u.Password == userDTO.Password
                                                    && u.Role.RoleName == roleName);

                if (user != null)
                {
                    return "Login Successful";
                }

                return "Invalid Email and Password";
            }
            catch (Exception ex)
            {
                Log.Error($"Error occurred during login: {ex.Message}");
                throw new Exception("Error occurred during login", ex);
            }
        }

    }
}
