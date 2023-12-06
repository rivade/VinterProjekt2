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

    private Level currentLevel
    {
        get
        {
            try
            {
                return levels[levelInt];
            }
            catch (IndexOutOfRangeException) //Spelaren har vunnit. Blir IndexOutOfRangeException eftersom levelint i detta fall blir större än levels-arrayens längd. (jag behövde en try catch i mitt spel för kunskapskrav okej)
            {
                ChangeUI(2);
                ChangeLevel(0);
                Raylib.PlaySound(SoundController.sounds[2]);
                currentState = State.UIscreen;
                return levels[levelInt];
            }
        }
    }
    private Level[] levels;
    public static int levelInt;

    private UIscreen currentUI
    {
        get
        {
            return uiScreens[uiInt];
        }
    }
    private UIscreen[] uiScreens;
    private static int uiInt;



    public GameManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Jumpman 2");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);
        Raylib.SetMasterVolume(1);
        SoundController.SoundInit();

        currentState = State.UIscreen;
        _player = new Player();
        _camera = new Camera(_player);

        levels = new Level[] { new LevelOne(), new LevelTwo(), new LevelThree(), new LevelFour(), new LevelFive(), new LevelSix() };
        levelInt = 0;

        uiScreens = new UIscreen[] { new StartScreen(_player), new GameOverScreen(_player), new WinScreen(), new InfoScreen(_player), new LevelSelector(_player) };
        uiInt = 0;

        _camera.InitializeCamera();
    }

    public static void ChangeUI(int uiSelection)
    {
        uiInt = uiSelection;
    }

    public static void ChangeLevel(int levelSelection)
    {
        levelInt = levelSelection;
    }

    private void GameLogic()
    {
        _camera.CameraBounds(currentLevel.layout.GetLength(1) * Level.blockWidth);
        _player.Movement(currentLevel);
        _player.CheckSpikeDeath(currentLevel);
        if (currentLevel.WinCheck(_player))
        {
            if (levelInt != (levels.Length - 1))
                Raylib.PlaySound(SoundController.sounds[0]);
            _player.ResetCharacter(currentLevel);
            ChangeLevel(levelInt + 1);
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
        Raylib.DrawText($"Level {levelInt + 1}", 875, 10, 30, Color.BLACK);
        Raylib.EndDrawing();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateMusicStream(SoundController.backgroundMusic);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
            {
                ChangeUI(0);
                currentState = State.UIscreen;
            }

            switch (currentState)
            {
                case State.UIscreen:
                    currentUI.Logic(currentLevel);
                    currentUI.Draw();
                    Raylib.PauseMusicStream(SoundController.backgroundMusic);
                    break;

                case State.Game:
                    GameLogic();
                    DrawGame();
                    Raylib.ResumeMusicStream(SoundController.backgroundMusic);
                    break;
            }
        }
    }
}
