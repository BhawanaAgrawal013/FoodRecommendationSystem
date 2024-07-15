using DataAcessLayer.ModelDTOs;
using Serilog;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DataAcessLayer.Helpers
{
    public class FeedbackHelper : IFeedbackHelper
    {
        private readonly IReviewService _reviewService;
        private readonly IRatingService _ratingService;
        private readonly IFoodService _foodService;
        private readonly ISummaryRatingService _summaryRatingService;
        private readonly IMealService _mealService;
        private readonly IUserService _user;
        private readonly IDiscardedMenuFeedbackService _discardedMenuFeedback;

        public FeedbackHelper(IRatingService ratingService, IReviewService reviewService, 
                            IFoodService foodService, ISummaryRatingService summaryRatingService,
                            IMealService mealService, IUserService user, IDiscardedMenuFeedbackService discardedMenuFeedback)
        {
            _ratingService = ratingService;
            _reviewService = reviewService;
            _foodService = foodService;
            _summaryRatingService = summaryRatingService;
            _mealService = mealService;
            _user = user;
            _discardedMenuFeedback = discardedMenuFeedback;
        }

        public void AddFeedback(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            try
            {
                var user = _user.GetAllUsers().FirstOrDefault(x => x.Email == ratingDTO.User.Email) ?? throw new ArgumentException("User not found");
                int userId = user.Id;
                reviewDTO.User = new User { Id = userId };
                reviewDTO.Food = new Food { Id = ratingDTO.Food.Id };
                ratingDTO.User = new User { Id = userId };

                var existingReview = _reviewService.GetFoodReviewByUser(userId, ratingDTO.Food.Id);
                if (existingReview == null)
                {
                    _reviewService.AddReview(reviewDTO);
                }
                else
                {
                    reviewDTO.Id = existingReview.Id;
                    _reviewService.UpdateReview(reviewDTO);
                }

                var existingRating = _ratingService.GetFoodRatingByUser(userId, ratingDTO.Food.Id);
                if (existingRating == null)
                {
                    _ratingService.AddRating(ratingDTO);
                }
                else
                {
                    ratingDTO.Id = existingRating.Id;
                    _ratingService.UpdateRating(ratingDTO);
                }

                SetSummaryRating(reviewDTO, ratingDTO);
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding feedback: {ex.Message}");
                throw new Exception("Error adding feedback", ex);
            }
        }

        public List<MealDTO> GetMeals(string classification)
        {
            try
            {
                return _mealService.GetAllMeals().Where(x => x.MealName.MealType == classification).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting meals for classification '{classification}': {ex.Message}");
                throw new Exception($"Error getting meals for classification '{classification}'", ex);
            }
        }

        public string GetSentimentSummary(int foodId)
        {
            try
            {
                var reviewTexts = _reviewService.GetAllReviews().Where(x => x.Food.Id == foodId).Select(x => x.ReviewText).ToList();
                var sentimentWordsSet = new HashSet<string>();

                foreach (var review in reviewTexts)
                {
                    var words = review.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '(', ')', '[', ']', '{', '}', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        var lowerWord = word.ToLower();
                        if (WordsDictionary.SentimentWords.ContainsKey(lowerWord))
                        {
                            sentimentWordsSet.Add(lowerWord);
                        }
                    }
                }

                return string.Join(", ", sentimentWordsSet);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting sentiment summary for food ID '{foodId}': {ex.Message}");
                throw new Exception($"Error getting sentiment summary for food ID '{foodId}'", ex);
            }
        }


        public void AddDiscardedFeedback(DiscardedMenuFeedbackDTO discardedMenuFeedbackDTO)
        {
            try
            {
                _discardedMenuFeedback.AddDiscardedMenuFeedback(discardedMenuFeedbackDTO);
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding discarded feedback: {ex.Message}");
                throw new Exception("Error adding discarded feedback", ex);
            }
        }


        private SummaryRatingDTO SetSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            try
            {
                var existingSummaryRatingDTO = _summaryRatingService.GetSummaryRatingByFoodId(reviewDTO.Food.Id);

                if (existingSummaryRatingDTO == null)
                {
                    var summaryRatingDTO = CreateSummaryRating(reviewDTO, ratingDTO);
                    summaryRatingDTO.Food = _foodService.GetFood(reviewDTO.Food.Id);
                    _summaryRatingService.AddSummaryRating(summaryRatingDTO);
                    return summaryRatingDTO;
                }
                else
                {
                    var newScore = AnalyzeSentiment(reviewDTO.ReviewText);
                    existingSummaryRatingDTO.SentimentComment = GetSentimentSummary(reviewDTO.Food.Id);
                    existingSummaryRatingDTO.SentimentScore = CalculateAverage(existingSummaryRatingDTO.SentimentScore, existingSummaryRatingDTO.NumberOfPeople, newScore);
                    existingSummaryRatingDTO.TotalQuantityRating = CalculateAverage(existingSummaryRatingDTO.TotalQuantityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QuantityRating);
                    existingSummaryRatingDTO.AverageRating = CalculateAverage(existingSummaryRatingDTO.AverageRating, existingSummaryRatingDTO.NumberOfPeople, ratingDTO.RatingValue);
                    existingSummaryRatingDTO.TotalAppearanceRating = CalculateAverage(existingSummaryRatingDTO.TotalAppearanceRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.AppearanceRating);
                    existingSummaryRatingDTO.TotalQualityRating = CalculateAverage(existingSummaryRatingDTO.TotalQualityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QualityRating);
                    existingSummaryRatingDTO.NumberOfPeople += 1;
                    existingSummaryRatingDTO.TotalValueForMoneyRating = CalculateAverage(existingSummaryRatingDTO.TotalValueForMoneyRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.ValueForMoneyRating);
                    _summaryRatingService.UpdateSummaryRating(existingSummaryRatingDTO);
                    return existingSummaryRatingDTO;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error setting summary rating: {ex.Message}");
                throw new Exception("Error setting summary rating", ex);
            }
        }


        private SummaryRatingDTO CreateSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            return new SummaryRatingDTO
            {
                SentimentScore = AnalyzeSentiment(reviewDTO.ReviewText),
                AverageRating = ratingDTO.RatingValue,
                TotalAppearanceRating = reviewDTO.AppearanceRating,
                TotalQualityRating = reviewDTO.QualityRating,
                TotalQuantityRating = reviewDTO.QuantityRating,
                TotalValueForMoneyRating = reviewDTO.ValueForMoneyRating,
                NumberOfPeople = 1,
                SentimentComment = GetSentimentSummary(reviewDTO.Food.Id)
            };
        }


        private static double CalculateAverage(double sum, double numberOfPeople, double newRating)
        {
            return ((sum * numberOfPeople) + newRating) / (numberOfPeople + 1);
        }


        private static string RemoveUnwantedWords(string text)
        {
            string normalizedText = new string(text.ToLower().Where(c => !char.IsPunctuation(c)).ToArray());
            string[] words = normalizedText.Split(' ');

            var filteredWords = words.Where(word => !WordsDictionary.Stopwords.Contains(word)).ToArray();

            return string.Join(" ", filteredWords);
        }


        private static double AnalyzeSentiment(string text)
        {
            text = RemoveUnwantedWords(text);
            string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            double totalSentimentScore = 0;

            foreach (var sentence in sentences)
            {
                string[] words = sentence.Split(' ');

                int sentenceScore = 0;
                bool negate = false;
                int intensify = 1;
                bool conjunctionFound = false;

                foreach (var word in words)
                {
                    if (WordsDictionary.Negations.Contains(word))
                    {
                        negate = true;
                    }
                    else if (WordsDictionary.Intensifiers.Contains(word))
                    {
                        intensify = 2;
                    }
                    else if (WordsDictionary.Conjunctions.Contains(word))
                    {
                        conjunctionFound = true;
                    }
                    else if (WordsDictionary.SentimentWords.ContainsKey(word))
                    {
                        int score = WordsDictionary.SentimentWords[word];
                        if (negate)
                        {
                            score = -score;
                            negate = false;
                        }
                        score *= intensify;
                        intensify = 1;
                        sentenceScore += score;
                    }
                    else
                    {
                        negate = false;
                    }
                }

                totalSentimentScore += conjunctionFound ? sentenceScore / 2 : sentenceScore;
            }

            return totalSentimentScore;
        }

    }
}