using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Server;

class Program
{
    static void Main(string[] args)
    {
        System.Threading.Thread.Sleep(4000);

        var client = new SocketClient("127.0.0.1", 5000);

        AdminLogin(client);

        Console.WriteLine("Admin Console");
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Add Menu Item");
        Console.WriteLine("3. Update Menu Item");
        Console.WriteLine("4. Delete Menu Item");
        Console.WriteLine("5. To Exit");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => AddMenuItem(client) },
            { "3", () => UpdateMenuItem(client) },
            { "4", () => DeleteMenuItem(client) },
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
    }

    static void UpdateMenuItem(SocketClient client)
    {
        Console.WriteLine("Update Menu Item function placeholder.");
    }

    static void DeleteMenuItem(SocketClient client)
    {
        Console.WriteLine("Delete Menu Item function placeholder.");
    }

    static void ExitProgram()
    {
        Environment.Exit(0);
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
        client.SendMessage($"LOGIN|{json}");
    }
}
