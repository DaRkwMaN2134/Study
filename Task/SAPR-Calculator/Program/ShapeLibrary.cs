using Shape_Calculator;
using System;
using System.Collections.Generic;
using System.Text;
public static class ShapeLibrary
{
    public static (IAreaCalculator Area, IPerimeterCalculator Perimeter) Get(string type)
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
}
