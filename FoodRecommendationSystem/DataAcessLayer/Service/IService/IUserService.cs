namespace DataAcessLayer.Service.IService
{
    public interface IUserService
    {
        int AddUser(UserDTO userDTO);
        void DeleteUser(int id);
        List<UserDTO> GetAllUsers();
        UserDTO GetUser(int id);
        void UpdateUser(UserDTO userDTO);
    }
}