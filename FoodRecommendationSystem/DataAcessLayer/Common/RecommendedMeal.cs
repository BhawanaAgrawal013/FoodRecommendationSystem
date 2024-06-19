namespace DataAcessLayer.Common
{
    public class RecommendedMeal
    {
        public string PrimaryFoodName { get; set; }
        public MealNameDTO MealName { get; set; }

        public SummaryRatingDTO SummaryRating { get; set; }
    }
}
