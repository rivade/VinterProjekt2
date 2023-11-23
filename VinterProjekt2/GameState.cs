using System.Diagnostics.Contracts;
using Raylib_cs;

public class Game
{
    public enum State
    {
        Start,
        Game,
        GameOver,
        Win
    }
    public static State currentState;

    public const int screenWidth = 1008;
    public const int screenHeight = 756;

    private Player p;
    private Level currentLevel;
    private LevelOne l1 = new();
    private LevelTwo l2 = new();

    public Game()
    {
        Raylib.InitWindow(Game.screenWidth, Game.screenHeight, "mongo");
        Raylib.SetTargetFPS(60);
        currentState = State.Game;
        currentLevel = l1;

        p = new Player();
    }

    private void GameLogic()
    {
        p.Movement(currentLevel);
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
                case State.Start:

                    break;
                
                case State.Game:
                    GameLogic();
                    DrawGame();
                    break;

                case State.GameOver:
                    
                    break;
                
                case State.Win:

                    break;
            }
        }
    }
}
