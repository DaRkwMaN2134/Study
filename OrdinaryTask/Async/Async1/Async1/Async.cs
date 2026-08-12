using System.Diagnostics;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
class Programm
{
    static async Task Main()
    {

        Console.WriteLine("Введите текст для 1 метода");
        string text1 = Console.ReadLine();
        Console.WriteLine("Введите текст для 2 метода");
        string text2 = Console.ReadLine();
        Console.WriteLine("Введите задержку для 1");
        string delaytext1 = Console.ReadLine();
        Console.WriteLine("Введите задержку для 2");
        string delaytext2 = Console.ReadLine();
        int delay1 = int.Parse(delaytext1);
        int delay2 = int.Parse(delaytext2);
        Task task1 = PrintMessageAsync(text1, delay1);
        Task task2 =PrintMessageAsync(text2, delay2);
        await Task.WhenAll(task1, task2);

        ////////////////////////////////////////////////////////////////////////////////////////

        string path = @"C:\Users\Илья\Desktop\программирование\Async\Async\bin\Debug\net10.0\readfile.txt";
        string textfile = "";
        textfile = await ReadFileAsync(path);
        Console.WriteLine($"Длина строки с файла - {textfile.Length}");

        /////////////////////////////////////////////////////////////////////////////////////////

        string numpath = @"C:\Users\Илья\Desktop\программирование\Async\Async\bin\Debug\net10.0\numfile.txt";
        int numsum = 0;
        try
        {
            numsum = await SumNumbersFromFileAsync(numpath);
            Console.WriteLine($"Сумма чисел с файла - {numsum}");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("Файл не найден");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////

        await ProcessTasksAsync();
    }


    static async Task PrintMessageAsync(string Text, int Delay)
    {
        await Task.Delay(Delay);
        Console.WriteLine(Text);
    }


    static async Task<string> ReadFileAsync(string path)
    {
        string text = await File.ReadAllTextAsync(path);
        return text;
    }

    static async Task<int> SumNumbersFromFileAsync(string path)
    {
        string[] text = await File.ReadAllLinesAsync(path);
        string errorlog = @"C:\Users\Илья\Desktop\программирование\Async\Async\bin\Debug\net10.0\error.log";
        int num = 0;
        foreach (var number in text)
        {
            try
            {
                num += int.Parse(number);
            }
            catch(FormatException ex)
            {
                await File.AppendAllTextAsync(errorlog, number);
            }
        }
        return num;
    }

    static async Task ProcessTasksAsync()
    {
        Random rnd = new Random();
        Stopwatch sw = Stopwatch.StartNew();
        Task task1 = Task.Delay(rnd.Next(999, 5001));
        Task task2 = Task.Delay(rnd.Next(999, 5001));
        Task task3 = Task.Delay(rnd.Next(999, 5001));
        Task task4 = Task.Delay(rnd.Next(999, 5001));
        Task task5 = Task.Delay(rnd.Next(999, 5001));
        await Task.WhenAll(task1, task2, task3, task4, task5);
        sw.Stop();
        Console.WriteLine($"Время выполнения - {sw.ElapsedMilliseconds}мс");
    }
 }