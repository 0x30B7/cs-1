using System.Runtime.Serialization;

namespace GameApp.Utils;

public class Logger
{
    private DateTimeFormat _dateTimeFormat;

    public Logger(DateTimeFormat dateTimeFormat)
    {
        _dateTimeFormat = dateTimeFormat;
    }

    public void Info(string message)
    {
        var now = DateTime.Now;
        var timeFormatted = now.ToString(_dateTimeFormat.FormatString);
        Console.WriteLine($"[Info] {timeFormatted} {message}");
    }
    
    public void Warn(string message)
    {
        var now = DateTime.Now;
        var timeFormatted = now.ToString(_dateTimeFormat.FormatString);
        Console.WriteLine($"[Warn] {timeFormatted} {message}");
    }
}