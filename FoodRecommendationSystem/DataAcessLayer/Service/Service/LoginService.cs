﻿namespace DataAcessLayer.Service.Service
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

            var user = users.SingleOrDefault(u => u.Email == userDTO.Email && u.Password == userDTO.Password);

            if (user != null)
            {
                return "Login Sucessfull";
            }

            return "Invalid Email and Password";
        }
    }
}
