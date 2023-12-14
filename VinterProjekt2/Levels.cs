using System.Runtime.InteropServices;
using System.Numerics;
using Raylib_cs;

public class Level
{
    // Konstantvärden som avgör bredd för varje 'tile' i banan
    public const int blockWidth = GameManager.screenWidth / 16;
    public const int blockHeight = GameManager.screenHeight / 12;

    // Värden som används för bakgrundens parallax effekt
    private const float parallaxFactor = 3.5f; //Faktorn för bakgrundens positionering relativt till spelaren
    public float parallaxOffset = 0; //Bakgrundens offset för positionen
    private int playerLastX; //Int som kontrollerar om spelaren har rört sig (Används för att kolla om bakgrunden ska flyttas)

    // Animator för portalen
    private AnimationController anim = new(0.07f, 5);

    // Levelstruktur
    public int[,] layout; //Matris för 'tilemapen'
    public List<Rectangle> walls = new(); //Lista med tilesen som ska fungera som väggar
    public List<Rectangle> spikes = new(); //Samma fast med spikar
    private Rectangle goal; //Den rektangel som spelaren måste nudda för att vinna banan

    // Texturer
    private Texture2D bg = Raylib.LoadTexture("Backgrounds/wall.png");
    private Texture2D portal = Raylib.LoadTexture("Sprites/portal.png");
    private Texture2D groundTile = Raylib.LoadTexture("Sprites/groundtile.png");
    private Texture2D wallTile = Raylib.LoadTexture("Sprites/walltile.png");
    private Texture2D spikeBall = Raylib.LoadTexture("Sprites/spikeball.png");

    // Metoder som ritar ut bakgrunden med tidigare nämnd parallaxeffekt
    public void DrawBackground(Player player, Camera camera)
    {
        int playerMovement = GetPlayerDirection(player, camera); //Hämtar int för playerns riktning

        parallaxOffset += parallaxFactor * playerMovement; //Avgör offset för bakgrundens position, playermovement är riktning för spelarens rörelse

        for (int x = -1; x < layout.GetLength(1); x++) //Ritar ut flera bakgrunder eftersom varje bild är mindre än skärmen
        {
            for (int y = 0; y < 2; y++)
            {
                Raylib.DrawTexture(bg, (int)((x * bg.width) + parallaxOffset), y * bg.height, Color.WHITE);
            }
        }

        playerLastX = (int)player.playerRect.x; //Sätter värdet för spelarens senaste position
    }
    private int GetPlayerDirection(Player player, Camera camera) //Returnerar en int med positivt eller negativt värde för att avgöra åt vilket håll bakgrunden ska röra sig
    {
        if (IsPlayerMovingRight(player) && camera.isTrackingPlayer) //Bakgrunden ska inte flyttas när kameran inte följer spelaren, därav är camera.isTrackingPlayer ett krav
        {
            return 1;
        }
        else if (IsPlayerMovingLeft(player) && camera.isTrackingPlayer)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    private bool IsPlayerMovingRight(Player player) //Kollar så att spelaren aktivt rör sig åt vänster (Behövs för att bakgrunden inte ska röra på sig när spelaren går mot en vägg)
    {
        return (Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            && (player.playerRect.x != playerLastX);
    }
    private bool IsPlayerMovingLeft(Player player) //Samma som ovan fast med vänster
    {
        return (Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            && (player.playerRect.x != playerLastX);
    }

    // Metod som ritar ut alla tiles i banan
    public virtual void DrawTiles()
    {
        Rectangle sourceRec = new Rectangle(0, 0, 63, 126); //Source-rektangel i portalens spritesheet
        
        // Uppdaterar frame för sourcerektangeln
        anim.FrameLogic();
        sourceRec.x = anim.frame * sourceRec.width;

        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++) //Kollar igenom varje värde i tilemapen och ritar ut en texture beroende på värdet
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
                        Raylib.DrawTextureRec(portal, sourceRec, new Vector2(x * blockWidth, (y * blockHeight) - blockHeight), Color.WHITE); //Y-pos ska vara ett block över än själva tilen
                        break;

                }
            }
        }
    }

    // Metod som genererar levelns olika rectangles
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

    // Metod som kollar om spelaren nuddat portalen.
    public bool WinCheck(Player p)
    {
        return Raylib.CheckCollisionRecs(p.playerRect, goal);
    }
}

public class LevelOne : Level
{
    public LevelOne()
    {
        layout = new int[,] //0 motsvarar luft, 1 motsvarar mark, 2 motsvarar vägg, 3 motsvarar spikes, 4 motsvarar portalen
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
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
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        GenerateRectangles();
    }
}

public class LevelThree : Level
{
    public LevelThree()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 4},
            {1, 1, 1, 3, 3, 3, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        GenerateRectangles();
    }
}

public class LevelFour : Level
{
    public LevelFour()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 3, 3, 0, 0, 2, 0, 0, 3, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 3, 3, 2, 3, 3, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1}
        };
        GenerateRectangles();
    }
}

public class LevelFive : Level
{
    public LevelFive()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 3, 3, 0, 0, 2},
            {0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 2},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 3, 3, 2},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 3, 3, 0, 0, 2, 2, 3, 3, 3, 3, 3, 2, 2, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        GenerateRectangles();
    }
}

public class LevelSix : Level
{
    public LevelSix()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 3, 3, 3, 3, 3, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 2, 3, 3, 3, 3, 3, 3, 2, 0, 0, 0, 0, 2, 0, 0, 3, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1}
        };
        GenerateRectangles();
    }
}
