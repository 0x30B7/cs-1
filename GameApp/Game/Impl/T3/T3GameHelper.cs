namespace GameApp.Game.Impl.T3;

public partial class T3Game
{
    private Slot? GetGridSlot(int x, int y)
    {
        if (x is < 0 or > 2 || y is < 0 or > 2)
        {
            return null;
        }

        return _slots[y * 3 + x];
    }
}