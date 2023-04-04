namespace GameApp.Utils;

public class Rectangle
{
    public readonly int X, Y, Width, Height;

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool IsPointWithin(int x, int y)
    {
        return X < x && x < X + Width && Y < y && y < Y + Height;
    }
}