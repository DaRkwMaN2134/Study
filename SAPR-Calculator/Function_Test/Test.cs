using Shape_Calculator;

namespace Function_Test
{
    public class Function_Test
    {
        [Theory]
        [InlineData(3.14, 1.0)]
        public void CircleAreaTest(double excepted, double Radius)
        {
            Shape shape = new Circle(Radius);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 4);
        }

        [Theory]
        [InlineData(1.0, 1.0, 1.0)]
        public void RectangleAreaTest(double excepted, double Widht, double Height)
        {
            Shape shape = new Rectangle(Widht, Height);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 4);
        }


        [Theory]
        [InlineData(0.43, 1.0, 1.0, 1.0)]
        public void TriangleAreaTest(double excepted, double A, double B, double C)
        {
            Shape shape = new Triangle(A, B, C);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 2);
        }


        [Theory]
        [InlineData(6.28, 1.0)]
        public void CirclePerimeteraTest(double excepted, double Radius)
        {
            Shape shape = new Circle(Radius);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 2);
        }

        [Theory]
        [InlineData(4.0, 1.0, 1.0)]
        public void RectanglePerimeteraTest(double excepted, double Widht, double Height)
        {
            Shape shape = new Rectangle(Widht, Height);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 2);
        }

        [Theory]
        [InlineData(3.0, 1.0, 1.0, 1.0)]
        public void TrianglePerimeterTest(double excepted, double A, double B, double C)
        {
            Shape shape = new Triangle(A, B, C);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 2);
        }

    }
}
