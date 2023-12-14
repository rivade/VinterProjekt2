using System.Diagnostics.Contracts;
using System.Numerics;
using Raylib_cs;

public class GameManager
{
    public enum State
    {
        Game, //Denna state innefattar alla levlar, alltså själva spel-delen
        UIscreen //Innefattar de olika UI-skärmarna i spelet, typ startskärm och deathskärm
    }
    public static State currentState;

    public const int screenWidth = 1008; //Konstantvärden som ska kunna kommas åt i andra klasser.
    public const int screenHeight = 756;

    private Player _player;
    private Camera _camera;

    private Level currentLevel
    {
        get
        {
            try
            {
                return levels[levelInt]; //Försöker returna instansen i levels-arrayen som är vid den aktuella level-inten.
            }
            catch (IndexOutOfRangeException) //Spelaren har vunnit. Blir IndexOutOfRangeException eftersom levelint i detta fall blir större än levels-arrayens längd. (jag behövde en try catch i mitt spel för kunskapskrav okej)
            {
                ChangeUI(2);
                ChangeLevel(0);
                Raylib.PlaySound(SoundController.sounds[2]);
                ChangeState(State.UIscreen);
                //Denna kod ovan återställer spelets bana och skickar spelaren till win-skärmen

                return levels[levelInt];
            }
        }
    }
    private Level[] levels; //Array med spelets alla levlar
    public static int levelInt; //Avgör vilken level som är aktuell

    private UIscreen currentUI
    {
        get
        {
            return uiScreens[uiInt]; //Returnar instansen i uiScreens arrayen som är vid den aktuella UI-inten
        }
    }
    private UIscreen[] uiScreens; //Array med alla UI-skärmar
    private static int uiInt; //Avgör vilken UI-skärm som är aktuell



    public GameManager() //Konstruktor
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Jumpman 2");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);
        Raylib.SetMasterVolume(1);
        SoundController.SoundInit();
        //Initierar alla spelinställningar som krävs för Raylib.

        currentState = State.UIscreen;
        _player = new Player();
        _camera = new Camera(_player);

        levels = new Level[] { new LevelOne(), new LevelTwo(), new LevelThree(), new LevelFour(), new LevelFive(), new LevelSix() };
        levelInt = 0;
        //Skapar Level-arrayen och sätter aktuell level till första i arrayen. (Level 1)

        uiScreens = new UIscreen[] { new StartScreen(_player), new GameOverScreen(_player), new WinScreen(), new InfoScreen(_player), new LevelSelector(_player) };
        uiInt = 0;
        //Samma som ovan fast med UIskärmarna

        _camera.InitializeCamera();
        //Initierar kamerainstansens inställningar
    }

    public static void ChangeUI(int uiSelection) => uiInt = uiSelection; //Sätter aktuell UI till det nummer som används när metoden kallas

    public static void ChangeLevel(int levelSelection) => levelInt = levelSelection; //Samma som ovan med levlar.

    public static void ChangeState(GameManager.State newState) => currentState = newState; //Samma med state.
    

    private void GameLogic() //Hanterar all spellogik.
    {
        _camera.CameraBounds(currentLevel.layout.GetLength(1) * Level.blockWidth);
        _player.Movement(currentLevel);
        _player.CheckSpikeDeath(currentLevel);
        if (currentLevel.WinCheck(_player))
        {
            if (levelInt != (levels.Length - 1)) //Kollar så att win-ljudet inte spelas vid sista leveln då det krockar med annat ljud
                Raylib.PlaySound(SoundController.sounds[0]);

            _player.ResetCharacter(currentLevel);
            ChangeLevel(levelInt + 1); //Gör så aktuell level blir nästa i arrayen.
        }
    }

    private void DrawGame() //Ritar ut spelet, ganska självklart
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

    public void Run() //Kör spelet. Den metod som används i program.cs
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.UpdateMusicStream(SoundController.backgroundMusic);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_M)) //Gör så man alltid kan gå tillbaka till menyn oavsett state.
            {
                ChangeUI(0);
                ChangeState(State.UIscreen);
            }

            switch (currentState) //Bestämmer kod som ska köras för de olika "gamestatesen" - UISkärm och spelet i sig. 
            {
                case State.UIscreen:
                    currentUI.Logic(currentLevel);
                    currentUI.Draw();
                    Raylib.PauseMusicStream(SoundController.backgroundMusic);
                    break;

                case State.Game:
                    GameLogic();
                    DrawGame();
                    Raylib.ResumeMusicStream(SoundController.backgroundMusic); //Bakgrundsmusik ska bara spelas i levlarna, så spelar om state är game och pausar vid UI-screen
                    break;
            }
        }
    }
}
