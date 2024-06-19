using DataAcessLayer.ModelDTOs;
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
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Select Meal Option");
        Console.WriteLine("3. Send Notification");
        Console.WriteLine("5. To Exit");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => SelectMealOptions(client) },
            { "3", () => SendNotification(client) },
            { "5", () => ExitProgram() }
        };

        while (true)
        {
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

    static void SelectMealOptions(SocketClient client)
    {
        Console.Write("Enter the Classification (Breakfast/Lunch/Dinner): ");
        string classification = Console.ReadLine();

        client.SendMessage($"MEAL_SELECT|{classification}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.WriteLine("\n\nThis is the full meanu:");

        client.SendMessage("MENU_GET");

        response = client.RecieveMessage();
        Console.WriteLine(response);

        List<string> meals = new List<string>();

        Console.WriteLine("Enter 3 the meal names you want to add as an option:");
        for(int i = 1; i <= 3; i++)
        {
            Console.Write(i + " ");
            meals.Add(Console.ReadLine());        
        }

        var meal = JsonConvert.SerializeObject(meals);

        client.SendMessage($"MEAL_OPTION|{classification}|{meal}");

        response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void SendNotification(SocketClient client)
    {
        Console.WriteLine("Enter Notification: ");
        string message = Console.ReadLine();

        client.SendMessage($"NOTI_SEND|{message}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void ExitProgram()
    {
        Environment.Exit(0);
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
        client.SendMessage($"LOGIN|{json}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response == "Login Sucessfull")
        {
            return;
        }

        ChefLogin(client);
    }
}