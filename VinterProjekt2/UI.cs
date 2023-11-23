using Raylib_cs;

public class UIscreen
{
    protected Texture2D background;
    public virtual void Draw()
    {
        Raylib.DrawTexture(background, 0, 0, Color.WHITE);
    }
}

public class StartScreen : UIscreen
{
    public StartScreen()
    {
        
    }
}

public class GameOverScreen : UIscreen
{
    public GameOverScreen()
    {}
}

public class WinScreen : UIscreen
{
    public WinScreen()
    {}
}