using Raylib_cs;
using System.Numerics;

public class Player
{
    //Logic
    public Rectangle playerRect = new(0, 0, 50, 75);
    private int direction;
    private bool isGrounded;
    private float verticalVelocity;

    private void Gravity()
    {
        if ((Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) && isGrounded)
        {
            verticalVelocity = -10;
        }
        else if (isGrounded)
        {
            verticalVelocity = 0;
        }

        if (!isGrounded && verticalVelocity < 15)
        {
            verticalVelocity += 0.3f;
        }

        playerRect.y += verticalVelocity;
    }
    private void CheckCollisions()
    {
        Gravity();
    }

    public void Movement()
    {
        CheckCollisions();
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            direction = 1;
            playerRect.x += 5;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            direction = -1;
            playerRect.x -= 5;
        }
    }



    //Drawing
    private int frame = 1;
    private float elapsed = 0;
    private Texture2D[] sprites;
    private int currentSprite { get; set; }
    private Texture2D sprite
    {
        get
        {
            return sprites[currentSprite];
        }

        set { }
    }
    private void FrameLogic()
    {
        const float frameDuration = 0.07f;
        elapsed += Raylib.GetFrameTime();

        if (elapsed >= frameDuration)
        {
            frame++;
            elapsed -= frameDuration;
        }

        frame %= 12;
    }
    private void SpriteSelector()
    {
        if ((Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) ||
             Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) && isGrounded)
        {
            currentSprite = 1;
        }

        else if (!isGrounded && verticalVelocity < 0)
        {
            currentSprite = 2;
        }

        else if (!isGrounded && verticalVelocity > 0)
        {
            currentSprite = 3;
        }

        else
        {
            currentSprite = 0;
        }
    }

    public void DrawCharacter()
    {
        SpriteSelector();
        Rectangle sourceRec = new Rectangle(0, 0, 50 * direction, 75);
        if (currentSprite == 1)
        {
            FrameLogic();
            sourceRec.x = frame * sourceRec.width;
        }
        Raylib.DrawTextureRec(sprite, sourceRec, new Vector2(playerRect.x, playerRect.y), Color.WHITE);
    }

    public Player()
    {
        direction = 1;
        isGrounded = false;
        currentSprite = 0;
        verticalVelocity = 0;

        sprites = new Texture2D[]
        {Raylib.LoadTexture("character.png"),
        Raylib.LoadTexture("running.png"),
        Raylib.LoadTexture("air.png"),
        Raylib.LoadTexture("fall.png")};
    }
}