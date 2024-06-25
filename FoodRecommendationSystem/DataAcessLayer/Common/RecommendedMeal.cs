namespace DataAcessLayer.Common
{
    public class RecommendedMeal
    {
        public string PrimaryFoodName { get; set; }

        public int ShouldBeDiscarded { get; set; } = 0;

        public MealNameDTO MealName { get; set; }

        public SummaryRatingDTO SummaryRating { get; set; }
    }
}
