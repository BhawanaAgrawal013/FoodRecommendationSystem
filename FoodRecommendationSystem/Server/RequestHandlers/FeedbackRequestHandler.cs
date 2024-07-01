using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;

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
                {"FEEDBACK_LIST", HandleSendFoodReview},
                {"FEEDBACK_GIVE", HandleSendingFeedback }
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return "Invalid request.";
        }

        private string HandleSendFoodReview(string request)
        {
            var parts = request.Split('|');
            var foods = _feedbackHelper.GetMeals(parts[1]);

            string result = String.Empty;

            foreach(var food in foods)
            {
                result += ($"\nFood ID: {food.Food.Id}\tName: {food.Food.Name}");
            }

            return result;
        }

        private string HandleSendingFeedback(string request)
        {
            var parts = request.Split('|');

            ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(parts[1]);
            RatingDTO ratingDTO = JsonConvert.DeserializeObject<RatingDTO>(parts[2]);

            _feedbackHelper.AddFeedback(reviewDTO, ratingDTO);

            return "Added Feedback";
        }
    }
}
