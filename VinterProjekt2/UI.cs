using Raylib_cs;
using System.Numerics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

// Klasser för spelets olika UI-skärmar
public class UIscreen
{
    // Används för centrerad-text-metoden.
    const int lineHeight = 40; //Avgör avstånd mellan vajre textrad.
    protected string[] textMessages; //Array med texten som ska vara centrerad

    // Ganska självklart vad dessa är
    protected Player player;
    protected Texture2D background;
    protected Rectangle button;
    protected Color buttonColor;

    // Hanterar logik för UI-skärmarna
    public virtual void Logic(Level level)
    {
        buttonColor = new(137, 137, 137, 255); //Standardfärg för knappen
        Vector2 mouse = Raylib.GetMousePosition();
        if (Raylib.CheckCollisionPointRec(mouse, button)) //Kollar om man hovrar musen på knappen
        {
            buttonColor = new(171, 171, 171, 255); //Gör knappfärgen ljusare i detta fall
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Raylib.PlayMusicStream(SoundController.backgroundMusic); //Startar bakgrundsmusik
                player.ResetCharacter(level); //Återställer spelaren
                GameManager.ChangeState(GameManager.State.Game); //Startar spelet
            }
        }
    }

    // Metod som ritar ut skärmen (Overridas alltid för att varje skärm är unik, därför bara begindrawing här, enddrawing finns i de egna draw-metoderna)
    public virtual void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        Raylib.DrawTexture(background, 0, 0, Color.WHITE);
        Raylib.DrawRectangleRec(button, buttonColor);
    }

    // Metod som ritar ut centrerad text
    protected void DrawCenteredText(int startY, Color textcolor) //startY avgör vart texten ska börja ritas.
    {
        for (int i = 0; i < textMessages.Length; i++) //Går igenom alla textrader i textmessages-arrayen
        {
            int textWidth = Raylib.MeasureText(textMessages[i], 40); //Kollar textens bredd
            int startX = (GameManager.screenWidth - textWidth) / 2; //Avgör därefter vart texten ska börja ritas

            Raylib.DrawText(textMessages[i], startX, startY + i * lineHeight, 40, textcolor); //Ritar ut texten på rätt plats.
        }
    }
}

public class StartScreen : UIscreen
{
    // Används för bakgrundens rörelse
    Rectangle sourceRec;

    // Titeltext
    Texture2D titleText = Raylib.LoadTexture("Sprites/titletext.png");

    // Constructor
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

        // Dessa nya används för att startmenyn har två extra alternativ
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_I))
            GameManager.ChangeUI(3);

        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_L))
            GameManager.ChangeUI(4);

    }
}

public class GameOverScreen : UIscreen
{
    // Constructor
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
    // Constructor
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

    // Logic skiljer sig här då den ska göra så att man går tillbaka till startmenyn istället för att börja spelet
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
    // Constructor
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
            "Get to the portal at the end to win",
            "Press M at any time to return to menu!",
            "(HINT) this game has coyotetime"
        };
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLUE);
        DrawCenteredText(150, Color.BLACK);
        Raylib.DrawRectangleRec(button, buttonColor);
        Raylib.DrawText("START", 433, 545, 40, Color.BLACK);
        Raylib.EndDrawing();
    }
}

public class LevelSelector : UIscreen
{
    // Värden för denna klass knappar (denna har fler än 1 därför skiljer sig detta)
    private List<Rectangle> buttons = new();
    const int buttonWidth = 250;
    const int buttonHeight = 100;
    const int buttonPadding = 20; //Avgör avstånd mellan knapparna
    const int centeringOffset = 150; //Värde som centrerar knapparna (inte exakt men hann inte fixa metod för att göra detta exakt)

    // Constructor
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

    public override void Logic(Level level) //Denna metod skiljer sig pga flera knappar
    {
        Vector2 mouse = Raylib.GetMousePosition();

        for (int index = 0; index < buttons.Count; index++) //Kollar muspositionen relativt för varje knapp
        {
            Rectangle button = buttons[index]; //Aktiv knapp i loopen

            if (Raylib.CheckCollisionPointRec(mouse, button) && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                player.ResetCharacter(level);
                GameManager.ChangeLevel(index);
                Raylib.PlayMusicStream(SoundController.backgroundMusic);
                GameManager.ChangeState(GameManager.State.Game);
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