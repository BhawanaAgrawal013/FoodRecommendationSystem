using DataAcessLayer.ModelDTOs;
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
        public FeedbackHelper(IRatingService ratingService, IReviewService reviewService, 
                            IFoodService foodService, ISummaryRatingService summaryRatingService, 
                            IMealService mealService, IUserService user)
        {
            _ratingService = ratingService;
            _reviewService = reviewService;
            _foodService = foodService;
            _summaryRatingService = summaryRatingService;
            _mealService = mealService;
            _user = user;
        }

        public void AddFeedback(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            var userId = _user.GetAllUsers().Where(x => x.Email == ratingDTO.User.Email).Select(x => x.Id).First();

            reviewDTO.User = new User
            {
                Id = userId
            };

            reviewDTO.Food = new Food
            {
                Id = ratingDTO.Food.Id
            };

            ratingDTO.User = new User
            {
                Id = userId
            };

            if (!_reviewService.ReviewByUserExist(userId, ratingDTO.Food.Id))
            {
                _reviewService.AddReview(reviewDTO);
            }
            else
            {
                var reviewid = _reviewService.GetFoodReviewByUser(userId, ratingDTO.Food.Id).Id;
                reviewDTO.Id = reviewid;
                _reviewService.UpdateReview(reviewDTO);
            }
            

            if(!_ratingService.RatingByUserExist(userId, ratingDTO.Food.Id))
            {
                _ratingService.AddRating(ratingDTO);
            }
            else
            {
                var ratingId = _ratingService.GetFoodRatingByUser(userId, ratingDTO.Food.Id).Id;
                ratingDTO.Id = ratingId;
                _ratingService.UpdateRating(ratingDTO);
            }

            var summaryRatingDTO = SetSummaryRating(reviewDTO, ratingDTO);
        }

        private SummaryRatingDTO SetSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
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
                existingSummaryRatingDTO.SentimentScore = CalcualteAverage(existingSummaryRatingDTO.SentimentScore, existingSummaryRatingDTO.NumberOfPeople, newScore);
                existingSummaryRatingDTO.TotalQuantityRating = CalcualteAverage(existingSummaryRatingDTO.TotalQuantityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QuantityRating);
                existingSummaryRatingDTO.AverageRating = CalcualteAverage(existingSummaryRatingDTO.AverageRating, existingSummaryRatingDTO.NumberOfPeople, ratingDTO.RatingValue);
                existingSummaryRatingDTO.TotalAppearanceRating = CalcualteAverage(existingSummaryRatingDTO.TotalAppearanceRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.AppearanceRating); ;
                existingSummaryRatingDTO.TotalQualityRating = CalcualteAverage(existingSummaryRatingDTO.TotalQualityRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.QualityRating);
                existingSummaryRatingDTO.NumberOfPeople += 1;
                existingSummaryRatingDTO.TotalValueForMoneyRating = CalcualteAverage(existingSummaryRatingDTO.TotalValueForMoneyRating, existingSummaryRatingDTO.NumberOfPeople, reviewDTO.ValueForMoneyRating);

                _summaryRatingService.UpdateSummaryRating(existingSummaryRatingDTO);

                return existingSummaryRatingDTO;
            }
        }

        private SummaryRatingDTO CreateSummaryRating(ReviewDTO reviewDTO, RatingDTO ratingDTO)
        {
            SummaryRatingDTO summaryRating = new SummaryRatingDTO
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

            return summaryRating;
        }

        private double CalcualteAverage(double sum, double numberOfPeople, double newRating)
        {
            return ( (sum * numberOfPeople) + newRating ) / (numberOfPeople + 1);
        }

        private static string RemoveUnwantedWords(string text)
        {
            string normalizedText = new string(text.ToLower().Where(c => !char.IsPunctuation(c)).ToArray());
            string[] words = normalizedText.Split(' ');

            var filteredWords = words.Where(word => !WordsDictionary.Stopwords.Contains(word)).ToArray();

            return string.Join(" ", filteredWords);
        }

        static double AnalyzeSentiment(string text)
        {
            text = RemoveUnwantedWords(text);
            string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            double totalSentimentScore = 0;

            foreach (string sentence in sentences)
            {
                string[] words = sentence.Split(' ');

                int sentenceScore = 0;
                bool negate = false;
                int intensify = 1;
                bool conjunctionFound = false;

                foreach (string word in words)
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

                if (conjunctionFound)
                {
                    totalSentimentScore += sentenceScore / 2;
                }
                else
                {
                    totalSentimentScore += sentenceScore;
                }
            }

            return totalSentimentScore;
        }

        public List<MealDTO> GetMeals(string classification)
        {
            var meals = _mealService.GetAllMeals().Where(x => x.MealName.MealType == classification).ToList();

            return meals;
        }

        public string GetSentimentSummary(int foodId)
        {
            List<string> reviewTexts = _reviewService.GetAllReviews().Where(x => x.Food.Id == foodId).Select(x => x.ReviewText).ToList();

            var result = string.Empty;

            foreach (var review in reviewTexts)
            {

                var words = review.Split(new[] { ' ', '.', ',', '!', '?', ';', ':', '-', '(', ')', '[', ']', '{', '}', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var sentimentWordsSet = new HashSet<string>();

                foreach (var word in words)
                {
                    var lowerWord = word.ToLower();
                    if (WordsDictionary.SentimentWords.ContainsKey(lowerWord))
                    {
                        sentimentWordsSet.Add(lowerWord);
                    }
                }

                result += string.Join(", ", sentimentWordsSet);
            }

            return result;
        }
    }
}