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

    private Player _player;
    private Camera _camera;
    private static UIscreen currentUI;
    private static StartScreen startScreen;
    private static GameOverScreen gameOverScreen;
    private static WinScreen winScreen;
    private static InfoScreen infoScreen;
    private Level currentLevel
    {
        get
        {
            try
            {
                return levels[levelInt];
            }
            catch (IndexOutOfRangeException) //Spelaren har vunnit. Blir IndexOutOfRangeException eftersom levelint i detta fall blir större än levels-arrayens längd.
            {
                ChangeUI(3);
                levelInt = 0;
                currentState = State.UIscreen;
                return levels[levelInt];
            }
        }
    }
    private LevelOne l1;
    private LevelTwo l2;
    private Level[] levels;
    private int levelInt;



    public GameManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "mongo");
        Raylib.SetTargetFPS(60);
        currentState = State.UIscreen;
        _player = new Player();
        _camera = new Camera(_player);

        startScreen = new(_player);
        gameOverScreen = new(_player);
        winScreen = new();
        infoScreen = new(_player);
        currentUI = startScreen;

        l1 = new();
        l2 = new();
        levels = new Level[] { l1, l2 };
        levelInt = 0;

        _camera.InitializeCamera();
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
            
            case 4:
                currentUI = infoScreen;
                break;
        }
    }

    private void ResetGame()
    {
        levelInt = 0;
        _player.ResetCharacter(currentLevel);
        ChangeUI(1);
        currentState = State.UIscreen;
    }

    private void GameLogic()
    {
        _camera.CameraBounds((currentLevel.layout.GetLength(1) * Level.blockWidth));
        _player.Movement(currentLevel);
        _player.CheckSpikeDeath(currentLevel);
        if (currentLevel.WinCheck(_player))
        {
            _player.ResetCharacter(currentLevel);
            levelInt++;
        }
    }

    private void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(_camera.c);
        Raylib.ClearBackground(Color.WHITE);
        currentLevel.DrawBackground(_player, _camera);
        currentLevel.DrawTiles();
        _player.DrawCharacter(currentLevel);
        Raylib.EndMode2D();
        Raylib.DrawFPS(10, 10);
        Raylib.DrawRectangle(855, 0, 153, 50, Color.GOLD);
        Raylib.DrawText($"Level {levelInt + 1}", 880, 10, 30, Color.BLACK);
        Raylib.EndDrawing();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            switch (currentState)
            {
                case State.UIscreen:
                    currentUI.Logic(currentLevel);
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
