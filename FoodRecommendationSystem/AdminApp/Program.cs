using Server;

class Program
{
    static void Main(string[] args)
    {
        // Wait for a short period to ensure the server has time to start
        System.Threading.Thread.Sleep(2000);

        var client = new SocketClient("127.0.0.1", 5000);

        Console.WriteLine("Admin Console");
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Give Feedback");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                // View Menu Logic
                client.SendMessage("GET_MENU");
                break;
            case "2":
                // Give Feedback Logic
                Console.Write("Enter Menu Item ID: ");
                var menuItemId = Console.ReadLine();
                Console.Write("Enter Comment: ");
                var comment = Console.ReadLine();
                Console.Write("Enter Rating: ");
                var rating = Console.ReadLine();
                client.SendMessage($"ADD_FEEDBACK|{menuItemId}|{comment}|{rating}");
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.ReadLine();
    }
}