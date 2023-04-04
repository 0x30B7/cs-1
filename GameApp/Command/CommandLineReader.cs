namespace GameApp.Command;

public class CommandLineReader
{
    private Thread _thread;
    private bool running { get; set; } = true;
    private readonly Dictionary<string, Command> _commands = new();
    private readonly Action<string, string[]> SyncExecutor;

    public CommandLineReader(Action<string, string[]> syncExecutor)
    {
        SyncExecutor = syncExecutor;
    }
    
    public void Start()
    {
        if (_thread != null)
        {
            throw new Exception("Command line reader already active.");
        }

        _thread = new Thread(Run)
        {
            Name = "Command Line Thread"
        };
        _thread.Start();
    }

    public void Stop()
    {
        if (_thread == null)
        {
            throw new Exception("Command line reader not active.");
        }
        
        running = false;
    }
    
    public void RegisterCommand() { }

    private void Run()
    {
        while (true)
        {
            var next = Console.ReadLine();

            if (next != null)
            {
                var split = next.Split(" ");
                string command = split[0];;
                string[] args;
                
                if (split.Length == 1)
                {
                    args = Array.Empty<string>();
                }
                else
                {
                    args = new string[split.Length - 1];
                    Array.Copy(split, 1, args, 0, args.Length);
                }
                
                SyncExecutor.Invoke(command, args);
            }
        }
    }
}