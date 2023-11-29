using Raylib_cs;
using System.Numerics;

public class Player
{
    //Logic
    public Rectangle playerRect = new(0, 0, 50, 75);
    private bool isGrounded;
    public bool canJump;
    private int direction;
    private float verticalVelocity;
    private Vector2 lastPosition;

    private void Gravity(Level l)
    {
        if ((Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) && canJump)
        {
            verticalVelocity = -10;
        }
        else if (isGrounded || GetWallCollide(l))
        {
            verticalVelocity = 0;
        }

        if (!isGrounded && verticalVelocity < 15)
        {
            verticalVelocity += 0.3f;
        }

        playerRect.y += verticalVelocity;
    }
    private void CheckGroundCollisions(Level l)
    {
        int playerBlockX = (int)(playerRect.x / Level.blockWidth);
        int playerBlockY = (int)((playerRect.y + playerRect.height) / Level.blockHeight);

        isGrounded = false;

        for (int offset = 0; offset < 2; offset++) //Kör koden 2 gånger så den kollar kollisioner på båda sidorna av spelaren
        {
            if (isGrounded) break;

            playerBlockX = (int)((playerRect.x + (playerRect.width * offset)) / Level.blockWidth); //Avgör plats för koll av kollision

            if (playerBlockX >= 0 && playerBlockX < l.layout.GetLength(1) && playerBlockY >= 0 && playerBlockY < l.layout.GetLength(0))
            {
                int blockType = l.layout[playerBlockY, playerBlockX];

                switch (blockType)
                {
                    case 0:
                        isGrounded = false;
                        canJump = false;
                        break;
                    case 3:
                        GameManager.currentState = GameManager.State.UIscreen;
                        GameManager.ChangeUI(2);
                        break;
                    case 4:
                        isGrounded = false;
                        canJump = false;
                        break;
                    default:
                        isGrounded = true;
                        canJump = true;
                        playerRect.y = playerBlockY * Level.blockHeight - playerRect.height;
                        break;

                }
            }
        }
    }
    private bool GetWallCollide(Level l)
    {
        return l.walls.Any(wallTile => Raylib.CheckCollisionRecs(playerRect, wallTile));
    }

    public void Movement(Level level)
    {
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

        if (GetWallCollide(level))
        {
            playerRect.x = lastPosition.X;
            canJump = true;
        }

        Gravity(level);
        CheckGroundCollisions(level);

        lastPosition.X = playerRect.x;
    }

    public void ResetCharacter()
    {
        direction = 1;
        isGrounded = false;
        currentSprite = 0;
        verticalVelocity = 0;
        playerRect.x = 0; playerRect.y = 0;
    }




    //Drawing
    private int frame = 1;
    private float elapsed = 0;
    private Texture2D[] sprites;
    public int currentSprite { get; set; }
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
    private void SpriteSelector(Level level)
    {
        if ((Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) ||
             Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) && isGrounded && !GetWallCollide(level))
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

    public void DrawCharacter(Level level)
    {
        SpriteSelector(level);
        Rectangle sourceRec = new Rectangle(0, 0, 50 * direction, 75);
        if (currentSprite == 1)
        {
            FrameLogic();
            sourceRec.x = frame * sourceRec.width;
        }
        Raylib.DrawTextureRec(sprite, sourceRec, new Vector2(playerRect.x, playerRect.y), Color.WHITE);
    }


    //Constructor
    public Player()
    {
        ResetCharacter();
        
        sprites = new Texture2D[]
        {Raylib.LoadTexture("character.png"),
        Raylib.LoadTexture("running.png"),
        Raylib.LoadTexture("air.png"),
        Raylib.LoadTexture("fall.png")};
    }
}