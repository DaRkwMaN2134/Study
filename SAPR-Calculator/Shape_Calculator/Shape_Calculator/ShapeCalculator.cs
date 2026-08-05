using Shape_Calculator;
using System.Globalization;

namespace Shape_Calculator
{

    public static class ShapeFactory
    {
        public static Shape Create(string type, IAreaCalculator _IAreaCalculator, IPerimeterCalculator _IPerimeterCalculator, params double[] parameters)
        {
            switch (type)
            {
                case "circle":
                    return new Circle(_IAreaCalculator, _IPerimeterCalculator, parameters[0]);

                case "rectangle":
                    return new Rectangle(_IAreaCalculator, _IPerimeterCalculator, parameters[0], parameters[1]);

                case "triangle":
                    return new Triangle(_IAreaCalculator, _IPerimeterCalculator, parameters[0], parameters[1], parameters[2]);
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
        public double Radius { get; set; }
        IAreaCalculator _IAreaCalculator;
        IPerimeterCalculator _IPerimeterCalculator;

        public Circle(IAreaCalculator iAreaCalculator, IPerimeterCalculator iPerimeterCalculator, double radius) : base("Круг")
        {
            Radius = radius;
            _IAreaCalculator = iAreaCalculator;
            _IPerimeterCalculator = iPerimeterCalculator;
        }

        public override double Area()
        {
            return _IAreaCalculator.CalculateArea(this);
        }

        public override double Perimeter()
        {
            return _IPerimeterCalculator.CalculatePerimeter(this);
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
        public double Width { get; set; }
        public double Height { get; set; }
        IAreaCalculator _IAreaCalculator;
        IPerimeterCalculator _IPerimeterCalculator;

        public Rectangle(IAreaCalculator iAreaCalculator, IPerimeterCalculator iPerimeterCalculator, double width, double height) : base("Прямоугольник")
        {
            _IAreaCalculator = iAreaCalculator;
            _IPerimeterCalculator = iPerimeterCalculator;
            Width = width;
            Height = height;
        }
        public override double Area()
        {
            return _IAreaCalculator.CalculateArea(this);
        }

        public override double Perimeter()
        {
            return _IPerimeterCalculator.CalculatePerimeter(this);
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
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        IAreaCalculator _IAreaCalculator;
        IPerimeterCalculator _IPerimeterCalculator;

        public Triangle(IAreaCalculator iAreaCalculator, IPerimeterCalculator iPerimeterCalculator, double sidea, double sideb, double sidec) : base("Треугольник")
        {
            _IAreaCalculator = iAreaCalculator;
            _IPerimeterCalculator = iPerimeterCalculator;
            SideA = sidea;
            SideB = sideb;
            SideC = sidec;
        }
        public override double Area()
        {
            return _IAreaCalculator.CalculateArea(this);
        }

        public override double Perimeter()
        {
            return _IPerimeterCalculator.CalculatePerimeter(this);
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

    public class CircleAreaCalculator: IAreaCalculator
    {
        public double CalculateArea(Shape shape)
        {
            return Math.PI * (((Circle)shape).Radius * ((Circle)shape).Radius);
        }
    }

    public class CirclePerimeterCalculator : IPerimeterCalculator
    {
        public double CalculatePerimeter(Shape shape)
        {
            return (2 * Math.PI) * ((Circle)shape).Radius;
        }
    }



    public class RectangleAreaCalculator : IAreaCalculator
    {
        public double CalculateArea(Shape shape)
        {
            return ((Rectangle)shape).Width * ((Rectangle)shape).Height;
        }
    }

    public class RectanglePerimeterCalculator : IPerimeterCalculator
    {
        public double CalculatePerimeter(Shape shape)
        {
            return 2 * (((Rectangle)shape).Width + ((Rectangle)shape).Height);
        }
    }


    public class TriangleAreaCalculator : IAreaCalculator
    {
        public double CalculateArea(Shape shape)
        {
            if (((Triangle)shape).SideA > 0 && ((Triangle)shape).SideB > 0 && ((Triangle)shape).SideC > 0 &&
                ((Triangle)shape).SideA + ((Triangle)shape).SideB > ((Triangle)shape).SideC &&
                ((Triangle)shape).SideA + ((Triangle)shape).SideC > ((Triangle)shape).SideB &&
                ((Triangle)shape).SideB + ((Triangle)shape).SideC > ((Triangle)shape).SideA)
            {
                var p = (((Triangle)shape).SideA  + ((Triangle)shape).SideB + ((Triangle)shape).SideC) / 2;
                return Math.Sqrt(p * (p - ((Triangle)shape).SideA) * (p - ((Triangle)shape).SideB) * (p - ((Triangle)shape).SideC));
            }
            else
            {
                return 0;
            }
        }
    }


    public class TrianglePerimeterCalculator : IPerimeterCalculator
    {
        public double CalculatePerimeter(Shape shape)
        {
            return ((Triangle)shape).SideA + ((Triangle)shape).SideB + ((Triangle)shape).SideC;
        }
    }


    public interface IDrawable
    {
        public void Draw();
    }

    public interface IAreaCalculator
    {
        public double CalculateArea(Shape shape);
    }

    public interface IPerimeterCalculator
    {
        public double CalculatePerimeter(Shape shape);
    }
}
