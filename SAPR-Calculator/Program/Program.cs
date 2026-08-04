using Shape_Calculator;
List<Shape> list = new List<Shape>()
{
    ShapeFactory.Create("circle", [1]),
    ShapeFactory.Create("rectangle", [1, 1]),
    ShapeFactory.Create("triangle", [1, 1 ,1])
 };
foreach (var shape in list)
{
    shape.PrintInfo();
    if (shape is IDrawable Draw)
    {
        Draw.Draw();
    }
    Console.WriteLine();
}