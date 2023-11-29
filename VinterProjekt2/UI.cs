using Raylib_cs;
using System.Numerics;

public class UIscreen
{
    protected Player p;
    protected Texture2D background;
    protected Rectangle button;


    public virtual void Logic()
    {
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {  
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                GameManager.currentState = GameManager.State.Game;
            }
        }
    }

    public virtual void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawTexture(background, 0, 0, Color.WHITE);
        Raylib.DrawRectangleRec(button, Color.GRAY);
    }
}

public class StartScreen : UIscreen
{
    public StartScreen()
    {
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
        p = inPlayer;
        background = Raylib.LoadTexture("deathscreen.png");
        button = new((GameManager.screenWidth / 2), (GameManager.screenHeight / 2) + 40, 220, 95);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.EndDrawing();
    }

    public override void Logic()
    {
        p.ResetCharacter();
        base.Logic();
    }
}

public class WinScreen : UIscreen
{
    public WinScreen()
    {
        background = Raylib.LoadTexture("");
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.EndDrawing();
    }
    public override void Logic()
    {
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {  
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                GameManager.ChangeUI(1);
            }
        }
    }
}