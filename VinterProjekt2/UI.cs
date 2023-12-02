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
        Raylib.DrawText("Press I for instructions!", 228, 550, 40, Color.RED);
        Raylib.EndDrawing();
    }

    public override void Logic(Level level)
    {
        base.Logic(level);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_I))
            GameManager.ChangeUI(4);
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

public class InfoScreen : UIscreen
{
    public InfoScreen(Player inPlayer)
    {
        player = inPlayer;
        button = new((GameManager.screenWidth / 2) - 110, (GameManager.screenHeight / 2) + 140, 220, 95);
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLUE);
        Raylib.DrawText("Use A/D or left/right arrow keys to move", 65, 180, 40, Color.BLACK);
        Raylib.DrawText("Press SPACE or Up arrow key to jump", 105, 220, 40, Color.BLACK);
        Raylib.DrawText("You can jump when moving against walls,", 95, 260, 40, Color.BLACK);
        Raylib.DrawText("but you have to keep pushing into them!", 90, 300, 40, Color.BLACK);
        Raylib.DrawText("Avoid the spikes as they kill you!", 130, 340, 40, Color.BLACK);
        Raylib.DrawRectangleRec(button, buttonColor);
        Raylib.DrawText("START", 433, 545, 40, Color.BLACK);
        Raylib.EndDrawing();
    }
}