namespace Shape_Calculator
{
    public class ShapeRepository : IShapeRepository
    {
        private List<Shape> _shapes = new List<Shape>();

        public void Add(Shape shape)
        {
            _shapes.Add(shape);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _shapes.Count)
            {
                _shapes.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Неправильный номер фигуры");
            }
        }

        public IEnumerable<Shape> GetAll()
        {
            return _shapes;
        }

        public double GetTotalArea()
        {
            return _shapes.Sum(s => s.Area());
        }

        public int Count()
        {
            return _shapes.Count;
        }

        public void Clear()
        {
            _shapes.Clear();
        }
    }

    public interface IShapeRepository
    {
        void Add(Shape shape);
        void RemoveAt(int index);
        IEnumerable<Shape> GetAll();
        double GetTotalArea();
        int Count();
        void Clear();
    }
}
