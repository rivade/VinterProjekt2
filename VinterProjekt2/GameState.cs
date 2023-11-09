using Raylib_cs;

public class GameState
{
    public const int screenWidth = 1024;
    public const int screenHeight = 768;

    private Player p = new();
    private LevelOne l1 = new();
    private LevelTwo l2 = new();
    private Level currentLevel;

    public GameState()
    {
        Raylib.InitWindow(GameState.screenWidth, GameState.screenHeight, "mongo");
        Raylib.SetTargetFPS(60);
        currentLevel = l1;
    }

    private void Logic()
    {
        p.Movement();
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
        }
    }
}
