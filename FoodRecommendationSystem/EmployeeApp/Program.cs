using DataAcessLayer;
using DataAcessLayer.Entity;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Server;

class Program
{
    static void Main(string[] args)
    {
        System.Threading.Thread.Sleep(4000);

        var client = new SocketClient("127.0.0.1", 5000);

        EmployeeLogin(client);

        Console.WriteLine("Employee Console");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => GetNotification(client)},
            { "3", () => VoteMealOptions(client)},
            { "4", () => GiveFeedback(client) },
            { "5", () => CheckDiscardedMenu(client)},
            { "6", () => LogOUt(client) }
        };

        while (true)
        {
            Console.WriteLine("1. View Menu");
            Console.WriteLine("2. Recieve Notification");
            Console.WriteLine("3. Vote for Meal Options");
            Console.WriteLine("4. Give Feedback");
            Console.WriteLine("5. Check Discarded Menu");
            Console.WriteLine("6. To Log Out");

            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            if (menuActions.TryGetValue(option, out var action))
            {
                action();
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    static void ViewMenu(SocketClient client)
    {
        client.SendMessage("MENU_GET");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

    }

    static void GetNotification(SocketClient client)
    {
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        client.SendMessage($"NOTI_RECIEVE|{email}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void VoteMealOptions(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        Console.WriteLine("Enter user email: ");
        string email = Console.ReadLine();

        client.SendMessage($"MEAL_GETOPTIONS|{classification}|{email}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.Write("Enter the choosen meal: ");
        string id = Console.ReadLine();

        client.SendMessage($"MEAL_VOTE|{id}");
    }

    static void CheckDiscardedMenu(SocketClient client)
    {
        client.SendMessage("DISCARD_MENU");
        var parts = client.RecieveMessage().Split('|');
        string mealName = parts[1];   
        Console.WriteLine($"We are trying to improve your experience with {mealName}. Please provide your feedback and help us");

        DiscardedMenuFeedbackDTO discardedMenuFeedbackDTO = new DiscardedMenuFeedbackDTO();

        Console.WriteLine($"Q1. What didn’t you like about {mealName}?");
        discardedMenuFeedbackDTO.DislikeText = Console.ReadLine();

        Console.WriteLine($"Q2. How would you like {mealName} to taste?");
        discardedMenuFeedbackDTO.LikeText = Console.ReadLine();

        Console.WriteLine($"Q3. Share your mom’s recipe.");
        discardedMenuFeedbackDTO.Recipie = Console.ReadLine();

        discardedMenuFeedbackDTO.DiscardedMenuId = Convert.ToInt32(parts[0]);

        string json = JsonConvert.SerializeObject(discardedMenuFeedbackDTO);

        client.SendMessage($"DISCARD_FEEDBACK|{json}");
    }

    static void GiveFeedback(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        client.SendMessage($"FEEDBACK_LIST|{classification}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.Write("Enter the food Id you want to review: ");
        int foodId = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter the user email: ");
        string email = Console.ReadLine();

        RatingDTO ratingDTO = new RatingDTO()
        {
            Food = new FoodDTO
            {
                Id = foodId,
            },
            User = new UserDTO
            {
                Email = email,
            },
            RatingValue = GetRating("Rating")
        };

        var review = GetReviewDetails();

        var jsonReview = JsonConvert.SerializeObject(review);
        var jsonRating = JsonConvert.SerializeObject(ratingDTO);

        client.SendMessage($"FEEDBACK_GIVE|{jsonReview}|{jsonRating}");

        response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void LogOUt(SocketClient client)
    {
        Console.Clear();
        EmployeeLogin(client);
    }

    static void EmployeeLogin(SocketClient client)
    {
        UserDTO user = new UserDTO();

        Console.WriteLine("Login as Employee");
        Console.WriteLine("Enter Email: ");
        user.Email = Console.ReadLine();
        Console.WriteLine("Enter Password: ");
        user.Password = Console.ReadLine();

        string json = JsonConvert.SerializeObject(user);
        client.SendMessage($"LOGIN|{json}|Employee");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if(response == "Login Sucessfull")
        {
            return;
        }

        EmployeeLogin(client);
    }

    static ReviewDTO GetReviewDetails()
    {
        ReviewDTO review = new ReviewDTO();
        Console.WriteLine("Enter your review text:");
        review.ReviewText = Console.ReadLine();

        review.ReviewDate = DateTime.Now;

        review.QuantityRating = GetRating("Quantity Rating");

        review.QualityRating = GetRating("Quality Rating");

        review.AppearanceRating = GetRating("Appearance Rating");

        review.ValueForMoneyRating = GetRating("Value for Money Rating");

        return review;
    }

    static int GetRating(string ratingType)
    {
        int rating;
        while (true)
        {
            Console.WriteLine($"Enter {ratingType} (1 to 5):");
            if (int.TryParse(Console.ReadLine(), out rating) && rating >= 1 && rating <= 5)
            {
                break;
            }
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
        }
        return rating;
    }

    static string GetClassificationFromUser()
    {
        while (true)
        {
            Console.WriteLine("Enter Classification (Breakfast, Beverage, Snacks, Thali, Appetizer, Healthy Snack):");
            string userInput = Console.ReadLine();

            string formattedInput = userInput.Replace(" ", string.Empty);

            if (Enum.TryParse(formattedInput, true, out Classification classification) && Enum.IsDefined(typeof(Classification), classification))
            {
                return userInput; 
            }
            else
            {
                Console.WriteLine("Invalid classification. Please enter one of the following: Breakfast, Beverage, Snacks, Thali, Appetizer, Healthy Snack.");
            }
        }
    }
}