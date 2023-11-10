using Raylib_cs;

public class GameState
{
    enum CurrentState
    {
        Start,
        Game,
        GameOver,
        Win
    }

    public const int screenWidth = 1024;
    public const int screenHeight = 768;

    private Player p;
    private Level currentLevel;
    private LevelOne l1 = new();
    private LevelTwo l2 = new();

    public GameState()
    {
        Raylib.InitWindow(GameState.screenWidth, GameState.screenHeight, "mongo");
        Raylib.SetTargetFPS(60);
        currentLevel = l1;

        p = new Player();
    }

    private void Logic()
    {
        p.Movement(currentLevel);
    }

    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        p.DrawCharacter();
        currentLevel.DrawLevel();
        Raylib.EndDrawing();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Logic();
            Draw();
            System.Console.WriteLine($"{p.playerRect.x}, {p.playerRect.y}");
        }
    }
}
