using DataAcessLayer.Repository.Repository;
using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class RatingService : IRatingService
    {
        private readonly IRepository<Rating> _ratingRepository;

        public RatingService(IRepository<Rating> ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public void AddRating(RatingDTO ratingDTO)
        {
            try
            {
                Rating rating = (Rating)ratingDTO;
                _ratingRepository.Insert(rating);
                _ratingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting rating", ex);
            }
        }

        public List<RatingDTO> GetAllRatings()
        {
            try
            {
                var ratings = _ratingRepository.GetAll();
                return ratings.Select(rating => (RatingDTO)rating).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all ratings", ex);
            }
        }

        public RatingDTO GetRating(int id)
        {
            try
            {
                var rating = _ratingRepository.GetById(id);
                return (RatingDTO)rating;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the rating {id}", ex);
            }
        }

        public RatingDTO GetFoodRatingByUser(int userId, int foodId)
        {
            try
            {
                var rating = _ratingRepository.GetAll().Where(x => x.UserId == userId && x.FoodId == foodId).FirstOrDefault();
                return (RatingDTO)rating;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the review for {foodId} by {userId}", ex);
            }
        }

        public void UpdateRating(RatingDTO ratingDTO)
        {
            try
            {
                var rating = (Rating)ratingDTO;
                _ratingRepository.Update(rating);
                _ratingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the rating", ex);
            }
        }

        public void DeleteRating(int id)
        {
            try
            {
                _ratingRepository.Delete(id);
                _ratingRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the rating {id}", ex);
            }
        }
    }

}
