namespace DataAcessLayer.Service.IService
{
    public interface IReviewService
    {
        void AddReview(ReviewDTO reviewDTO);
        void DeleteReview(int id);
        List<ReviewDTO> GetAllReviews();
        ReviewDTO GetReview(int id);
        void UpdateReview(ReviewDTO reviewDTO);
        bool ReviewByUserExist(int userId, int foodId);
        ReviewDTO GetFoodReviewByUser(int userId, int foodId);
    }
}