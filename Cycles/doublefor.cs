int tabl = 5;
int tablmaxvalue = 9;
for(int i = 1; i <= tabl; i++)
{
    Console.WriteLine($"Таблица умножения на {i}");
    for (int j = 1; j <= tablmaxvalue; j++)
    {
        Console.WriteLine($"{i} * {j} = {i * j} ");
    }
}