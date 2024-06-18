namespace DataAcessLayer.ModelDTOs
{
    public class FoodDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsInMainMenu { get; set; }

        public static implicit operator FoodDTO(Food food)
        {
            if (food ==null) return null;

            return new FoodDTO()
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                IsAvailable = food.IsAvailable,
                IsInMainMenu = food.IsInMainMenu
            };
        }

        public static implicit operator Food(FoodDTO foodDTO)
        {
            if (foodDTO == null) return null;

            return new Food()
            {
                Id = foodDTO.Id,
                Name = foodDTO.Name,
                Description = foodDTO.Description,
                Price = foodDTO.Price,
                IsAvailable = foodDTO.IsAvailable,
                IsInMainMenu= foodDTO.IsInMainMenu
            };
        }
    }
}
