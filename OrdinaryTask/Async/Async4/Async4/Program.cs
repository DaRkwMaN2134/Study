using System.Diagnostics;
using System.Linq;
    
class Program
{
    static string originalfile = "originalfile.txt";
    static string targetfile = "copyfile.txt";
    static string folderCFA = "CopyFileAsync";
    static string folderPFA = "ProcessFilesAsync";
    static string reportFile = "report.txt";
    static async Task Main()
    {
        string fullPath1 = Path.Combine(folderCFA, originalfile);
        string fullPath2 = Path.Combine(folderCFA, targetfile);

        string fullPath3 = Path.Combine(folderPFA, reportFile);
        string[] filePaths = new string[]
        {
            Path.Combine(folderPFA, "1.txt"),
            Path.Combine(folderPFA, "2.txt"),
            Path.Combine(folderPFA, "3.txt")
        };

        using var cts = new CancellationTokenSource(3000);
        try
        {
            string result = await SimulateWorkAsync(5000, cts.Token);
            Console.WriteLine(result);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция отменена по таймауту");
        }


        await CopyFileAsync(fullPath1, fullPath2);
        await ProcessFilesAsync(filePaths, fullPath3);
    }

    static async Task CopyFileAsync(string originalfile, string targetfile)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            var fileContent = await File.ReadAllTextAsync(originalfile);
            await File.WriteAllTextAsync(targetfile, fileContent);
            Console.WriteLine("Файл скопирован");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл ненайден");
        }
        catch (IOException)
        {
            Console.WriteLine("Файл недоступен");
        }
        stopwatch.Stop();
        Console.WriteLine($"Затрачено времени: {stopwatch.Elapsed}");
    }


    static async Task ProcessFilesAsync(string[] paths, string reportfile)
    {
        // Список задач для параллельного чтения
        List<Task<string>> tasks = new List<Task<string>>();

        foreach (var path in paths)
        {
            // Запускаем асинхронное чтение каждого файла и добавляем задачу в список
            tasks.Add(ReadFileSafelyAsync(path));
        }

        // Ждём завершения всех задач параллельно
        string[] results = await Task.WhenAll(tasks);

        // Записываем результаты в отчёт
        foreach (var result in results)
        {
            await File.AppendAllTextAsync(reportfile, result + Environment.NewLine);
        }
    }

    static async Task<string> ReadFileSafelyAsync(string path)
    {
        try
        {
            // Читаем содержимое файла асинхронно
            string content = await File.ReadAllTextAsync(path);
            int length = content.Length;
            return $"{path}: {length} символов";
        }
        catch (FileNotFoundException)
        {
            return $"{path}: Файл не найден";
        }
        catch (IOException ex)
        {
            return $"{path}: Ошибка ввода-вывода - {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"{path}: Неизвестная ошибка - {ex.Message}";
        }
    }

    static async Task<string> SimulateWorkAsync(int delay, CancellationToken token)
    {
        await Task.Delay(delay, token);
        return "Работа завершена";
    }
}
