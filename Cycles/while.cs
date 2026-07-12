string n = "";
while (n != "0")
{
    n = Console.ReadLine();

    if(n == "")
    {
        Console.WriteLine("Повторите");
    }
    else if (n == "0")
    {
        Console.WriteLine("Число = 0");
    }
    else
    {
        Console.WriteLine($"Квадрат числа {int.Parse(n)} = {int.Parse(n) * int.Parse(n)} ");
    }
}