Random rnd = new Random();

int[] FillArray()
{
    int[] Array = new int[100];
    for (int i = 0; i < Array.Length; i++)
    {
        int randomNumber = rnd.Next(1, 101);
        Array[i] = randomNumber;
    }
    return Array;
}

void PrintArray(int[] arrs)
{
    for (int i = 0; i < arrs.Length; i++)
    {
        Console.WriteLine(arrs[i]);
    }
}

int SumArray(int[] arrs)
{
    int sum = 0;
    for (int i = 0; i < arrs.Length; i++)
    {
        sum += arrs[i];
    }
    return sum;

}

int FindMax(int[] arrs)
{
    int max = arrs[0];
    foreach (int num in arrs)
    {
        if (num > max)
        {
            max = num;
        }
    }
    return max;
}

int FindMin(int[] arrs)
{
    int min = arrs[0];
    foreach (int num in arrs)
    {
        if (num < min)
        {
            min = num;
        }
    }
    return min;
}

int CountEven(int[] arrs)
{
    int even = 0;
    for (int i = 0; i < arrs.Length; i++)
    {
        if (arrs[i] % 2 == 0)
        {
            even++;
        }
    }
    return even;
}


void Main()
{
    int[] arr = FillArray();
    PrintArray(arr);
    Console.WriteLine($"Сумма: {SumArray(arr)}");
    Console.WriteLine($"Макс: {FindMax(arr)}");
    Console.WriteLine($"Мин: {FindMin(arr)}");
    Console.WriteLine($"Чётных: {CountEven(arr)}");
}

Main();