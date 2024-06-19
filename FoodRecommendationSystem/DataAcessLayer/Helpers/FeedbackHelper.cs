namespace DataAcessLayer.Helpers
{
    public class FeedbackHelper : IFeedbackHelper
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
            else
            {
                _reviewService.UpdateReview(reviewDTO);
            }
            
            var rating = _ratingService.GetFoodRatingByUser(ratingDTO.User.Id, ratingDTO.Food.Id);

            if(rating == null)
            {
                _ratingService.AddRating(ratingDTO);
            }
            else
            {
                _ratingService.UpdateRating(ratingDTO);
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
                var newScore = AnalyzeSentiment(reviewDTO.ReviewText);
                existingSummaryRatingDTO.SentimentScore = CalcualteAverage(existingSummaryRatingDTO.SentimentScore, existingSummaryRatingDTO.NumberOfPeople, newScore);
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
                SentimentScore = AnalyzeSentiment(reviewDTO.ReviewText), 
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
    }
}