namespace DataAcessLayer.ModelDTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; }

        public double OverallRating { get; set; }

        public int QuantityRating { get; set; }

        public int QualityRating { get; set; }

        public int AppearanceRating { get; set; }

        public int ValueForMoneyRating { get; set; }

        public UserDTO User { get; set; }
        public FoodDTO Food { get; set; }

        public static implicit operator ReviewDTO(Review review)
        {
            if (review == null) return null;

            return new ReviewDTO()
            {
                Id = review.Id,
                ReviewText = review.ReviewText,
                ReviewDate = review.ReviewDate,
                OverallRating = review.OverallRating,
                QuantityRating = review.QuantityRating,
                QualityRating = review.QualityRating,
                AppearanceRating = review.AppearanceRating,
                ValueForMoneyRating = review.ValueForMoneyRating,
                User = (UserDTO)review.User,
                Food = (FoodDTO)review.Food
            };
        }

        public static implicit operator Review(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null) return null;

            return new Review()
            {
                Id = reviewDTO.Id,
                ReviewText = reviewDTO.ReviewText,
                ReviewDate = reviewDTO.ReviewDate,
                OverallRating = reviewDTO.OverallRating,
                QuantityRating = reviewDTO.QuantityRating,
                QualityRating = reviewDTO.QualityRating,
                AppearanceRating = reviewDTO.AppearanceRating,
                ValueForMoneyRating = reviewDTO.ValueForMoneyRating,
                UserId = reviewDTO.User.Id,
                FoodId = reviewDTO.Food.Id,
                User = (User)reviewDTO.User,
                Food = (Food)reviewDTO.Food
            };
        }
    }
}
