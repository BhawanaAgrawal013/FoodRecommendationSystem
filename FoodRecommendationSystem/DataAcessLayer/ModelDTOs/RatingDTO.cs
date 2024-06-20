namespace DataAcessLayer.ModelDTOs
{
    public class RatingDTO
    {
        public int Id { get; set; }

        public double RatingValue { get; set; }

        public UserDTO User { get; set; }

        public FoodDTO Food { get; set; }

        public static implicit operator RatingDTO(Rating rating)
        {
            if (rating == null) return null;

            return new RatingDTO()
            {
                Id = rating.Id,
                RatingValue = rating.RatingValue,
                User = (UserDTO)rating.User,
                Food = (FoodDTO)rating.Food,
            };
        }

        public static implicit operator Rating(RatingDTO ratingDTO)
        {
            if (ratingDTO == null) return null;

            return new Rating()
            {
                Id = ratingDTO.Id,
                RatingValue = ratingDTO.RatingValue,
                FoodId = ratingDTO.Food.Id,
                UserId = ratingDTO.User.Id
            };
        }
    }
}
