using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Serilog;

namespace Server.RequestHandlers
{
    public class FeedbackRequestHandler : IRequestHandler<FeedbackRequestHandler>
    {
        private readonly IFeedbackHelper _feedbackHelper;

        public FeedbackRequestHandler(IFeedbackHelper feedbackHelper)
        {
            _feedbackHelper = feedbackHelper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"FEEDBACK_LIST", GetMealsForReview},
                {"FEEDBACK_GIVE", SendFeedback }
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return String.Empty;
        }

        private string GetMealsForReview(string request)
        {
            try
            {
                var parts = request.Split('|');
                var foods = _feedbackHelper.GetMeals(parts[1]);

                string result = String.Empty;

                foreach (var food in foods)
                {
                    result += ($"\nFood ID: {food.Food.Id}\tName: {food.Food.Name}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending the food list for review {ex.Message}");
                throw new Exception($"Error sending the food list for review {ex.Message}");
            }
        }

        private string SendFeedback(string request)
        {
            try
            {
                var parts = request.Split('|');

                ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(parts[1]);
                RatingDTO ratingDTO = JsonConvert.DeserializeObject<RatingDTO>(parts[2]);

                _feedbackHelper.AddFeedback(reviewDTO, ratingDTO);

                return "Added Feedback";
            }
            catch (Exception ex)
            {
                Log.Error($"Error sending the food feedback {ex.Message}");
                throw new Exception($"Error sending the food feedback {ex.Message}");
            }
        }
    }
}
