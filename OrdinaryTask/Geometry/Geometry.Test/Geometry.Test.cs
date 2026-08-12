using System.Numerics;

namespace Geometry.Test
{
    public class Geometry_Test
    {
        [Fact]
        public void AddTest()
        {
            var v1 = new Vector2D(2, 2);
            var v2 = new Vector2D(2, 2);
            var expected = new Vector2D(4, 4);

            var result = v1.Add(v2);

            Assert.Equal(expected.X, result.X, 5);
            Assert.Equal(expected.Y, result.Y, 5);
        }


        [Fact]
        public void SubtractTest()
        {
            var v1 = new Vector2D(2, 2);
            var v2 = new Vector2D(2, 2);
            var expected = new Vector2D(0, 0);

            var result = v1.Subtract(v2);

            Assert.Equal(expected.X, result.X, 5);
            Assert.Equal(expected.Y, result.Y, 5);
        }


        [Fact]
        public void DotTest()
        {
            var v1 = new Vector2D(2, 2);
            var v2 = new Vector2D(1, 1);
            var expected = new Vector2D(2, 2);

            var result = v1.Dot(v2);

            Assert.Equal(expected.X, result.X);
            Assert.Equal(expected.Y, result.Y, 5);
        }

        [Fact]
        public void LengthTest()
        {
            var v = new Vector2D(3, 4);
            var expected = 5;

            var result = v.Length();

            Assert.Equal(expected, result, 5);
        }

        [Fact]
        public void NormalizeTest()
        {
            var v = new Vector2D(1, 1);
            var expected = new Vector2D(0.70710678, 0.70710678);

            var result = v.Normalize();

            Assert.Equal(expected.X, result.X, 5);
            Assert.Equal(expected.Y, result.Y, 5);
        }
    }
}
