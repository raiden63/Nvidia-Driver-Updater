using System.Diagnostics;

public class CommandProcess 
{
    public static string Execute(string command)
    {
        var p = new Process();

        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = $"/c {command}";

        p.Start();

        var output = p.StandardOutput.ReadToEnd();

        p.WaitForExit();

        return output.Trim();
    }
}