using Raylib_cs;

public class UIscreen
{
    protected Texture2D background;
    protected string titleString;

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
    public StartScreen()
    {
        background = Raylib.LoadTexture("");
        titleString = "Jumper!";
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.EndDrawing();
    }
    public override void Logic()
    {}
}

public class GameOverScreen : UIscreen
{
    public GameOverScreen()
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