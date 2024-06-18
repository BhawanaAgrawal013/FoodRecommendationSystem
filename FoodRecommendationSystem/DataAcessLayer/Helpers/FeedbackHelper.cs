using DataAcessLayer.Entity;
using DataAcessLayer.ModelDTOs;
using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Helpers
{
    public class FeedbackHelper
    {
        private readonly IReviewService _reviewService;
        private readonly IRatingService _ratingService;
        private readonly IFoodService _foodService;
        private readonly ISummaryRatingService _summaryRatingService;
        public FeedbackHelper(IRatingService ratingService, IReviewService reviewService, 
                            IFoodService foodService, ISummaryRatingService summaryRatingService)
        {
            _ratingService = ratingService;
            _reviewService = reviewService;
            _foodService = foodService;
            _summaryRatingService = summaryRatingService;
        }

        public void AddFeedback(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            var review = _reviewService.GetFoodReviewByUser(reviewDTO.User.Id, reviewDTO.Food.Id);

            if (review == null)
            {
                _reviewService.AddReview(reviewDTO);
            }
            
            var rating = _ratingService.GetFoodRatingByUser(ratingDTO.User.Id, ratingDTO.Food.Id);

            if(rating == null)
            {
                _ratingService.AddRating(ratingDTO);
            }

            var summaryRatingDTO = SetSummaryRating(reviewDTO, ratingDTO);
            _summaryRatingService.AddSummaryRating(summaryRatingDTO);
        }

        private SummaryRatingDTO SetSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            var existingSummaryRatingDTO = _summaryRatingService.GetSummaryRatingByFoodId(reviewDTO.Food.Id);

            if (existingSummaryRatingDTO == null)
            {
                var summaryRatingDTO = CreateSummaryRating(reviewDTO, ratingDTO);
                summaryRatingDTO.Food = _foodService.GetFood(reviewDTO.Food.Id);

                return summaryRatingDTO;
            }
            else
            {
                existingSummaryRatingDTO.ReviewSummary = "";  //to be added a function
                existingSummaryRatingDTO.TotalQuantityRating = CalcualteAverage(existingSummaryRatingDTO.TotalQuantityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QuantityRating);
                existingSummaryRatingDTO.AverageRating = CalcualteAverage(existingSummaryRatingDTO.AverageRating, existingSummaryRatingDTO.NumberOfPeople, ratingDTO.RatingValue);
                existingSummaryRatingDTO.TotalAppearanceRating = CalcualteAverage(existingSummaryRatingDTO.TotalAppearanceRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.AppearanceRating); ;
                existingSummaryRatingDTO.TotalQualityRating = CalcualteAverage(existingSummaryRatingDTO.TotalQualityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QualityRating);
                existingSummaryRatingDTO.NumberOfPeople += 1;
                existingSummaryRatingDTO.TotalValueForMoneyRating = CalcualteAverage(existingSummaryRatingDTO.TotalValueForMoneyRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.ValueForMoneyRating);

                return existingSummaryRatingDTO;
            }
        }

        private SummaryRatingDTO CreateSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            SummaryRatingDTO summaryRating = new SummaryRatingDTO
            {
                ReviewSummary = "",  //to be added a function
                AverageRating = ratingDTO.RatingValue,
                TotalAppearanceRating = reviewDTO.AppearanceRating,
                TotalQualityRating = reviewDTO.QualityRating,
                TotalQuantityRating = reviewDTO.QuantityRating,
                TotalValueForMoneyRating = reviewDTO.ValueForMoneyRating,
                NumberOfPeople = 1
            };

            return summaryRating;
        }

        private double CalcualteAverage(double sum, double numberOfPeople, double newRating)
        {
            return ( (sum * numberOfPeople) + newRating ) / (numberOfPeople + 1);
        }
    }
}