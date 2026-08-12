using Shape_Calculator;
using System.IO;
using System.Runtime.ConstrainedExecution;


var repository = new ShapeRepository();

var serializer = new JsonShapeSerializer();

var logger = Logger.Instance;

(IAreaCalculator Area, IPerimeterCalculator Perimeter) ShapeCalculator(string type)
{
    switch (type)
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
            return (_TriangleAreaCalculator, TrianglePerimeterCalculator);
        default:
            return (null, null);
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
            var (area, per) = ShapeCalculator(type);
            switch (type)
            {
                case "circle":
                    Console.WriteLine("Введите радиус");
                    var radiusInput = Console.ReadLine();
                    if(double.TryParse(radiusInput, out double rad))
                    {
                        repository.Add(ShapeFactory.Create(type, area, per, [rad]));
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверные параметры");
                    }
                    break;

                case "rectangle":
                    Console.WriteLine("Введите ширину и высоту (каждую в новую строчку)");
                    var widthInput = Console.ReadLine();
                    var heightInput = Console.ReadLine();
                    if (double.TryParse(widthInput, out double width) && double.TryParse(heightInput, out double height))
                    {
                        repository.Add(ShapeFactory.Create(type, area, per, [width, height]));
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверные параметры");
                    }
                    break;

                case "triangle":
                    Console.WriteLine("Введите длину сторон A,B,C (каждую в новую строчку)");
                    var aInput = Console.ReadLine();
                    var bInput = Console.ReadLine();
                    var cInput = Console.ReadLine();
                    if (double.TryParse(aInput, out double A) && double.TryParse(bInput, out double B) && double.TryParse(cInput, out double C))
                    {
                        repository.Add(ShapeFactory.Create(type, area, per, [A, B, C]));
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверные параметры");
                    }
                    break;
                default:
                    Console.WriteLine("Такой фигуры не существует");
                    break;
            }
           break;
        case "2":
            if(repository.Count() != 0)
            {
                Console.WriteLine("Какую фигуру вы хотите удалить?");
                var all = repository.GetAll().ToList();
                for (int i = 0; i < all.Count(); i++)
                {
                    Console.Write($"{i+1}. ");
                    all[i].PrintInfo();
                }
                var indexInput = Console.ReadLine();
                if (int.TryParse(indexInput, out int index))
                {
                    repository.RemoveAt(index - 1);
                }
                else
                {
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
                var all = repository.GetAll().ToList();
                for (int i = 0; i < all.Count(); i++)
                {
                    Console.Write($"{i + 1}. ");
                    all[i].PrintInfo();
                }
            }
            else
            {
                Console.WriteLine("Фигур нет");
            }
            break;
        case "4":
            await serializer.SaveAsync("shapes.json", repository.GetAll());
            Console.WriteLine("Сохранение фигур");
            break;
        case "5":
            repository.Clear();
            var loaded = await serializer.LoadAsync("shapes.json");
            foreach (var shape in loaded)
            {
                repository.Add(shape);
            }
            Console.WriteLine($"Загружено {loaded.Count()} фигур");
            break;
        case "6":
            if (repository.Count() != 0)
            {
                Console.WriteLine(repository.GetTotalArea());
            }
            else
            {
                Console.WriteLine("Фигур нет");
            }
            break;
        case "7":
            return;
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