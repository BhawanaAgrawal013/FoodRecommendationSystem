namespace DataAcessLayer.ModelDTOs
{
    public class DiscardedMenuDTO
    {
        public int Id { get; set; }

        public bool IsDiscarded { get; set; }

        public int MealNameId { get; set; }

        public MealNameDTO MealName { get; set; }

        public static implicit operator DiscardedMenuDTO(DiscardedMenu discardedMenu)
        {
            if (discardedMenu == null) return null;

            return new DiscardedMenuDTO()
            {
                Id = discardedMenu.Id,
                IsDiscarded = discardedMenu.IsDiscarded,
                MealNameId = discardedMenu.MealNameId,
                MealName = discardedMenu.MealName
            };
        }

        public static implicit operator DiscardedMenu(DiscardedMenuDTO discardedMenuDTO)
        {
            if (discardedMenuDTO == null) return null;

            return new DiscardedMenu()
            {
                Id = discardedMenuDTO.Id,
                IsDiscarded = discardedMenuDTO.IsDiscarded,
                MealNameId = discardedMenuDTO.MealNameId
            };
        }
    }
}
