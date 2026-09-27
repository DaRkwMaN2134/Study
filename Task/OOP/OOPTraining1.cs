class Program
{
    static void Main(string[] args)
    {

        List<Shape> list = new List<Shape>()
        {
            new Circle(1),
            new Circle(2),
            new Circle(3),
            new Rectangle(1, 1),
            new Rectangle(2, 2),
            new Rectangle(3, 3)
        };
        foreach (Shape shape in list)
        {
            shape.PrintInfo();
            if (shape is IDrawable drawable)
            {
                drawable.Draw();
            }
        }

    }
}

interface IDrawable
{
    void Draw();
}

abstract class Shape
{
    public abstract string name { get; set; }
    public abstract double Area();
    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {name}, Площадь: {Area()}");
    }
}

class Circle : Shape, IDrawable
{
    public override string name { get; set; } = "Круг";
    public double R { get; set; }
    public override double Area() => Math.PI * (R * R);
    public void Draw()
    {
        Console.WriteLine($"Рисуем {name} с радиусом {R}");
    }
    public Circle(double r)
    {
        R = r;
    }
}

class Rectangle : Shape, IDrawable
{
    public override string name { get; set; } = "Прямоугольник";
    public double Width { get; set; }
    public double Height { get; set; }
    public override double Area() => Width * Height;
    public void Draw()
    {
        Console.WriteLine($"Рисуем {name} {Width} на {Height}");
    }
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
}