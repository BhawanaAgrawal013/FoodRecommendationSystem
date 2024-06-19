namespace DataAcessLayer.Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<User> _userRepository;

        public LoginService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public string Login(UserDTO userDTO)
        {
            IEnumerable<User> users = _userRepository.GetAll();

            var user = users.Where(x => x.Email == userDTO.Email);

            if (user != null)
            {
                if (user.Select(x => x.Password).ToString() == userDTO.Password)
                {
                    return "Logged in Sucessfully";
                }
            }

            return "Invalid Email and Password";
        }
    }
}
