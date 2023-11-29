using System.Diagnostics.Contracts;
using System.Numerics;
using Raylib_cs;

public class GameManager
{
    public enum State
    {
        Game,
        UIscreen
    }
    public static State currentState;

    public const int screenWidth = 1008;
    public const int screenHeight = 756;

    private Player p;
    private static UIscreen currentUI;
    private static StartScreen startScreen = new();
    private static GameOverScreen gameOverScreen = new();
    private static WinScreen winScreen = new();
    private Level currentLevel;
    private LevelOne l1;
    private LevelTwo l2;



    public GameManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "mongo");
        Raylib.SetTargetFPS(60);
        currentState = State.UIscreen;
        currentUI = startScreen;

        l1 = new(l2);
        l2 = new(null);
        currentLevel = l1;

        p = new Player();
    }

    public static void ChangeUI(int uiSelector)
    {
        switch (uiSelector)
        {
            case 1:
                currentUI = startScreen;
                break;

            case 2:
                currentUI = gameOverScreen;
                break;

            case 3:
                currentUI = winScreen;
                break;
        }
    }

    private void GameLogic()
    {
        p.Movement(currentLevel);
        if (currentLevel.WinCheck(p))
        {
            p.playerRect.x = 0;
            p.playerRect.y = 0;
            currentLevel = currentLevel.nextLevel;
        }
    }

    private void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        p.DrawCharacter(currentLevel);
        currentLevel.DrawLevel();
        Raylib.EndDrawing();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            switch (currentState)
            {
                case State.UIscreen:
                    currentUI.Logic();
                    currentUI.Draw();
                    break;
                case State.Game:
                    GameLogic();
                    DrawGame();
                    break;
            }
        }
    }
}
