using System;
using Raylib_cs;
using System.Numerics;

public class Camera
{
    Player player; //Referens till spelaren som kameran ska följa

    // Konstanter som används för att definiera kamerans beteende
    const int playerCameraOffset = 250; // Det avstånd som kameran ska ha från spelaren när den trackar spelaren
    const int playerMaxDistanceToLvlEnd = GameManager.screenWidth - playerCameraOffset; //Max tillåtna avstånd för spelaren till levelns slut (Används för att avgöra om kameran ska följa spelaren)

    public bool isTrackingPlayer; // En bool som indikerar om kameran för närvarande följer spelaren

    public Camera2D c = new(); // Skapar kameraobjektet

    // Konstruktor för kameran, tar emot gamemanagerns instans av spelaren
    public Camera(Player inPlayer)
    {
        player = inPlayer;
    }

    // Metod för att initialisera kamerans inställningar
    public void InitializeCamera()
    {
        c.zoom = 1;
        c.offset = new Vector2(GameManager.screenWidth / 2, GameManager.screenHeight / 2); //Centrerar kameran
    }

    // Metod för att definiera begränsningar för kamerans rörelse baserat på spelarens position och nivåns bredd
    public void CameraBounds(int levelWidth)
    {
        // Om spelaren har passerat en viss punkt och avståndet till slutet av nivån är tillräckligt stort
        if (player.playerRect.x >= 265 && (levelWidth - player.playerRect.x >= playerMaxDistanceToLvlEnd))
        {
            isTrackingPlayer = true; // Kameran följer spelaren
            c.target = new Vector2((player.playerRect.x + playerCameraOffset), (GameManager.screenHeight / 2)); // Kamerans position sätts relativt till spelarens position
        }

        // Om avståndet till slutet av nivån är mindre än eller lika med det tillåtna avståndet
        else if (levelWidth - player.playerRect.x <= playerMaxDistanceToLvlEnd)
        {
            isTrackingPlayer = false; // Kameran följer inte sprlaren
            c.target = new Vector2((levelWidth - (GameManager.screenWidth / 2)), GameManager.screenHeight / 2); // Kamerans position sätts till slutet av nivån
        }

        // Om ingen av ovanstående villkor uppfylls (spelaren är i början av banan)
        else
        {
            isTrackingPlayer = false; // Kameran följer inte spelaren
            c.target = new Vector2((GameManager.screenWidth / 2), (GameManager.screenHeight / 2)); // Kamerans position är i mitten av skärmen vid banans början
        }
    }
}
