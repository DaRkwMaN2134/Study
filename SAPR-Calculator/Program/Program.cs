using Shape_Calculator;
using System.IO;


/////////////////////////////////////////////////////////////////////////////////////////////////////


DateTime startTime = DateTime.Now;
await Logger.Instance.LogAsync("Программа запущена\n");

string mainfile = "shapes.txt";
string mainpath = Path.Combine(mainfile);

List<Shape> list = new List<Shape>(){};


/////////////////////////////////////////////////////////////////////////////////////////////////////


async Task<string[]> ReadFileAsync()
{
    try
    {
        string[] allContent = await File.ReadAllLinesAsync(mainpath);
        return allContent;
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("Файл не найден. Создаю файл с примерами..."); // Заполнение и создание файла(если он отсутствует)
        string[] sampleLines = new string[]
        {
            "circle 5                ",
            "rectangle 4 6           ",
            "triangle 3 4 5          ",
            "circle -2",
            "rectangle 0 5",
            "triangle 1 1 3       ",
            "               ",
            "square 1 2"
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


/////////////////////////////////////////////////////////////////////////////////////////////////////

 (IAreaCalculator Area, IPerimeterCalculator Perimeter) ShapeCalculator(string type)
{
    switch(type)
    {
        case "circle":
            var _CircleAreaCalculator = new CircleAreaCalculator();
            var CirclePerimeterCalculator = new CirclePerimeterCalculator();
            return (_CircleAreaCalculator, CirclePerimeterCalculator);
        case "rectangle":
            var _RectangleAreaCalculator = new RectangleAreaCalculator();
            var RectanglePerimeterCalculator = new RectanglePerimeterCalculator();
            return (_RectangleAreaCalculator, RectanglePerimeterCalculator);
        case "triangle":
            var _TriangleAreaCalculator = new TriangleAreaCalculator();
            var TrianglePerimeterCalculator = new TrianglePerimeterCalculator();
            return(_TriangleAreaCalculator, TrianglePerimeterCalculator);
        default:
            return(null, null);
    }
        
}


/////////////////////////////////////////////////////////////////////////////////////////////////////


async Task ParseCircleAsync(string type, string[] parts)
{
    if (double.TryParse(parts[1], out double radius))
    {
        var (area, per) = ShapeCalculator(type);
        if (radius > 0)
        {
            await Logger.Instance.LogAsync($"Создана фигура: {type}, Радиус: {parts[1]}\n");
            list.Add(ShapeFactory.Create("circle", area, per, [radius]));
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
}


/////////////////////////////////////////////////////////////////////////////////////////////////////


async Task ParseRectangleAsync(string type, string[] parts)
{
    if (double.TryParse(parts[1], out double width) && double.TryParse(parts[2], out double height)) // Перевод текстовых данных в числа с точкой для сравнения
    {
        var (area, per) = ShapeCalculator(type);
        if (width > 0 && height > 0)
        {
            await Logger.Instance.LogAsync($"Создана фигура: {type}, Ширина: {parts[1]}, Высота: {parts[2]}\n");
            list.Add(ShapeFactory.Create("rectangle", area, per, [width, height]));
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
}


/////////////////////////////////////////////////////////////////////////////////////////////////////


async Task ParseTriangleAsync(string type, string[] parts)
{
    if (double.TryParse(parts[1], out double sidea) && double.TryParse(parts[2], out double sideb) && double.TryParse(parts[3], out double sidec))
    {
        var (area, per) = ShapeCalculator(type);
        if (sidea + sideb > sidec && sidea + sidec > sideb && sideb + sidec > sidea)
        {
            await Logger.Instance.LogAsync($"Создана фигура: {type}, Стороны: {sidea}, {sideb}, {sidec}\n");
            list.Add(ShapeFactory.Create("triangle", area, per, [sidea, sideb, sidec]));
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
}


/////////////////////////////////////////////////////////////////////////////////////////////////////
async Task ShapeDataAsync(string[] lines)
{
    if (lines == null) return;
    foreach (string line in lines) 
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries); // Разбивка массива на строки
        if (parts.Length == 0)
            continue;

        string type = parts[0].ToLower();

        switch (type)
        {
            case "circle": // Проверка фигур
                if (parts.Length < 2)
                {
                    Console.WriteLine("Недостаточно параметров для круга");
                    break;
                }
                await ParseCircleAsync(type, parts);
                break;

            case "rectangle":
                if (parts.Length < 3)
                {
                    Console.WriteLine("Недостаточно параметров для прямоугольника");
                    break;
                }
                await ParseRectangleAsync(type, parts);
                break;

            case "triangle":
                if (parts.Length < 4)
                {
                    Console.WriteLine("Недостаточно параметров для треугольника");
                    break;
                }
                await ParseTriangleAsync(type, parts);
                break;

            default:
                await Logger.Instance.LogErrorAsync($"Неизвестный тип фигуры: {type}\n");
                Console.WriteLine($"Неизвестный тип фигуры: {type}");
                break;
        }
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////////////


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



/////////////////////////////////////////////////////////////////////////////////////////////////////


public class Logger
{
    private static Logger _instance;
    private Logger() { }
    public static Logger Instance // Создание синглтона
    {
        get
        {
            if (_instance == null)
                _instance = new Logger(); // создаётся объект логера при первом обращении
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