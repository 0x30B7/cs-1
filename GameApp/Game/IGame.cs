namespace GameApp.Game;

public interface IGame
{
    void Start();

    void Stop();

    void UpdateAndDraw();

    event EventHandler GameQuitRequestEvent;

}