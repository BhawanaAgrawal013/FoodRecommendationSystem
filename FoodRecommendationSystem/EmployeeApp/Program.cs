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
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Recieve Notification");
        Console.WriteLine("3. Vote for Meal Options");
        Console.WriteLine("5. To Exit");

        var menuActions = new Dictionary<string, Action>
        {
            { "1", () => ViewMenu(client) },
            { "2", () => GetNotification(client)},
            { "3", () => VoteMealOptions(client)},
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
        Console.Write("Enter Classification: ");
        string classification = Console.ReadLine();

        client.SendMessage($"MEAL_GETOPTIONS|{classification}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        Console.Write("Enter the choosen meal: ");
        string id = Console.ReadLine();

        client.SendMessage($"MEAL_VOTE|{id}");
    }

    static void ExitProgram()
    {
        Environment.Exit(0);
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
        client.SendMessage($"LOGIN|{json}");

        var response = client.RecieveMessage();
        Console.WriteLine(response);

        if(response == "Login Sucessfull")
        {
            return;
        }

        EmployeeLogin(client);
    }
}