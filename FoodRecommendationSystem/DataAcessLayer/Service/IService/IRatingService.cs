namespace DataAcessLayer.Service.IService
{
    public interface IRatingService
    {
        void AddRating(RatingDTO ratingDTO);
        void DeleteRating(int id);
        List<RatingDTO> GetAllRatings();
        RatingDTO GetRating(int id);
        void UpdateRating(RatingDTO ratingDTO);
        RatingDTO GetFoodRatingByUser(int userId, int foodId);
    }
}