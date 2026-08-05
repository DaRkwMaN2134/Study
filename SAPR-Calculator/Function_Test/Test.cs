using Shape_Calculator;

namespace Function_Test
{
    public class Function_Test
    {
        [Theory]
        [InlineData(3.14, 1.0)]
        [InlineData(12.56, 2)]
        [InlineData(0 ,0)]
        [InlineData(78.50, -5)]
        public void CircleAreaTest(double excepted, double Radius)
        {
            Shape shape = new Circle(Radius);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 5);
        }

        [Theory]
        [InlineData(1.0, 1.0, 1.0)]
        [InlineData(6, 2, 3)]
        [InlineData(0, 0, 5)]
        [InlineData(-4, -1, 4)]
        public void RectangleAreaTest(double excepted, double Widht, double Height)
        {
            Shape shape = new Rectangle(Widht, Height);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 4);
        }


        [Theory]
        [InlineData(6, 3, 4, 5)]
        [InlineData(10.83, 5, 5, 5)]
        [InlineData(0.43, 1, 1, 1)]
        [InlineData(0, 1, 1, 3)]
        [InlineData(0, 0, 5, 5)]
        [InlineData(0, -1, 2, 2)]
        public void TriangleAreaTest(double excepted, double A, double B, double C)
        {
            Shape shape = new Triangle(A, B, C);
            double actual = shape.Area();
            Assert.Equal(excepted, actual, 2);
        }


        [Theory]
        [InlineData(6.28, 1.0)]
        [InlineData(12.56, 2)]
        [InlineData(0, 0)]
        [InlineData(-31.40, -5)]
        public void CirclePerimeteraTest(double excepted, double Radius)
        {
            Shape shape = new Circle(Radius);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 5);
        }

        [Theory]
        [InlineData(4.0, 1.0, 1.0)]
        [InlineData(10, 2, 3)]
        [InlineData(10, 0, 5)]
        [InlineData(6, -1, 4)]
        public void RectanglePerimeteraTest(double excepted, double Widht, double Height)
        {
            Shape shape = new Rectangle(Widht, Height);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 2);
        }

        [Theory]
        [InlineData(3.0, 1.0, 1.0, 1.0)]
        [InlineData(12, 3, 4, 5)]
        [InlineData(15, 5, 5, 5)]
        [InlineData(3, 1, 1, 1)]
        public void TrianglePerimeterTest(double excepted, double A, double B, double C)
        {
            Shape shape = new Triangle(A, B, C);
            double actual = shape.Perimeter();
            Assert.Equal(excepted, actual, 2);
        }

    }
}
