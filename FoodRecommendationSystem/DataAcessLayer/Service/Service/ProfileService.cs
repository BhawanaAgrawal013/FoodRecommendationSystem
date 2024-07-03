using Serilog;

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
                Log.Error($"Error adding profile: {ex.Message}");
                throw new Exception("Error adding profile", ex);
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
                Log.Error($"Error getting all profiles: {ex.Message}");
                throw new Exception("Error getting all profiles", ex);
            }
        }

        public ProfileDTO GetUserProfile(int id)
        {
            try
            {
                var profile = _profileRepository.GetById(id);
                if (profile == null)
                {
                    throw new Exception($"Profile with id {id} not found");
                }
                return (ProfileDTO)profile;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting profile with id {id}: {ex.Message}");
                throw new Exception($"Error getting profile with id {id}", ex);
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
            catch (Exception ex)
            {
                Log.Error($"Error updating profile: {ex.Message}");
                throw new Exception("Error updating profile", ex);
            }
        }

        public void DeleteProfile(int id)
        {
            try
            {
                _profileRepository.Delete(id);
                _profileRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting profile with id {id}: {ex.Message}");
                throw new Exception($"Error deleting profile with id {id}", ex);
            }
        }

    }
}
