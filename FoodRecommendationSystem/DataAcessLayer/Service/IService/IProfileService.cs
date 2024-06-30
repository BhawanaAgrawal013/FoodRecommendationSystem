namespace DataAcessLayer.Service.IService
{
    public interface IProfileService
    {
        void AddProfile(ProfileDTO profileDTO);
        void DeleteProfile(int id);
        List<ProfileDTO> GetAllProfiles();
        ProfileDTO GetUserProfile(int id);
        void UpdateProfile(ProfileDTO profileDTO);
    }
}