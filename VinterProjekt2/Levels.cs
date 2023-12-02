using System.Runtime.InteropServices;
using Raylib_cs;

public class Level
{
    public const int blockWidth = GameManager.screenWidth / 16;
    public const int blockHeight = GameManager.screenHeight / 12;
    private const float parallaxFactor = 2.5f;

    public int[,] layout;
    public List<Rectangle> walls = new();
    public List<Rectangle> spikes = new();
    private Rectangle goal;

    public float bgOffset = 0;
    private int playerMovement;
    private int playerLastX;
    private Texture2D bg = Raylib.LoadTexture("wall.png");
    private Texture2D groundTile = Raylib.LoadTexture("groundtile.png");
    private Texture2D wallTile = Raylib.LoadTexture("walltile.png");
    private Texture2D spikeBall = Raylib.LoadTexture("spikeball.png");

    public void DrawBackground(Player player, Camera camera)
    {
        UpdatePlayerMovement(player, camera);

        bgOffset += parallaxFactor * playerMovement;

        for (int x = -2; x < layout.GetLength(1); x++)
        {
            for (int y = 0; y < 2; y++)
            {
                Raylib.DrawTexture(bg, (int)((x * bg.width) + bgOffset), y * bg.height, Color.WHITE);
            }
        }

        playerLastX = (int)player.playerRect.x;
    }

    private void UpdatePlayerMovement(Player player, Camera camera)
    {
        if (IsPlayerMovingRight(player) && camera.isTrackingPlayer)
        {
            playerMovement = 1;
        }
        else if (IsPlayerMovingLeft(player) && camera.isTrackingPlayer)
        {
            playerMovement = -1;
        }
        else
        {
            playerMovement = 0;
        }
    }

    private bool IsPlayerMovingRight(Player player)
    {
        return (Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            && (player.playerRect.x != playerLastX);
    }

    private bool IsPlayerMovingLeft(Player player)
    {
        return (Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            && (player.playerRect.x != playerLastX);
    }


    public virtual void DrawTiles()
    {
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++)
            {
                switch (layout[y, x])
                {
                    case 1:
                        Raylib.DrawTexture(groundTile, x * blockWidth, y * blockHeight, Color.WHITE);
                        break;

                    case 2:
                        Raylib.DrawTexture(wallTile, x * blockWidth, y * blockHeight, Color.WHITE);
                        break;
                    case 3:
                        Raylib.DrawTexture(spikeBall, x * blockWidth, y * blockHeight, Color.WHITE);
                        break;
                    case 4:
                        Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.GREEN);
                        break;

                }
            }
        }
    }

    public void GenerateRectangles()
    {
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++)
            {
                switch (layout[y, x])
                {
                    case 2:
                        walls.Add(new Rectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight));
                        break;
                    case 3:
                        spikes.Add(new Rectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight));
                        break;
                    case 4:
                        goal = new Rectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight);
                        break;
                }

            }
        }
    }

    public bool WinCheck(Player p)
    {
        return Raylib.CheckCollisionRecs(p.playerRect, goal);
    }
}

public class LevelOne : Level
{
    public LevelOne()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        GenerateRectangles();
    }
}

public class LevelTwo : Level
{
    public LevelTwo()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        GenerateRectangles();
    }
}
