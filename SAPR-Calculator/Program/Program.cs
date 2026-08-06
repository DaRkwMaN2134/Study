using Shape_Calculator;
using System.IO;

DateTime startTime = DateTime.Now;
await Logger.Instance.LogAsync("Программа запущена\n");

string mainfile = "shapes.txt";
string mainpath = Path.Combine(mainfile);

List<Shape> list = new List<Shape>(){};

async Task<string[]> ReadFileAsync()
{
    try
    {
        string[] allContent = await File.ReadAllLinesAsync(mainpath);
        return allContent;
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("Файл не найден. Создаю файл с примерами...");
        string[] sampleLines = new string[]
        {
            "circle 5",
            "rectangle 4 6",
            "triangle 3 4 5",
            "circle -2",
            "rectangle 0 5",
            "triangle 1 1 3"
        };
        await File.WriteAllLinesAsync(mainpath, sampleLines);
        Console.WriteLine("Файл создан. Перезапустите программу.");
        return null;
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
        return null;
    }
}

async Task ShapeDataAsync(string[] lines)
{
    if (lines == null) return;

    foreach (string line in lines)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            continue;

        string type = parts[0].ToLower();

        switch (type)
        {
            case "circle":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Недостаточно параметров для круга");
                    break;
                }
                if (double.TryParse(parts[1], out double radius))
                {
                    var _CircleAreaCalculator = new CircleAreaCalculator();
                    var CirclePerimeterCalculator = new CirclePerimeterCalculator();
                    if (radius > 0)
                    {
                        await Logger.Instance.LogAsync($"Создана фигура: {type}, Радиус: {parts[1]}\n");
                        list.Add(ShapeFactory.Create("circle", _CircleAreaCalculator, CirclePerimeterCalculator, [radius]));
                    }
                    else
                    {
                        await Logger.Instance.LogErrorAsync($"Фигура: {type}, Радиус: {parts[1]}\n");
                        Console.WriteLine("Неверные параметры для круга");
                    }
                }
                else
                {
                    Console.WriteLine($"Некорректный радиус: {parts[1]}");
                }
                break;

            case "rectangle":
                if (parts.Length < 3)
                {
                    Console.WriteLine("Недостаточно параметров для прямоугольника");
                    break;
                }
                if (double.TryParse(parts[1], out double width) && double.TryParse(parts[2], out double height))
                {
                    var _RectangleAreaCalculator = new RectangleAreaCalculator();
                    var RectanglePerimeterCalculator = new RectanglePerimeterCalculator();
                    if (width > 0 && height > 0)
                    {
                        await Logger.Instance.LogAsync($"Создана фигура: {type}, Ширина: {parts[1]}, Высота: {parts[2]}\n");
                        list.Add(ShapeFactory.Create("rectangle", _RectangleAreaCalculator, RectanglePerimeterCalculator, [width, height]));
                    }
                    else
                    {
                        await Logger.Instance.LogErrorAsync($"Фигура: {type}, Ширина: {parts[1]}, Высота: {parts[2]}\n");
                        Console.WriteLine("Неверные параметры для прямоугольника");
                    }
                }
                else
                {
                    Console.WriteLine($"Некорректные размеры: {parts[1]}, {parts[2]}");
                }
                break;

            case "triangle":
                if (parts.Length < 4)
                {
                    Console.WriteLine("Недостаточно параметров для треугольника");
                    break;
                }
                if (double.TryParse(parts[1], out double sidea) && double.TryParse(parts[2], out double sideb) && double.TryParse(parts[3], out double sidec))
                {
                    var _TriangleAreaCalculator = new TriangleAreaCalculator();
                    var TrianglePerimeterCalculator = new TrianglePerimeterCalculator();
                    if (sidea + sideb > sidec && sidea + sidec > sideb && sideb + sidec > sidea)
                    {
                        await Logger.Instance.LogAsync($"Создана фигура: {type}, Стороны: {sidea}, {sideb}, {sidec}\n");
                        list.Add(ShapeFactory.Create("triangle", _TriangleAreaCalculator, TrianglePerimeterCalculator, [sidea, sideb, sidec]));
                    }
                    else
                    {
                        await Logger.Instance.LogErrorAsync($"Фигура: {type}, Стороны: {sidea}, {sideb}, {sidec}\n");
                        Console.WriteLine("Неравенство треугольника не выполняется");
                    }
                }
                else
                {
                    Console.WriteLine($"Некорректные стороны: {parts[1]}, {parts[2]}, {parts[3]}");
                }
                break;

            default:
                Console.WriteLine($"Неизвестный тип фигуры: {type}");
                break;
        }
    }
}


string[] lines = await ReadFileAsync();
if (lines != null)
{
    await ShapeDataAsync(lines);
}
else
{
    Console.WriteLine("Перезапустите программу после создания файла.");
    return; // завершаем выполнение (если ты в top-level)
}
foreach (var shape in list)
{
    Console.WriteLine("\n");
    shape.PrintInfo();
    if (shape is IDrawable Draw)
    {
        Draw.Draw();
    }
}

DateTime endTime = DateTime.Now;
TimeSpan duration = endTime - startTime;
await Logger.Instance.LogAsync($"Время выполнения: {duration.TotalMilliseconds} мс\n");


public class Logger
{
    private static Logger _instance;
    private Logger() { }
    public static Logger Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Logger(); // создаётся при первом обращении
            return _instance;
        }
    }

    public async Task LogAsync(string message)
    {
        DateTime localTime = DateTime.Now;
        string logfile = "log.txt";
        await File.AppendAllTextAsync(logfile, $" [{localTime}] INFO:{message}");
    }

    public async Task LogErrorAsync(string message)
    {
        DateTime localTime = DateTime.Now;
        string errorfile = "errors.log";
        await File.AppendAllTextAsync(errorfile, $" [{localTime}] ERROR:{message}");
    }
}