using DataAcessLayer.ModelDTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Server;

class Program
{
    static void Main(string[] args)
    {
        System.Threading.Thread.Sleep(4000);

        var client = new SocketClient("127.0.0.1", 5000);

        ChefLogin(client);

        Console.WriteLine("Chef Console");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => SelectMealOptions(client) },
            { "3", () => SendNotification(client) },
            { "4", () => SelectMeal(client) },
            { "5", () => LogOut(client) }
        };

        while (true)
        {
            Console.WriteLine("1. View Menu");
            Console.WriteLine("2. Select Meal Option");
            Console.WriteLine("3. Send Notification");
            Console.WriteLine("4. Select Meal for tomorrow");
            Console.WriteLine("5. To Exit");

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

        client.SendMessage("MENU_GET");

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

        client.SendMessage($"MEAL_OPTION|{classification}|{meal}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        SendNotification(client);
    }

    static void SelectMeal(SocketClient client)
    {
        string classification = GetClassificationFromUser();

        client.SendMessage($"MEAL_GETOPTIONS|{classification}");

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
        Console.Clear();
        ChefLogin(client);
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
        client.SendMessage($"LOGIN|{json}|Chef");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response == "Login Sucessfull")
        {
            return;
        }

        ChefLogin(client);
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