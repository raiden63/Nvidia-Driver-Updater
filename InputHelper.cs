using Serilog;

public class InputHelper
{
    public static ILogger? _logger;
    
    public static ConsoleKey PromptKey(string message, bool newLine = false)
    {
        if (newLine)
        {
            Console.WriteLine(message);
        }
        else 
        {
            Console.Write(message);
        }

        return Console.ReadKey().Key;
    }
}