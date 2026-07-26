class Programm
{
    static void Main()
    {
        Logger log = Logger.Instance;
        log.Log("Первое сообщение");
        log.Log("Второе сообщение");
        log.PrintAllLogs();
        Logger log2 = Logger.Instance;
        Console.WriteLine(log == log2);

    }
}



class Logger
{
    private static Logger _instance;
    public static Logger Instance => _instance ??= new Logger();

    private List<string> logList = new List<string>();

    private Logger()
    {
    }

    public void Log(string message)
    {
        logList.Add(message);
    }

    public void PrintAllLogs()
    {
        if (logList.Count > 0)
        {
            Console.WriteLine(string.Join(",", logList));
        }
        else
        {
            Console.WriteLine("Логов нет");
        }
    }
}