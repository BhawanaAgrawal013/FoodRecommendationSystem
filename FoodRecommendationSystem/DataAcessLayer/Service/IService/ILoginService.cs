namespace DataAcessLayer.Service.IService
{
    public interface ILoginService
    {
        string Login(UserDTO userDTO, string roleName);
    }
}