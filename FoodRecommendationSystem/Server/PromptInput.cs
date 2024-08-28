namespace Server
{
    public class PromptInput
    {
        public static string StringValues(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input. Please enter only letters, numbers, and spaces.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return input;
        }

        public static string StringValues(string prompt, List<string> validOptions)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input) || !validOptions.Contains(input, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Invalid input. Please enter one of the following: {string.Join(", ", validOptions)}.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return input;
        }

        public static bool BoolValues(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            while (input.ToUpper() != "Y" && input.ToUpper() != "N")
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return input.ToUpper() == "Y";
        }

        public static int IntValues(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            int value;
            while (!int.TryParse(input, out value) || value <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return value;
        }

        public static int ChoiceValues(string prompt, params int[] validChoices)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            int choice;
            while (!int.TryParse(input, out choice) || !validChoices.Contains(choice))
            {
                Console.WriteLine($"Invalid input. Please enter one of the following: {string.Join(", ", validChoices)}.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return choice;
        }
    }
}
