using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Server;

class Program
{
    static void Main(string[] args)
    {
        var client = new SocketClient("127.0.0.1", 5000);

        AdminLogin(client);

        Console.WriteLine("Admin Console");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => AddMenuItem(client) },
            { "3", () => UpdateMenuItem(client) },
            { "4", () => DeleteMenuItem(client) },
            { "5", () => LogOut(client) }
        };

        while (true)
        {
            Console.WriteLine("\n\n1. View Menu");
            Console.WriteLine("2. Add Menu Item");
            Console.WriteLine("3. Update Menu Item");
            Console.WriteLine("4. Delete Menu Item");
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

    static void AddMenuItem(SocketClient client)
    {
        MealNameDTO mealName = new MealNameDTO();
        Console.Write("Enter Name: ");
        mealName.MealName = Console.ReadLine();
        Console.Write("Enter Type: ");
        mealName.MealType = Console.ReadLine(); 
        
        string jsonMealName = JsonConvert.SerializeObject(mealName);
        client.SendMessage($"MENU_ADD|{jsonMealName}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

    }

    static void UpdateMenuItem(SocketClient client)
    {
        Console.WriteLine("Enter the Menu Name Id that you want to udpate: ");
        string mealNameId = Console.ReadLine();

        MealNameDTO mealName = new MealNameDTO();
        mealName.MealNameId = Convert.ToInt32(mealNameId);
        Console.Write("Enter Name: ");
        mealName.MealName = Console.ReadLine();
        Console.Write("Enter Type: ");
        mealName.MealType = Console.ReadLine();

        string jsonMealName = JsonConvert.SerializeObject(mealName);
        client.SendMessage($"MENU_UPDATE|{jsonMealName}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

    }
    static void DeleteMenuItem(SocketClient client)
    {
        Console.Write("Enter ID: ");
        string mealNameId = Console.ReadLine();

        client.SendMessage($"MENU_DELETE|{mealNameId}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

    }

    static void LogOut(SocketClient client)
    {
        Console.Clear();
        AdminLogin(client);
    }

    static void AdminLogin(SocketClient client)
    {
        UserDTO user = new UserDTO();

        Console.WriteLine("Login as Admin");
        Console.WriteLine("Enter Email: ");
        user.Email = Console.ReadLine();
        Console.WriteLine("Enter Password: ");
        user.Password = Console.ReadLine();

        string json = JsonConvert.SerializeObject(user);
        client.SendMessage($"LOGIN|{json}|Admin");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if (response == "Login Sucessfull")
        {
            return;
        }

        AdminLogin(client);
    }
}
