﻿using DataAcessLayer.ModelDTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server;

class Program
{
    private static string? UserEmail { get; set; }

    static void Main(string[] args)
    {
        var basepath = DataAcessLayer.Common.Constant.basePath;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basepath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var port = configuration.GetValue<int>("SocketServer:Port");

        SocketClient client = new SocketClient("127.0.0.1", port);

        CheckClient(client);
        EmployeeLogin(client);

        Console.WriteLine("-------------------");
        Console.WriteLine("Employee Console");
        Console.WriteLine("-------------------");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => GetNotification(client)},
            { "3", () => VoteMealOptions(client)},
            { "4", () => GiveFeedback(client) },
            { "5", () => CheckDiscardedMenu(client)},
            { "6", () => LogOut(client) }
        };

        while (true)
        {
            Console.WriteLine("\n\n1. View Menu");
            Console.WriteLine("2. Receive Notification");
            Console.WriteLine("3. Vote for Meal Options");
            Console.WriteLine("4. Give Feedback");
            Console.WriteLine("5. Check Discarded Menu");
            Console.WriteLine("6. Log Out");

            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            if (menuActions.TryGetValue(option, out var action))
            {
                try
                {
                    CheckClient(client);
                    action();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    private static void CheckClient(SocketClient client)
    {
        if (!client.isConnected)
        {
            Console.WriteLine("Socket connection is abandoned. Exiting the application.");

            Console.ReadLine();
            Environment.Exit(0);
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
        client.SendMessage($"NOTI_RECEIVE|{UserEmail}");
        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void VoteMealOptions(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        client.SendMessage($"MEAL_GET_OPTIONS|{classification}|{UserEmail}");
        var response = client.RecieveMessage();
        Console.WriteLine(response);

        string id = PromptInput.StringValues("Enter the chosen meal: ");
        client.SendMessage($"MEAL_VOTE|{id}");
        response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void CheckDiscardedMenu(SocketClient client)
    {
        client.SendMessage("DISCARD_MENU");
        var parts = client.RecieveMessage().Split('|');
        string mealName = parts[1];
        Console.WriteLine($"We are trying to improve your experience with {mealName}. Please provide your feedback and help us");

        DiscardedMenuFeedbackDTO discardedMenuFeedbackDTO = new DiscardedMenuFeedbackDTO();
        discardedMenuFeedbackDTO.DislikeText = PromptInput.StringValues($"Q1. What didn’t you like about {mealName}? ");
        discardedMenuFeedbackDTO.LikeText = PromptInput.StringValues($"Q2. How would you like {mealName} to taste? ");
        discardedMenuFeedbackDTO.Recipie = PromptInput.StringValues($"Q3. Share your mom’s recipe. ");
        discardedMenuFeedbackDTO.DiscardedMenuId = Convert.ToInt32(parts[0]);

        string json = JsonConvert.SerializeObject(discardedMenuFeedbackDTO);
        client.SendMessage($"DISCARD_FEEDBACK|{json}");
        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void GiveFeedback(SocketClient client)
    {
        string classification = GetClassificationFromUser();
        client.SendMessage($"FEEDBACK_LIST|{classification}");
        var response = client.RecieveMessage();
        Console.WriteLine(response);

        int foodId = PromptInput.IntValues("Enter the food Id you want to review: ");

        RatingDTO ratingDTO = new RatingDTO()
        {
            Food = new FoodDTO { Id = foodId },
            User = new UserDTO { Email = UserEmail },
            RatingValue = GetRating("Rating")
        };

        var review = GetReviewDetails();
        var jsonReview = JsonConvert.SerializeObject(review);
        var jsonRating = JsonConvert.SerializeObject(ratingDTO);
        client.SendMessage($"FEEDBACK_GIVE|{jsonReview}|{jsonRating}");

        response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void LogOut(SocketClient client)
    {
        try
        {
            Console.Clear();
            EmployeeLogin(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging out: {ex.Message}");
            throw;
        }
    }

    static void EmployeeLogin(SocketClient client)
    {
        UserDTO user = new UserDTO();
        Console.WriteLine("Login as Employee");
        Console.WriteLine("Enter Email: ");
        user.Email = Console.ReadLine();
        user.Password = PromptInput.StringValues("Enter Password: ");

        string json = JsonConvert.SerializeObject(user);
        client.SendMessage($"LOGIN|{json}|{UserRole.Employee.ToString()}");
        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response == "Login Successful")
        {
            UserEmail = user.Email;
            return;
        }

        EmployeeLogin(client);
    }

    static ReviewDTO GetReviewDetails()
    {
        try
        {
            ReviewDTO review = new ReviewDTO();
            review.ReviewText = PromptInput.StringValues("Enter your review text: ");
            review.ReviewDate = DateTime.Now;
            review.QuantityRating = GetRating("Quantity Rating");
            review.QualityRating = GetRating("Quality Rating");
            review.AppearanceRating = GetRating("Appearance Rating");
            review.ValueForMoneyRating = GetRating("Value for Money Rating");
            return review;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while giving review: {ex.Message}");
            throw;
        }
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
            string userInput = PromptInput.StringValues("Enter Classification (Breakfast, Lunch, Dinner): ");
            string formattedInput = userInput.Replace(" ", string.Empty);

            if (Enum.TryParse(formattedInput, true, out Classification classification) && Enum.IsDefined(typeof(Classification), classification))
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Invalid classification. Please enter one of the following: Breakfast, Lunch, Dinner.");
            }
        }
    }
}
