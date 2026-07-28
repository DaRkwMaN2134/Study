using System.Numerics;

public class Proramm
{
    public static void Main()
    {
        Vector2D res = new Vector2D(100, 100).Add(new Vector2D(100, 100));
        Console.WriteLine($"({res.X}, {res.Y})".ToString());
    }
}


public class Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2D(double x, double y)
    {
        
        X = x;
        Y = y;
    }
    Vector2D() : this(0, 0) { }

    public Vector2D Add(Vector2D other)
    {
        return new Vector2D(this.X + other.X, this.Y + other.Y);
    }

    public Vector2D Subtract(Vector2D other)
    {
        return new Vector2D(this.X - other.X, this.Y - other.Y);
    }

    public Vector2D Dot(Vector2D other)
    {
        return new Vector2D(this.X * other.X, this.Y * other.Y);
    }

    public double Length()
    {
       return Math.Sqrt(((this.X * this.X) + (this.Y * this.Y)));
    }

    public Vector2D Normalize()
    {
        if(Length() == 0)
        {
            return new Vector2D(0, 0);
        }
        else
        {
            return new Vector2D(this.X / Length(), this.Y / Length());
        }    
    }
}