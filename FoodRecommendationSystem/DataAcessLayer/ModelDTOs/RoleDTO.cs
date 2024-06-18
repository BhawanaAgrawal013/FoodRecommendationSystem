namespace DataAcessLayer.ModelDTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public static implicit operator RoleDTO(Role role)
        {
            if (role == null) return null;

            return new RoleDTO()
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }

        public static implicit operator Role(RoleDTO roleDTO)
        {
            if (roleDTO == null) return null;

            return new Role()
            {
                Id = roleDTO.Id,
                RoleName = roleDTO.RoleName
            };
        }
    }
}
