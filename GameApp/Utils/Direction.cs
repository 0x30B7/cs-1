namespace GameApp.Utils;

public class Direction
{
    public readonly int X, Y;
    public Direction Opposite { get; internal set; }

    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }
}