using System.Runtime.Serialization;
using GameApp.Game;
using GameApp.Game.Impl.T3;
using GameApp.menu;
using GameApp.Utils;
using Raylib_cs;

// var reader = new CommandLineReader(RunTask);
//
// reader.Start();
//
// void RunTask(string command, string[] args)
// {
//     Console.WriteLine($"Executing task on {Thread.CurrentThread.Name}");
// }

var logger = new Logger(new DateTimeFormat("MM/dd/yyyy H:mm:ss"));
var gameManager = new GameManager(logger);

gameManager.RegisterGames(new List<Type>
{
    typeof(T3Game)
});

gameManager.StartGame(typeof(T3Game));

var menu = new Menu(gameManager);

Raylib.InitWindow(600, 320, "C# Assignment 1");
Raylib.SetExitKey(0);

while (!Raylib.WindowShouldClose())
{
    var activeGame = gameManager.ActiveGame;
    
    if (activeGame != null)
    {
        activeGame.UpdateAndDraw();
    }
    else
    {
        menu.UpdateAndDraw();
    }
}

gameManager.ActiveGame?.Stop();

Raylib.CloseWindow();