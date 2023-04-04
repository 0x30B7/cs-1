using GameApp.Utils;
using Raylib_cs;
using Rectangle = GameApp.Utils.Rectangle;

namespace GameApp.Game.Impl.T3;

public partial class T3Game : IGame
{
    private readonly List<Slot> _slots = Enumerable.Range(0, 10).Select(n => new Slot()).ToList();
    private int _openSlotsRemaining;

    private readonly List<KeyValuePair<Rectangle, int>> _slotRegions = new();
    private Rectangle _playAgainButtonRegion;
    private Rectangle _backToMenuButtonRegion;

    private bool _firstFrame = true;

    private readonly Player _playerA = new("Player A", Color.RED, SlotState.O);
    private readonly Player _playerB = new("Player B", Color.BLUE, SlotState.X);
    private Player _turn;
    private Player? _winner;

    public event EventHandler? GameQuitRequestEvent;

    public void Start()
    {
        _ResetState();
    }

    public void Stop()
    { }

    public void UpdateAndDraw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);

        if (_firstFrame)
        {
            _firstFrame = false;
            _CalculateComponentPositions();
        }

        _HandleInput();
        _DrawBoard();

        var gameOver = _winner != null || _openSlotsRemaining == 0;

        if (gameOver)
        {
            _DrawGameOverGraphics();
        }

        Raylib.EndDrawing();
    }

    private void _CalculateComponentPositions()
    {
        _slotRegions.Clear();

        var screenWidth = Raylib.GetScreenWidth();
        var screenHeight = Raylib.GetScreenHeight();
        var centerX = screenWidth / 2;
        var centerY = screenHeight / 2;

        const int buttonWidth = 100;
        const int buttonHeight = 25;

        _playAgainButtonRegion = new((centerX - buttonWidth) - 5, (int)(screenHeight - (buttonHeight * 1.5)),
            buttonWidth, buttonHeight);

        _backToMenuButtonRegion = new((centerX + 5), (int)(screenHeight - (buttonHeight * 1.5)),
            buttonWidth, buttonHeight);

        const int gameSpaceSpan = 48 * 3;

        var originalSpanX = centerX - gameSpaceSpan / 2;
        var originalSpanY = centerY - gameSpaceSpan / 2;
        var spanY = originalSpanY;

        var index = 0;
        for (var y = 0; y < 3; y++)
        {
            var spanX = originalSpanX;

            for (var x = 0; x < 3; x++)
            {
                _slotRegions.Add(new(new(spanX, spanY, 48, 48), index++));
                spanX += 48;
            }

            spanY += 48;
        }
    }
    
    private void _SwitchTurn()
    {
        _turn = _turn.Equals(_playerA) ? _playerB : _playerA;
    }

    private void _ResetState()
    {
        _firstFrame = true;
        _openSlotsRemaining = _slots.Count - 1;
        _turn = new Random().Next(10) > 5 ? _playerA : _playerB;
        _winner = null;
        foreach (var slot in _slots)
        {
            slot.Reset();
        }
    }

    private void _CheckAndUpdateWinner(int lastPlaceX, int lastPlaceY)
    {
        var consecutiveHits = 1;
        var current = GetGridSlot(lastPlaceX, lastPlaceY);

        foreach (var direction in Directions.Half)
        {
            for (var i = 1; i < 3; i++)
            {
                var placedAt = GetGridSlot(
                    lastPlaceX + direction.X * i,
                    lastPlaceY + direction.Y * i);
                if (placedAt != null && placedAt.State == current!.State)
                {
                    consecutiveHits++;
                }
                else
                {
                    break;
                }
            }

            for (var i = 1; i < 3; i++)
            {
                var placedAt = GetGridSlot(
                    lastPlaceX + direction.Opposite.X * i,
                    lastPlaceY + direction.Opposite.Y * i);
                if (placedAt != null && placedAt.State == current!.State)
                {
                    consecutiveHits++;
                }
                else
                {
                    break;
                }
            }

            if (consecutiveHits >= 3)
            {
                _winner = _turn;
                break;
            }

            consecutiveHits = 1;
        }
    }

    private void _HandleInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
        {
            GameQuitRequestEvent(this, EventArgs.Empty);
        }
        
        if (!Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) return;

        var mouseX = Raylib.GetMouseX();
        var mouseY = Raylib.GetMouseY();

        if (_winner != null) return;

        foreach (var (region, index) in _slotRegions)
        {
            if (!region.IsPointWithin(mouseX, mouseY)) continue;

            var slot = _slots[index];

            if (slot.State != SlotState.None) continue;

            var xx = index % 3;
            var yy = index / 3;

            slot.Update(_turn.State, _turn.Color);
            _openSlotsRemaining--;

            _CheckAndUpdateWinner(xx, yy);

            if (_winner != null)
            {
                break;
            }

            _SwitchTurn();

            break;
        }
    }

    private void _DrawBoard()
    {
        foreach (var (region, value) in _slotRegions)
        {
            Raylib.DrawRectangle(region.X, region.Y, region.Width, region.Height,
                value % 2 == 0 ? Color.GRAY : Color.DARKGRAY);

            var slot = _slots[value];

            switch (slot.State)
            {
                case SlotState.None:
                    break;

                case SlotState.O:
                    Raylib.DrawText("O", region.X + 10, region.Y + 2, 48, slot.Color);
                    break;

                case SlotState.X:
                    Raylib.DrawText("X", region.X + 10, region.Y + 2, 48, slot.Color);
                    break;
            }
        }
    }

    private void _DrawGameOverGraphics()
    {
        var uiColor = _winner?.Color ?? Color.DARKPURPLE;

        var text = _winner != null ? $"Winner: {_winner.Name}!" : "Tie!";
        var width = Raylib.MeasureText(text, 24);
        var centerX = Raylib.GetScreenWidth() / 2;
        Raylib.DrawText(text, centerX - width / 2, 32, 24, uiColor);

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            var mouseX = Raylib.GetMouseX();
            var mouseY = Raylib.GetMouseY();

            if (_playAgainButtonRegion.IsPointWithin(mouseX, mouseY))
            {
                _ResetState();
                return;
            }

            if (_backToMenuButtonRegion.IsPointWithin(mouseX, mouseY))
            {
                GameQuitRequestEvent(this, EventArgs.Empty);
            }
        }

        const string retryText = "Retry";
        var retryTextWidth = Raylib.MeasureText(retryText, 16);

        Raylib.DrawText(retryText,
            _playAgainButtonRegion.X + _playAgainButtonRegion.Width / 2 - retryTextWidth / 2,
            _playAgainButtonRegion.Y + _playAgainButtonRegion.Height / 2 - 8, 16, Color.BLACK);

        Raylib.DrawRectangleLines(_playAgainButtonRegion.X, _playAgainButtonRegion.Y,
            _playAgainButtonRegion.Width, _playAgainButtonRegion.Height, uiColor);

        const string mainMenuText = "Main Menu";
        var mainMenuTextWidth = Raylib.MeasureText(mainMenuText, 16);

        Raylib.DrawText(mainMenuText,
            _backToMenuButtonRegion.X + _backToMenuButtonRegion.Width / 2 - mainMenuTextWidth / 2,
            _backToMenuButtonRegion.Y + _backToMenuButtonRegion.Height / 2 - 8, 16, Color.BLACK);

        Raylib.DrawRectangleLines(_backToMenuButtonRegion.X, _backToMenuButtonRegion.Y,
            _backToMenuButtonRegion.Width, _backToMenuButtonRegion.Height, uiColor);
    }
}