namespace DataAcessLayer.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public int AddUser(UserDTO userDTO)
        {
            try
            {
                User user = (User)userDTO;
                _userRepository.Insert(user);
                _userRepository.Save();

                var users = _userRepository.GetAll();
                return users.FirstOrDefault(x => x.Name == userDTO.Name)?.Id ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting user", ex);
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAll();
                return users.Select(user => (UserDTO)user).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all users", ex);
            }
        }

        public UserDTO GetUser(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                return (UserDTO)user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the user {id}", ex);
            }
        }

        public void UpdateUser(UserDTO userDTO)
        {
            try
            {
                var user = (User)userDTO;
                _userRepository.Update(user);
                _userRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the user", ex);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                _userRepository.Delete(id);
                _userRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the user {id}", ex);
            }
        }

    }
}
