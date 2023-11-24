using Raylib_cs;
using System.Numerics;

public class UIscreen
{
    protected Texture2D background;

    public virtual void Logic()
    {}
    public virtual void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawTexture(background, 0, 0, Color.WHITE);
    }
}

public class StartScreen : UIscreen
{
    private Rectangle button = new((GameManager.screenWidth / 2) - 110, (GameManager.screenHeight / 2) + 40, 220, 95);
    public StartScreen()
    {
        background = Raylib.LoadTexture("startscreen.png");
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("Jumper!", 363, 200, 75, Color.WHITE);
        Raylib.DrawRectangleRec(button, Color.GRAY);
        Raylib.DrawText("START", 433, 445, 40, Color.BLACK);
        Raylib.EndDrawing();
    }
    public override void Logic()
    {
        Vector2 mouse = Raylib.GetMousePosition(); //Skapar en vektor med musens position
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                GameManager.currentState = GameManager.State.Game;
            }
        }
    }
}

public class GameOverScreen : UIscreen
{
    public GameOverScreen()
    {
        background = Raylib.LoadTexture("deathscreen.png");
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.EndDrawing();
    }
    public override void Logic()
    {}
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
    {}
}