using Shape_Calculator;
using System.IO;


var repository = new ShapeRepository();

var serializer = new JsonShapeSerializer();

var logger = Logger.Instance;

void PrintShapes()
{
    var all = repository.GetAll().ToList();
    for (int i = 0; i < all.Count(); i++)
    {
        Console.Write($"{i + 1}. ");
        all[i].PrintInfo();
    }
}

await Logger.Instance.LogAsync("Программа запущена");
while (true)
{
    Console.WriteLine("\n");
    Console.WriteLine
    (
    string.Join("\n",
    "===== Управление фигурами =====",
    "1.Добавить фигуру",
    "2.Удалить фигуру",
    "3.Показать все фигуры",
    "4.Сохранить в JSON",
    "5.Загрузить из JSON",
    "6.Общая площадь",
    "7.Выход"
    ));

    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine("Какую фигуру вы хотите добавить? (на английском)");
            var type = Console.ReadLine();
            var (area, per) = ShapeCalculator.Get(type);
            switch (type)
            {
                case "circle":
                    while (true)
                    {
                        Console.WriteLine("Введите радиус");
                        var radiusInput = Console.ReadLine();
                        if(double.TryParse(radiusInput, out double rad))
                        {
                            repository.Add(ShapeFactory.Create(type, area, per, [rad]));
                            await Logger.Instance.LogAsync($"Добавлена фигура {type} с параметрами {rad}");
                            break;
                        }
                        else
                        {
                            await Logger.Instance.LogErrorAsync($"Неверные параметры при добавлении фигуры {type}");
                            Console.WriteLine("Неверные параметры");
                            continue;
                        }
                        
                    }
                    break;

                case "rectangle":
                    while (true)
                    {
                        Console.WriteLine("Введите ширину и высоту (каждую в новую строчку)");
                        var widthInput = Console.ReadLine();
                        var heightInput = Console.ReadLine();
                        if (double.TryParse(widthInput, out double width) && double.TryParse(heightInput, out double height))
                        {
                            await Logger.Instance.LogAsync($"Добавлена фигура {type} с параметрами {width}x{height}");
                            repository.Add(ShapeFactory.Create(type, area, per, [width, height]));
                            break;
                        }
                        else
                        {
                            await Logger.Instance.LogErrorAsync($"Неверные параметры при добавлении фигуры {type}");
                            Console.WriteLine("Неверные параметры");
                            continue;
                        }
                    }
                    break;

                case "triangle":
                    while (true)
                    {
                        Console.WriteLine("Введите длину сторон A,B,C (каждую в новую строчку)");
                        var aInput = Console.ReadLine();
                        var bInput = Console.ReadLine();
                        var cInput = Console.ReadLine();
                        if (double.TryParse(aInput, out double A) && double.TryParse(bInput, out double B) && double.TryParse(cInput, out double C))
                        {
                            await Logger.Instance.LogAsync($"Добавлена фигура {type} с параметрами {A}, {B}, {C}");
                            repository.Add(ShapeFactory.Create(type, area, per, [A, B, C]));
                            break;
                        }
                        else
                        {
                            await Logger.Instance.LogErrorAsync($"Неверные параметры при добавлении фигуры {type}");
                            Console.WriteLine("Неверные параметры");
                            continue;
                        }
                    }
                    break;
                default:
                    await Logger.Instance.LogErrorAsync($"Неверные параметры при добавлении фигуры {type}");
                    Console.WriteLine("Такой фигуры не существует");
                    break;
            }
           break;
        case "2":
            if(repository.Count() != 0)
            {
                Console.WriteLine("Какую фигуру вы хотите удалить?");
                PrintShapes();
                var indexInput = Console.ReadLine();
                if (int.TryParse(indexInput, out int index))
                {
                    await Logger.Instance.LogAsync($"Удалена фигура с индексом {index}");
                    repository.RemoveAt(index - 1);
                }
                else
                {
                    await Logger.Instance.LogErrorAsync($"Неверные параметры при удалении фигуры: {index}");
                    Console.WriteLine("Неверный индекс");
                }

            }
            else
            {
                Console.WriteLine("Фигур нет");
            }
            break;
        case "3":
            if (repository.Count() != 0)
            {
                PrintShapes();
            }
            else
            {
                Console.WriteLine("Фигур нет");
            }
            break;
        case "4":
            await serializer.SaveAsync("shapes.json", repository.GetAll());
            await Logger.Instance.LogAsync("Сохранение фигур в JSON");
            Console.WriteLine("Сохранение фигур");
            break;
        case "5":
            repository.Clear();
            var loaded = await serializer.LoadAsync("shapes.json");
            foreach (var shape in loaded)
            {
                repository.Add(shape);
            }
            await Logger.Instance.LogAsync($"Загружено {repository.Count()} фигур из JSON");
            Console.WriteLine($"Загружено {loaded.Count()} фигур");
            break;
        case "6":
            if (repository.Count() != 0)
            {
                await Logger.Instance.LogAsync($"Общая площадь: {repository.Count()}");
                Console.WriteLine(repository.GetTotalArea());
            }
            else
            {
                Console.WriteLine("Фигур нет");
            }
            break;
        case "7":
            await Logger.Instance.LogAsync("Программа завершена");
            return;
        default:
            Console.WriteLine("Нет такой команды");
            break;
    }
}


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