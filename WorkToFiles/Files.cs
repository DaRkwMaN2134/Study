using System.IO;
using System.Linq;
using static System.Console;
using static System.DateTime;

var file = "log.txt";

List<string> lastlog = new List<string>();

if (File.Exists(file) == false)
{
    Console.WriteLine("Файл не был найден и мы его создали за вас");
    File.WriteAllText(file, "");
    Main();
}
else
{
    Main();

}


void Input()
{
    string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    lastlog.Add($"{time} - Запуск программы\n");
    File.AppendAllText(file, lastlog.Last());

}

void Output()
{
    Console.ReadKey(true);
    string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    lastlog.Add($"{time} - Завершение работы\n");
    File.AppendAllText(file, lastlog.Last());
}


void Main()
{
    Console.WriteLine("Последние записи:");
    var lastStart = File.ReadAllLines(file).TakeLast(3);
    Console.WriteLine(string.Join("\n", lastStart));
    Input();
    Console.WriteLine("\nПрограмма запущена. Нажмите любую клавишу для выхода... \n");
    Output();
}