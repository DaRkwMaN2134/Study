Console.Write("Введите число от 0 до 100: ");
string input = Console.ReadLine(); // Запрос строки
int number = int.Parse(input);

if(number >= 0 && number <=100)
{
    if (number >= 90)
    {
        Console.Write("Отлично");
    }
    else if (number >= 70)
    {
        Console.Write("Хорошо");
    }
    else if (number >= 50)
    {
        Console.Write("Удовлетворительно");
    }
    else if (number >= 0)
    {
        Console.Write("Неудовлетворительно");
    }
}
else
{
    Console.Write("Число не в диапазоне");
}