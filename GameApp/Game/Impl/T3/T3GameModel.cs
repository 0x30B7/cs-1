using Raylib_cs;

namespace GameApp.Game.Impl.T3;

public partial class T3Game
{
    private enum SlotState
    {
        None,
        X,
        O
    }

    private class Slot
    {
        private static readonly Color None = Color.BLACK;

        public SlotState State = SlotState.None;
        public Color Color = None;

        public Slot()
        {
        }

        public void Update(SlotState state, Color color)
        {
            State = state;
            Color = color;
        }

        public void Reset()
        {
            State = SlotState.None;
            ;
            Color = None;
        }
    }

    private class Player
    {
        public readonly string Name;
        public readonly Color Color;
        public readonly SlotState State;

        public Player(string name, Color color, SlotState state)
        {
            Name = name;
            Color = color;
            State = state;
        }
    }
}