namespace GameApp.Utils;

public static class Directions
{
    public static readonly Direction Up = new(0, -1);
    public static readonly Direction UpRight = new(1, -1);
    public static readonly Direction Right = new(1, 0);
    public static readonly Direction DownRight = new(1, 1);
    public static readonly Direction Down = new(0, 1);
    public static readonly Direction DownLeft = new(-1, 1);
    public static readonly Direction Left = new(-1, 0);
    public static readonly Direction UpLeft = new(-1, -1);

    public static readonly List<Direction> All = new() { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft };
    public static readonly List<Direction> Half = new() { Up, UpRight, Right, DownRight };
    
    static Directions()
    {
        Up.Opposite = Down;
        UpRight.Opposite = DownLeft;
        Right.Opposite = Left;
        DownRight.Opposite = UpLeft;
        Down.Opposite = Up;
        DownLeft.Opposite = UpRight;
        Left.Opposite = Right;
        UpLeft.Opposite = DownRight;
    }
}