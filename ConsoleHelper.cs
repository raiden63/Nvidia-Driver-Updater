using Serilog;

public class ConsoleHelper
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

        var key = Console.Read();
        return Enum.Parse<ConsoleKey>(key.ToString());
    }

    public static void ClearCurrentConsoleLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}