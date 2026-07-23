delegate void GreetingDelegate(string name);

class Program
{
    static void Main()
    {
        GreetingDelegate delh = SayHello;
        GreetingDelegate delg = new GreetingDelegate(SayGoodbye);
        delh("Илья");
        delg("Илья");
    }
    static void SayHello(string name)
    {
        Console.WriteLine($"Привет, {name}");
    }

    static void SayGoodbye(string name)
    {
        Console.WriteLine($"Пока, {name}");
    }
}