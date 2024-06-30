namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IFeedbackHelper
    {
        void AddFeedback(ReviewDTO reviewDTO, RatingDTO ratingDTO);

        void AddDiscardedFeedback(DiscardedMenuFeedbackDTO discardedMenuFeedbackDTO);
        
        List<MealDTO> GetMeals(string classification);
    }
}
