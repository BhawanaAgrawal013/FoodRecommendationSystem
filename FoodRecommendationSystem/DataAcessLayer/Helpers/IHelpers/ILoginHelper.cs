namespace DataAcessLayer.Helpers.IHelpers
{
    public interface ILoginHelper
    {
        string LoginUser(UserDTO userDTO, string roleName);
    }
}