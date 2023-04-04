using System.Collections.Specialized;
using GameApp.Utils;

namespace GameApp.Game;

public class GameManager
{
    private readonly Logger _logger;
    private readonly OrderedDictionary _games = new();
    public IGame? ActiveGame { get; private set; }
    public event EventHandler<int>? GameEndEvent; 

    public GameManager(Logger logger)
    {
        _logger = logger;
    }

    public void RegisterGames(IEnumerable<Type> games)
    {
        _games.Clear();
        foreach (var game in games)
        {
            _games.Add(game, () => (IGame)Activator.CreateInstance(game)!);
            _logger.Info($"Registering game '{game.Name}'");
        }
    }

    public void StartGame(Type gameType)
    {
        var gameProvider = (Func<IGame?>) _games[gameType]!;

        if (gameProvider == null)
        {
            throw new ArgumentException($"Game with type '{gameType.Name}' is not found.");
        }

        if (ActiveGame != null)
        {
            EndActiveGame();
        }

        var game = gameProvider.Invoke();

        if (game == null)
        {
            _logger.Warn($"Could not start game '{gameType.Name}'");
            return;
        }

        game.GameQuitRequestEvent += (_, _) =>
        {
            EndActiveGame();
        };
        
        _logger.Info($"Starting game '{game.GetType().Name}'");

        ActiveGame = game;
        ActiveGame.Start();
    }

    private void EndActiveGame()
    {
        if (ActiveGame == null)
        {
            throw new ArgumentException($"Attempting to end active game, but no game is active.");
        }
        
        GameEndEvent?.Invoke(this, 0);
        
        _logger.Info($"Stopping game '{ActiveGame.GetType().Name}'");
        ActiveGame.Stop();
        ActiveGame = null;
    }
}