class Program
{

    static Action<string> GreetingDelegate = name => Console.WriteLine($"Привет, {name}");
    static void Main()
    {
        GreetingDelegate("Илья");
    }
}
