using Raylib_cs;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

public class Player
{
    // Logic

    // Diverse variabler för spelaren (ganska självklart vad de flesta är)
    public Rectangle playerRect = new(0, 0, 50, 75);
    private bool isGrounded; //Grounded i detta fall är ifall spelaren står på något
    private bool wasGrounded;
    private bool wasOnWall;
    public bool canJump;
    public int direction; //Int som är 1 eller -1 beroende på vilket håll spelaren kollar åt (används vid utritning av sprite)
    private float verticalVelocity;
    private Vector2 lastPosition; //Vector med spelarens senaste position

    // Klassinstanser för animatör och coyotetimer
    private CoyoteTimer coyoteTimer;
    private AnimationController anim;

    // Metod för spelarens gravitation
    private void Gravity(Level l)
    {
        if ((Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) || Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) && canJump) //Om spelaren kan hoppa och trycker på hopp-knapp
        {
            canJump = false; //Kan den inte längre hoppa (Den är i luften)
            verticalVelocity = -8.5f; //Gör att spelaren åker upp i luften
        }
        else if (isGrounded || GetWallCollide(l))
        {
            verticalVelocity = 0; //Spelaren ska inte röra sig när den slår i tak eller står på något
        }
        else if (playerRect.y <= 0) //Spelaren ska inte kunna åka genom toppen av skärmen
        {
            verticalVelocity = 0;
            playerRect.y = 0;
        }

        if (!isGrounded && verticalVelocity < 15) //Gör att gravitationen ökar när spelaren är i luften med en högsta cap på 15
        {
            verticalVelocity += 0.3f;
        }

        playerRect.y += verticalVelocity; //Uppdaterar spelarens Y-position med den nya vertikala velociteten
    }
    // Metod som kollar om spelaren står på något
    private void CheckGroundCollisions(Level l)
    {
        int playerBlockX = (int)(playerRect.x / Level.blockWidth); //X värde för spelarens 'fötter'
        int playerBlockY = (int)((playerRect.y + playerRect.height) / Level.blockHeight); //Y värde för spelarens 'fötter'

        isGrounded = false; //Isgrounded är by default falskt (Om man står på något ändras detta senare)

        for (int offset = 0; offset < 2; offset++) //Kör koden 2 gånger så den kollar kollisioner på båda sidorna av spelaren (Tänk vänster och höger fot)
        {
            if (isGrounded) break; //Om spelaren redan står på något vid en 'fot' behöver den inte kolla andra

            playerBlockX = (int)((playerRect.x + (playerRect.width * offset)) / Level.blockWidth); //Avgör plats för koll av kollision relativt till vilken fot den ska kolla

            int blockType = l.layout[playerBlockY, playerBlockX]; //Kollar vilket block som är under spelaren

            switch (blockType)
            {
                case 0:
                    isGrounded = false; //Spelaren är i luften (0 i matrisen betyder luft)
                    break;
                case 3:
                    return; //Spelaren är på en tagg, annan kod ska köras, så att den kan sluta köra metoden
                case 4:
                    return; //Samma som ovan fast med portalen
                default: //Spelaren är antingen på mark eller på ett väggblock
                    isGrounded = true;
                    canJump = true;
                    playerRect.y = playerBlockY * Level.blockHeight - playerRect.height; //Gör så att spelaren inte fastnar i ett block (Återställer Y-position)
                    break;

            }
        }
    }
    // Metod som kollar om spelaren kolliderar med en vägg.
    private bool GetWallCollide(Level l)
    {
        return l.walls.Any(wallTile => Raylib.CheckCollisionRecs(playerRect, wallTile)); //Kolliderar spelaren med någon väggtile i levelns vägg-lista returnar den true, annars false
    }
    // Metod som kollar om spelaren kolliderar med nån av spikesen, fungerar på samma sätt som metoden ovan men med annan kod som ska köras vid true
    public void CheckSpikeDeath(Level l)
    {
        if (l.spikes.Any(spike => Raylib.CheckCollisionRecs(playerRect, spike)))
        {
            Raylib.PlaySound(SoundController.sounds[1]);
            GameManager.ChangeUI(1);
            GameManager.ChangeState(GameManager.State.UIscreen);
        }
    }

    // Metoden som hanterar spelarens rörelse
    public void Movement(Level level)
    {
        if ((Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)) && (playerRect.x + playerRect.width) <= (level.layout.GetLength(1) * Level.blockWidth)) //Den sista delen gör så att spelaren inte kan gå utanför banan åt höger
        {
            direction = 1;
            playerRect.x += 5;
        }
        else if ((Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) && playerRect.x >= 0) //Samma som ovan men åt vänster
        {
            direction = -1;
            playerRect.x -= 5;
        }

        if (GetWallCollide(level)) //Om spelaren kolliderar med en vägg
        {
            playerRect.x = lastPosition.X; //Återställer spelarens position så att den inte går igenom väggen
            canJump = true; //Spelaren kan hoppa på väggen
            wasOnWall = true; //Spelaren var/är på vägg
        }
        else if (wasOnWall) //Om spelaren inte är på en vägg men var det framen innan är wasonwall true men GetWallCollide false, därmed körs denna kod
        {
            canJump = false;
            wasOnWall = false; //Återställer wasonwall
        }

        Gravity(level); //Kör gravitationsmetoden
        wasGrounded = isGrounded; //Wasgrounded blir per default det som spelarens isgrounded värde var
        CheckGroundCollisions(level); //Kollar mark-kollissioner, ändrar eventuellt isgrounded värde vilket gör att wasgrounded är true men isgrounded inte är det

        // Kollar om spelaren nyss lämnade en plattform
        if (wasGrounded && !isGrounded && verticalVelocity == 0) //Funkar för att när man lämnar en plattform är verticalVelocity alltid 0 (Förhindrar timern från att starta när man exempelvis hoppar)
        {
            coyoteTimer.StartTimer(); //Startar coyotetimern
        }

        lastPosition.X = playerRect.x; //Settar värdet för spelarens senaste position
    }

    // Återställer spelarvariablerna till defaultvärden
    public void ResetCharacter(Level l) 
    {
        direction = 1;
        isGrounded = false;
        canJump = false;
        wasOnWall = false;
        CurrentSprite = 0;
        verticalVelocity = 0;
        playerRect.x = 0; playerRect.y = (GameManager.screenHeight - Level.blockHeight) - playerRect.height;
        try //Försöker återställa bakgrundens parallaxeffekt (Körs här för att ResetCharacter körs när man dör, och då måste även bakgrundens parallax återställas)
        {
            l.parallaxOffset = 0;
        }
        catch (NullReferenceException) //Om ingen level angavs vid körning av denna metod görs ingenting (Exempelvis när man startar appen och player-instansen skapas)
        {
            return;
        }
    }




    //Drawing
    private Texture2D[] sprites; 
    public int CurrentSprite { get; set; } //Int för spelarens sprite som ska användas
    private Texture2D Sprite
    {
        get
        {
            return sprites[CurrentSprite]; //Returnar motsvarande värde i sprites-listan
        }

        set { }
    }

    // Metod som avgör vilken sprite som ska användas för spelaren
    private void SpriteSelector(Level level)
    {
        if ((Raylib.IsKeyDown(KeyboardKey.KEY_D) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) ||
             Raylib.IsKeyDown(KeyboardKey.KEY_A) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) && isGrounded && !GetWallCollide(level))
        {
            CurrentSprite = 1;
        }

        else if (wasOnWall)
        {
            CurrentSprite = 4;
        }

        else if (!isGrounded && verticalVelocity < 0)
        {
            CurrentSprite = 2;
        }

        else if (!isGrounded && verticalVelocity > 0)
        {
            CurrentSprite = 3;
        }
        else
        {
            CurrentSprite = 0;
        }
    }

    // Metod som ritar ut spelaren
    public void DrawCharacter(Level level)
    {
        SpriteSelector(level); //Väljer spriten
        Rectangle sourceRec = new Rectangle(0, 0, 50 * direction, 75); //Source-rektangel för animation och riktning, Negativ bredd (avgörs av direction) innebär spegelvänd sprite
        if (CurrentSprite == 1) //Bara sprite 1 ska animeras
        {
            anim.FrameLogic();
            sourceRec.x = anim.frame * Math.Abs(sourceRec.width); //Absolutvärdet förhindrar spriten från att springa baklänges
        }

        Raylib.DrawTextureRec(Sprite, sourceRec, new Vector2(playerRect.x, playerRect.y), Color.WHITE); //Ritar ut spelaren
    }


    //Konstruktor
    public Player()
    {
        coyoteTimer = new(this); //Skapar ny instans av coyotetimern med denna specifika instans av spelaren
        anim = new(0.07f, 12);

        ResetCharacter(null); //Settar spelarens position till standardvärden när spelaren skapas

        sprites = new Texture2D[]
        {Raylib.LoadTexture("Sprites/character.png"),
        Raylib.LoadTexture("Sprites/running.png"),
        Raylib.LoadTexture("Sprites/air.png"),
        Raylib.LoadTexture("Sprites/fall.png"),
        Raylib.LoadTexture("Sprites/onwall.png")};
    }
}