namespace DataAcessLayer.Service.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> _profileRepository;

        public ProfileService(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public void AddProfile(ProfileDTO profileDTO)
        {
            try
            {
                Profile profile = (Profile)profileDTO;
                _profileRepository.Insert(profile);
                _profileRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting profile", ex);
            }
        }

        public List<ProfileDTO> GetAllProfiles()
        {
            try
            {
                var profiles = _profileRepository.GetAll();
                return profiles.Select(profile => (ProfileDTO)profile).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all the profiles", ex);
            }
        }

        public ProfileDTO GetUserProfile(int id)
        {
            try
            {
                var profile = _profileRepository.GetById(id);
                return (ProfileDTO)profile;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the profile {id}", ex);
            }
        }

        public void UpdateProfile(ProfileDTO profileDTO)
        {
            try
            {
                var profile = (Profile)profileDTO;
                _profileRepository.Update(profile);
                _profileRepository.Save();
            }
            catch
            {
                throw new Exception("Error updating the Profile");
            }
        }

        public void DeleteProfile(int id)
        {
            try
            {
                _profileRepository.Delete(id);
                _profileRepository.Save();
            }
            catch
            {
                throw new Exception("Error deleting the Profile");
            }
        }
    }
}
