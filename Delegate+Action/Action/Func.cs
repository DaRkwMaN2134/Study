class Program
{
    static void Main()
    {
        Func<int, int> sum = x => x * x;
        Console.WriteLine(sum(10));
    }
}