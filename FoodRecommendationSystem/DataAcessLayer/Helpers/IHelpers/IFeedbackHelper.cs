namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IFeedbackHelper
    {
        void AddFeedback(ReviewDTO reviewDTO, RatingDTO ratingDTO);

        List<MealDTO> GetMeals(string classification);
    }
}
