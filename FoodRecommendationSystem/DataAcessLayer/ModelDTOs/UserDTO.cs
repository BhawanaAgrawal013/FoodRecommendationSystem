namespace DataAcessLayer.ModelDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string EmpId { get; set; }

        public RoleDTO Role { get; set; }

        public static implicit operator UserDTO(User user)
        {
            if (user == null) return null;

            return new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                EmpId = user.EmpId,
                Role = (RoleDTO)user.Role,
                Password = user.Password
            };
        }

        public static implicit operator User(UserDTO userDTO)
        {
            if (userDTO == null) return null;

            return new User()
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                Name = userDTO.Name,
                Gender = userDTO.Gender,
                EmpId = userDTO.EmpId,
                RoleId = userDTO.Role.Id,
                Role = (Role)userDTO.Role,
                Password = userDTO.Password
            };
        }
    }
}
