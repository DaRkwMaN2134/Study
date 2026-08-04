using Shape_Calculator;
using System.Globalization;

namespace Shape_Calculator
{

    public static class ShapeFactory
    {
        public static Shape Create(string type, params double[] parameters)
        {
            switch(type)
            {
                case "circle":
                    return new Circle(parameters[0]);

                case "rectangle":
                    return new Rectangle(parameters[0], parameters[1]);

                case "triangle":
                    return new Triangle(parameters[0], parameters[1], parameters[2]);
                default:
                    return null;
            }

        }
    }


    public abstract class Shape
    {
        string Name { get; set; }

        public Shape(string name)
        {
            Name = name;
        }

        public abstract double Area();

        public abstract double Perimeter();

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Имя фигуры: {Name}, Площадь: {Area():F2}, Периметр: {Perimeter():F2}");
        }
    }

    public class Circle: Shape, IDrawable
    {
        double Radius { get; set; }

        public Circle(double radius) : base("Круг")
        {
            Radius = radius;
        }
        public override double Area()
        {
            return 3.14 * (Radius * Radius);
        }

        public override double Perimeter()
        {
            return (2 * 3.14) * Radius;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
        }

        public void Draw()
        {
            Console.WriteLine($"Рисуем круг радиусом {Radius:F2}");
        }
    }


    public class Rectangle : Shape, IDrawable
    {
        double Width { get; set; }
        double Height { get; set; }

        public Rectangle(double width, double height) : base("Прямоугольник")
        {
            Width = width;
            Height = height;
        }
        public override double Area()
        {
            return Width * Height;
        }

        public override double Perimeter()
        {
            return 2 * (Width + Height);
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
        }

        public void Draw()
        {
            Console.WriteLine($"Рисуем прямоугольник с стороными {Width:F2}x{Height:F2}");
        }
    }


    public class Triangle : Shape, IDrawable
    {
        double SideA { get; set; }
        double SideB { get; set; }
        double SideC { get; set; }

        public Triangle(double sidea, double sideb, double sidec) : base("Треугольник")
        {
            SideA = sidea;
            SideB = sideb;
            SideC = sidec;
        }
        public override double Area()
        {
            var p = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }

        public override double Perimeter()
        {
            return SideA + SideB + SideC;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
        }

        public void Draw()
        {
            Console.WriteLine($"Рисуем треугольник с сторонами A - {SideA:F2}, B - {SideB:F2}, C - {SideC:F2}");
        }
    }


    public interface IDrawable
    {
        public void Draw();
    }
}
