using Serilog;

public class ConsoleHelper
{
    public static ILogger? _logger;
    
    public static ConsoleKey Prompt(string message, bool newLine = false)
    {
        if (newLine)
        {
            Console.WriteLine(message);
        }
        else 
        {
            Console.Write(message);
        }

        var input = Console.ReadLine();
        Console.WriteLine();

        ConsoleKey key;
        if (Enum.TryParse<ConsoleKey>(input, out key))
        {
            return key;
        }

        return default(ConsoleKey);
    }
}