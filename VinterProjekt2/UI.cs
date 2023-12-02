using Raylib_cs;
using System.Numerics;

public class UIscreen
{
    protected Player player;
    protected Texture2D background;
    protected Rectangle button;
    protected Color buttonColor;


    public virtual void Logic(Level level)
    {
        buttonColor = new(137, 137, 137, 255);
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttonColor = new(171, 171, 171, 255);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                player.ResetCharacter(level);
                GameManager.currentState = GameManager.State.Game;
            }
        }
    }

    public virtual void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawTexture(background, 0, 0, Color.WHITE);
        Raylib.DrawRectangleRec(button, buttonColor);
    }
}

public class StartScreen : UIscreen
{
    public StartScreen(Player inPlayer)
    {
        player = inPlayer;
        background = Raylib.LoadTexture("startscreen.png");
        button = new((GameManager.screenWidth / 2) - 110, (GameManager.screenHeight / 2) + 40, 220, 95);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("Jumper!", 363, 200, 75, Color.WHITE);
        Raylib.DrawText("START", 433, 445, 40, Color.BLACK);
        Raylib.EndDrawing();
    }
}

public class GameOverScreen : UIscreen
{
    public GameOverScreen(Player inPlayer)
    {
        player = inPlayer;
        background = Raylib.LoadTexture("deathscreen.png");
        button = new((GameManager.screenWidth / 2) + 175, (GameManager.screenHeight / 2) + 140, 250, 95);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("Restart", (int)(button.x + 27), (int)(button.y + 25), 50, Color.BLACK);
        Raylib.EndDrawing();
    }
}

public class WinScreen : UIscreen
{
    public WinScreen()
    {
        button = new((GameManager.screenWidth / 2) + 175, (GameManager.screenHeight / 2) + 140, 250, 95);
        background = Raylib.LoadTexture("winscreen.png");
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("Menu", (int)(button.x + 60), (int)(button.y + 25), 50, Color.BLACK);
        Raylib.EndDrawing();
    }
    public override void Logic(Level l)
    {
        buttonColor = new(137, 137, 137, 255);
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttonColor = new(171, 171, 171, 255);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {   
                GameManager.ChangeUI(1);
            }
        }
    }
}