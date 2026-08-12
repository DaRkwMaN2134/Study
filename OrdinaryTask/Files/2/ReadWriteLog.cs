using System.IO;
using System.Linq;

var filepath = @"C:\Users\Илья\Desktop\программирование\Test\Test\bin\Debug\net10.0\data.txt";
var errorLog = @"C:\Users\Илья\Desktop\программирование\Test\Test\bin\Debug\net10.0\errors.log";
var resultFile = @"C:\Users\Илья\Desktop\программирование\Test\Test\bin\Debug\net10.0\result.txt";
int lineNumber = 0;
string message = "";
Random rnt = new Random();
List<int> randoms = new List<int>();
List<int> numlist = new List<int>();
List<string> errorlist = new List<string>();
List<string> resultlist = new List<string>();
if (File.Exists(filepath) == true)
{
    try
    {
        {
            var text = File.ReadAllLines(filepath);
            foreach (string item in text)
            {
                lineNumber++;
                try
                {
                    numlist.Add(int.Parse(item));
                }
                catch (System.FormatException)
                {
                    message = ($" Строка в файле: {lineNumber}");
                    errorlist.Add(($"Значение ошибки - {item};") + message);
                }
            }
            File.AppendAllLines(errorLog, errorlist);
        }
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine("Файл ненайден");
    }
    catch (System.IO.IOException)
    {
        Console.WriteLine("Файл ненайден или у вас нет прав на его открытие");
    }
}
else
{
    for (int i = 1; i < 21; i++)
    {
        randoms.Add(rnt.Next(0, 101));
    }
    var numbers = randoms.Select(x => x.ToString());
    File.WriteAllLines(filepath, numbers);

    var text = File.ReadAllLines(filepath);
    foreach (string item in text)
    {
        lineNumber++;
        try
        {
            numlist.Add(int.Parse(item));
        }
        catch (System.FormatException)
        {
            message = ($" Строка в файле: {lineNumber}");
            errorlist.Add(($"Значение ошибки - {item};") + message);
        }
    }
    File.AppendAllLines(errorLog, errorlist);
}

if (numlist.Any())
{
    var sum = numlist.Sum();
    var average = numlist.Average();
    var min = numlist.Min();
    var max = numlist.Max();

    resultlist.Add($"Сумма чисел: {sum}");
    resultlist.Add($"Среднее арифметическое число: {average}");
    resultlist.Add($"Минимальное число: {min}");
    resultlist.Add($"Максимальное число: {max}");
    File.WriteAllLines(resultFile, resultlist);
}