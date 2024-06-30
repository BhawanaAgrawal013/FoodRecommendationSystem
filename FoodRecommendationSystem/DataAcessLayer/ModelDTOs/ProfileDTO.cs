namespace DataAcessLayer.ModelDTOs
{
    public class ProfileDTO
    {
        public int Id { get; set; }

        public string DietType { get; set; }

        public string SpiceLevel { get; set; }

        public string CuisinePreference { get; set; }

        public bool IsSweet { get; set; }

        public UserDTO User { get; set; }

        public static implicit operator ProfileDTO(Profile profile)
        {
            if (profile == null) return null;

            return new ProfileDTO()
            {
                Id = profile.Id,
                DietType = profile.DietType,
                SpiceLevel = profile.SpiceLevel,
                CuisinePreference = profile.CuisinePreference,
                IsSweet = profile.IsSweet,
                User = (UserDTO)profile.User
            };
        }

        public static implicit operator Profile(ProfileDTO profileDTO)
        {
            if (profileDTO == null) return null;

            return new Profile()
            {
                Id = profileDTO.Id,
                DietType = profileDTO.DietType,
                SpiceLevel = profileDTO.SpiceLevel,
                CuisinePreference = profileDTO.CuisinePreference,
                IsSweet = profileDTO.IsSweet,
                User = (User)profileDTO.User
            };
        }
    }
}
