using DataAcessLayer.ModelDTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server;

class Program
{
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
        AdminLogin(client);

        Console.WriteLine("-------------------");
        Console.WriteLine("Admin Console");
        Console.WriteLine("-------------------");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => AddMenuItem(client) },
            { "3", () => UpdateMenuItem(client) },
            { "4", () => DeleteMenuItem(client) },
            { "5", () => SendNotification(client) },
            { "6", () => LogOut(client) }
        };

        while (true)
        {
            Console.WriteLine("\n\n1. View Menu");
            Console.WriteLine("2. Add Menu Item");
            Console.WriteLine("3. Update Menu Item");
            Console.WriteLine("4. Delete Menu Item");
            Console.WriteLine("5. Send Notification");
            Console.WriteLine("6. To Exit");

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

    static void AddMenuItem(SocketClient client)
    {
        MealNameDTO mealName = new MealNameDTO();

        mealName.MealName = PromptInput.StringValues("Enter Name: ");
        mealName.MealType = PromptInput.StringValues("Enter Type: ");
        mealName.CuisinePreference = PromptInput.StringValues("Enter Cuisine (South Indian/North Indian/Other): ", new List<string> { "South Indian", "North Indian", "Other" });
        mealName.DietType = PromptInput.StringValues("Enter Diet Type (Veg/Non-Veg/Egg): ", new List<string> { "Veg", "Non-Veg", "Egg" });
        mealName.IsSweet = PromptInput.BoolValues("Is it Sweet? (Y/N): ");
        mealName.SpiceLevel = PromptInput.StringValues("Enter Spice Level (Low/Medium/High): ", new List<string> { "Low", "Medium", "High" });

        string jsonMealName = JsonConvert.SerializeObject(mealName);
        client.SendMessage($"MENU_ADD|{jsonMealName}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void UpdateMenuItem(SocketClient client)
    {
        int mealNameId = PromptInput.IntValues("Enter the Menu Name Id that you want to update: ");

        MealNameDTO mealName = new MealNameDTO();
        mealName.MealNameId = mealNameId;
        mealName.MealName = PromptInput.StringValues("Enter Name: ");
        mealName.MealType = PromptInput.StringValues("Enter Type: ");
        mealName.CuisinePreference = PromptInput.StringValues("Enter Cuisine (South Indian/North Indian/Other): ", new List<string> { "South Indian", "North Indian", "Other" });
        mealName.DietType = PromptInput.StringValues("Enter Diet Type (Veg/Non-Veg/Egg): ", new List<string> { "Veg", "Non-Veg", "Egg" });
        mealName.IsSweet = PromptInput.BoolValues("Is it Sweet? (Y/N): ");
        mealName.SpiceLevel = PromptInput.StringValues("Enter Spice Level (Low/Medium/High): ", new List<string> { "Low", "Medium", "High" });

        string jsonMealName = JsonConvert.SerializeObject(mealName);
        client.SendMessage($"MENU_UPDATE|{jsonMealName}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void DeleteMenuItem(SocketClient client)
    {
        int mealNameId = PromptInput.IntValues("Enter ID: ");
        client.SendMessage($"MENU_DELETE|{mealNameId}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void SendNotification(SocketClient client)
    {
        string message = PromptInput.StringValues("Enter Notification: ");
        client.SendMessage($"NOTI_SEND|{message}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);
    }

    static void LogOut(SocketClient client)
    {
        try
        {
            Console.Clear();
            AdminLogin(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging out: {ex.Message}");
            throw;
        }
    }

    static void AdminLogin(SocketClient client)
    {
        UserDTO user = new UserDTO();

        Console.WriteLine("Login as Admin");
        Console.WriteLine("Enter Email: ");
        user.Email = Console.ReadLine();
        user.Password = PromptInput.StringValues("Enter Password: ");

        string json = JsonConvert.SerializeObject(user);
        client.SendMessage($"LOGIN|{json}|{UserRole.Admin.ToString()}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response != "Login Successful")
        {
            AdminLogin(client);
        }
    }
}
