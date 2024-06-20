namespace DataAcessLayer.ModelDTOs
{
    public class SummaryRatingDTO
    {
        public int Id { get; set; }

        public double SentimentScore { get; set; }

        public double AverageRating { get; set; }

        public double TotalAppearanceRating { get; set; }

        public double TotalQuantityRating { get; set; }

        public double TotalQualityRating { get; set; }

        public double TotalValueForMoneyRating { get; set; }

        public int NumberOfPeople { get; set; }

        public FoodDTO Food { get; set; }

        public static implicit operator SummaryRatingDTO(SummaryRating summaryRating)
        {
            if (summaryRating == null) return null;

            return new SummaryRatingDTO()
            {
                Id = summaryRating.Id,
                SentimentScore = summaryRating.SentimentScore,
                AverageRating = summaryRating.AverageRating,
                TotalAppearanceRating = summaryRating.TotalAppearanceRating,
                TotalQualityRating = summaryRating.TotalQualityRating,
                TotalQuantityRating = summaryRating.TotalQuantityRating,
                TotalValueForMoneyRating = summaryRating.TotalValueForMoneyRating,
                NumberOfPeople = summaryRating.NumberOfPeople,
                Food = (FoodDTO)summaryRating.Food
            };
        }

        public static implicit operator SummaryRating(SummaryRatingDTO summaryRatingDTO)
        {
            if (summaryRatingDTO == null) return null;

            return new SummaryRating()
            {
                Id = summaryRatingDTO.Id,
                SentimentScore = summaryRatingDTO.SentimentScore,
                AverageRating = summaryRatingDTO.AverageRating,
                TotalAppearanceRating = summaryRatingDTO.TotalAppearanceRating,
                TotalQualityRating = summaryRatingDTO.TotalQualityRating,
                TotalQuantityRating = summaryRatingDTO.TotalQuantityRating,
                TotalValueForMoneyRating = summaryRatingDTO.TotalValueForMoneyRating,
                NumberOfPeople= summaryRatingDTO.NumberOfPeople,
                FoodId = summaryRatingDTO.Food.Id
            };
        }
    }
}
