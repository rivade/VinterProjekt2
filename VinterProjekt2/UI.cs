using Raylib_cs;
using System.Numerics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

public class UIscreen
{
    const int lineHeight = 40;

    protected Player player;
    protected Texture2D background;
    protected Rectangle button;
    protected Color buttonColor;
    protected string[] textMessages;


    public virtual void Logic(Level level)
    {
        buttonColor = new(137, 137, 137, 255);
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button))
        {
            buttonColor = new(171, 171, 171, 255);
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Raylib.PlayMusicStream(SoundController.backgroundMusic);
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

    protected void DrawCenteredText(int startY, Color textcolor)
    {
        for (int i = 0; i < textMessages.Length; i++)
        {
            int textWidth = Raylib.MeasureText(textMessages[i], 40);
            int startX = (GameManager.screenWidth - textWidth) / 2;

            Raylib.DrawText(textMessages[i], startX, startY + i * lineHeight, 40, textcolor);
        }
    }
}

public class StartScreen : UIscreen
{
    Rectangle sourceRec;
    Texture2D titleText = Raylib.LoadTexture("Sprites/titletext.png");

    public StartScreen(Player inPlayer)
    {
        player = inPlayer;
        background = Raylib.LoadTexture("Backgrounds/startscreen.png");
        button = new((GameManager.screenWidth / 2) - 110, (GameManager.screenHeight / 2) + 40, 220, 95);
        sourceRec = new Rectangle(0, 0, GameManager.screenWidth, GameManager.screenHeight);

        textMessages = new string[]
        {
            "Press I for instructions",
            "Press L for level selection"
        };
    }

    public override void Draw()
    {
        sourceRec.x += 0.5f; //Sourcerektangelns x-värde motsvarar offseten i bakgrunds skrollningen.
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawTextureRec(background, sourceRec, new Vector2(0, 0), Color.WHITE);
        Raylib.DrawRectangleRec(button, buttonColor);
        Raylib.DrawTexture(titleText, GameManager.screenWidth / 2 - titleText.width / 2, 200, Color.WHITE);
        Raylib.DrawText("START", (int)button.x + 23, (int)button.y + 25, 50, Color.BLACK);
        DrawCenteredText(550, Color.RED);
        Raylib.EndDrawing();
    }

    public override void Logic(Level level)
    {
        base.Logic(level);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_I))
            GameManager.ChangeUI(3);

        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_L))
            GameManager.ChangeUI(4);

    }
}

public class GameOverScreen : UIscreen
{
    public GameOverScreen(Player inPlayer)
    {
        player = inPlayer;
        background = Raylib.LoadTexture("Backgrounds/deathscreen.png");
        button = new((GameManager.screenWidth / 2) + 175, (GameManager.screenHeight / 2) + 140, 250, 95);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("You died!", 640, 300, 75, Color.RED);
        Raylib.DrawText("Restart", (int)(button.x + 27), (int)(button.y + 25), 50, Color.BLACK);
        Raylib.EndDrawing();
    }
}

public class WinScreen : UIscreen
{
    public WinScreen()
    {
        button = new((GameManager.screenWidth / 2) + 100, (GameManager.screenHeight / 2) + 140, 250, 95);
        background = Raylib.LoadTexture("Backgrounds/winscreen.png");
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText("You win!", 560, 300, 90, Color.GOLD);
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
                GameManager.ChangeUI(0);
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
        textMessages = new string[]
        {
            "Use A/D or left/right arrow keys to move",
            "Press SPACE or Up arrow key to jump",
            "You can jump when moving against walls,",
            "but you have to keep pushing into them!",
            "Avoid the spikes as they kill you!",
            "Get to the portal at the end to win.",
            "Press M at any time to return to menu!",
        };
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLUE);
        DrawCenteredText(200, Color.BLACK);
        Raylib.DrawRectangleRec(button, buttonColor);
        Raylib.DrawText("START", 433, 545, 40, Color.BLACK);
        Raylib.EndDrawing();
    }
}

public class LevelSelector : UIscreen
{
    private List<Rectangle> buttons = new();
    const int buttonWidth = 250;
    const int buttonHeight = 100;
    const int buttonPadding = 20;
    const int centeringOffset = 150;

    public LevelSelector(Player inPlayer)
    {
        player = inPlayer;

        for (int y = 1; y < 3; y++)
        {
            for (int x = 1; x < 4; x++)
            {
                int buttonX = x * (buttonWidth + buttonPadding) - centeringOffset;
                int buttonY = y * (buttonHeight + buttonPadding) + centeringOffset;
                buttons.Add(new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight));
            }
        }
    }

    public override void Logic(Level level)
    {
        Vector2 mouse = Raylib.GetMousePosition();

        for (int index = 0; index < buttons.Count; index++)
        {
            Rectangle button = buttons[index];

            if (Raylib.CheckCollisionPointRec(mouse, button) && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                player.ResetCharacter(level);
                GameManager.ChangeLevel(index);
                Raylib.PlayMusicStream(SoundController.backgroundMusic);
                GameManager.currentState = GameManager.State.Game;
            }
        }
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLUE);
        for (int index = 0; index < buttons.Count; index++)
        {
            Rectangle button = buttons[index];

            Raylib.DrawRectangleRec(button, Color.GRAY);
            Raylib.DrawText($"Level {index + 1}", (int)button.x + 35, (int)button.y + 28, 50, Color.BLACK);
        }
        Raylib.EndDrawing();
    }
}