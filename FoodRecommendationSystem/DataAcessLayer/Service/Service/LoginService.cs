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
            List<UserDTO> users = _userService.GetAllUsers();

            var user = users.SingleOrDefault(u => u.Email == userDTO.Email && u.Password == userDTO.Password
                                            && u.Role.RoleName == roleName);

            if (user != null)
            {
                return "Login Sucessfull";
            }

            return "Invalid Email and Password";
        }
    }
}
