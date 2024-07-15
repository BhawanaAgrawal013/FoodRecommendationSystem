namespace DataAcessLayer.Service.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepository;
        public ReviewService(IRepository<Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        
        public void AddReview(ReviewDTO reviewDTO)
        {
            try
            {
                Review review = (Review)reviewDTO;
                review.OverallRating = (review.QuantityRating + review.QualityRating + review.ValueForMoneyRating + review.AppearanceRating) / 4;

                _reviewRepository.Insert(review);
                _reviewRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting review", ex);
            }
        }

        public List<ReviewDTO> GetAllReviews()
        {
            try
            {
                var reviews = _reviewRepository.GetAll();
                return reviews.Select(review => (ReviewDTO)review).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all reviews", ex);
            }
        }

        public ReviewDTO GetReview(int id)
        {
            try
            {
                var review = _reviewRepository.GetById(id);
                return (ReviewDTO)review;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the review {id}", ex);
            }
        }

        public bool ReviewByUserExist(int userId, int foodId)
        {
            try
            {
                var review = _reviewRepository.GetAll().FirstOrDefault(x => x.UserId == userId && x.FoodId == foodId);
                return (review != null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if review exists for {foodId} by {userId}", ex);
            }
        }

        public ReviewDTO GetFoodReviewByUser(int userId, int foodId)
        {
            try
            {
                var review = _reviewRepository.GetAll().FirstOrDefault(x => x.UserId == userId && x.FoodId == foodId);
                return (ReviewDTO)review;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the review for {foodId} by {userId}", ex);
            }
        }

        public void UpdateReview(ReviewDTO reviewDTO)
        {
            try
            {
                Review review = (Review)reviewDTO;
                review.OverallRating = (review.QuantityRating + review.QualityRating + review.ValueForMoneyRating + review.AppearanceRating) / 4;

                _reviewRepository.Update(review);
                _reviewRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the review", ex);
            }
        }

        public void DeleteReview(int id)
        {
            try
            {
                _reviewRepository.Delete(id);
                _reviewRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the review {id}", ex);
            }
        }

    }

}
