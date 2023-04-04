using GameApp.Game;
using Raylib_cs;

namespace GameApp.menu;

public partial class Menu
{
    private GameManager _gameManager;

    public Menu(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        
    }

    public void UpdateAndDraw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        
        
        
        Raylib.EndDrawing();
    }
}