Console.WriteLine("Ввелите пароль");
string n = Console.ReadLine();
do
{
    if (n != "1234")
    {
        Console.WriteLine("Пароль неправильный, повторите");
        n = Console.ReadLine();
    }

}
while (n != "1234");
{
    Console.WriteLine("Пароль принят");
}