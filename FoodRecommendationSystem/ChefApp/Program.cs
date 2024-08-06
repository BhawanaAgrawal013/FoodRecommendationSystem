using DataAcessLayer.ModelDTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server;

class Program
{
    private static string UserEmail { get; set; }
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
        ChefLogin(client);

        Console.WriteLine("-------------------");
        Console.WriteLine("Chef Console");
        Console.WriteLine("-------------------");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => SelectMealOptions(client) },
            { "3", () => SendNotification(client) },
            { "4", () => SelectMeal(client) },
            { "7", () => LogOut(client) },
            { "5", () => GetDiscardedMeal(client)},
            { "6", () => DiscardMeal(client) }
        };

        while (true)
        {
            Console.WriteLine("\n\n1. View Menu");
            Console.WriteLine("2. Select Meal Option");
            Console.WriteLine("3. Send Notification");
            Console.WriteLine("4. Select Meal for tomorrow");
            Console.WriteLine("5. Get Discarded Meal");
            Console.WriteLine("6. Discard a Meal");
            Console.WriteLine("7. To Exit");

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

    static void GetDiscardedMeal(SocketClient client)
    {
        client.SendMessage("DISCARD_GET");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.WriteLine("\nPress 1: To Get Feedback\nPress 2: To Discard");
        string decision = Console.ReadLine();

        Console.Write("Enter the Meal Id: ");
        string mealId = Console.ReadLine();

        client.SendMessage($"DISCARD_UPDATE|{mealId}|{decision}");

        response = client.RecieveMessage();
        Console.WriteLine(response);

        SendNotification(client);
    }

    static string GetRecommendedMeals(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        Console.Write("How many Meals do you want?");
        string numberOfMeals = Console.ReadLine();

        client.SendMessage($"MEAL_SELECT|{classification}|{numberOfMeals}");

        var response = client.RecieveMessage();
        Console.WriteLine("These are the recommended meals: ");
        Console.WriteLine(response);

        Console.WriteLine("\n\nThis is the full meanu:");

        client.SendMessage($"MENU_CLASSIFIED|{classification}");

        response = client.RecieveMessage();
        Console.WriteLine(response);

        return classification;
    }

    static void SelectMealOptions(SocketClient client)
    {
        var classification = GetRecommendedMeals(client);

        List<string> meals = new List<string>();

        Console.WriteLine("Enter 3 the meal names you want to add as an option:");
        for(int i = 1; i <= 3; i++)
        {
            Console.Write(i + " ");
            meals.Add(Console.ReadLine());        
        }

        var meal = JsonConvert.SerializeObject(meals);

        client.SendMessage($"MEAL_SEND_OPTION|{classification}|{meal}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        SendNotification(client);
    }

    static void SelectMeal(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        client.SendMessage($"MEAL_GET_OPTIONS|{classification}|{UserEmail}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.WriteLine("Enter the meal you want to make next day: ");
        string mealId = Console.ReadLine();

        client.SendMessage($"MEAL_CHOOSE|{mealId}");

        response = client.RecieveMessage();
        Console.WriteLine(response);

        SendNotification(client);
    }

    static void SendNotification(SocketClient client)
    {
        Console.WriteLine("Enter Notification: ");
        string message = Console.ReadLine();

        client.SendMessage($"NOTI_SEND|{message}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void LogOut(SocketClient client)
    {
        try
        {
            Console.Clear();
            ChefLogin(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging out: {ex.Message}");
            throw;
        }
    }

    static void ChefLogin(SocketClient client)
    {
        UserDTO user = new UserDTO();

        Console.WriteLine("Login as Chef");
        Console.WriteLine("Enter Email: ");
        user.Email = Console.ReadLine();
        Console.WriteLine("Enter Password: ");
        user.Password = Console.ReadLine();

        string json = JsonConvert.SerializeObject(user);
        client.SendMessage($"LOGIN|{json}|{UserRole.Chef.ToString()}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response == "Login Successful")
        {
            UserEmail = user.Email;
            return;
        }

        ChefLogin(client);
    }

    static string GetClassificationFromUser()
    {
        while (true)
        {
            Console.WriteLine("Enter Classification (Breakfast, Lunch, Dinner):");
            string userInput = Console.ReadLine();

            string formattedInput = userInput.Replace(" ", string.Empty);

            if (Enum.TryParse(formattedInput, true, out Classification classification) && Enum.IsDefined(typeof(Classification), classification))
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Invalid classification. Please enter one of the following: Breakfast, Thali.");
            }
        }
    }

    static void DiscardMeal(SocketClient client)
    {
        client.SendMessage("DISCARD_MENU");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.WriteLine("\nPress 1: To Get Feedback Again\nPress 2: To Discard");
        string decision = Console.ReadLine();

        Console.Write("Enter the Meal Id: ");
        string mealId = Console.ReadLine();

        client.SendMessage($"DISCARD_UPDATE|{mealId}|{decision}");

        response = client.RecieveMessage();
        Console.WriteLine(response);

        SendNotification(client);
    }
}