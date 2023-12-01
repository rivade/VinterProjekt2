using System;
using Raylib_cs;
using System.Numerics;

public class Camera
{
    Player player;

    const int playerMaxDistanceToLvlEnd = 754;

    public Camera(Player inPlayer)
    {
        player = inPlayer;
    }
    public Camera2D c = new();
    public void InitializeCamera()
    {
        c.zoom = 1;
        c.offset = new Vector2(GameManager.screenWidth / 2, GameManager.screenHeight / 2);
    }
    public void CameraBounds(int levelWidth) //Gör så att kameran bara följer efter spelaren efter den passerat en viss punkt vilket gör det snyggare
    {
        if (player.playerRect.x >= 265 && (levelWidth - player.playerRect.x >= playerMaxDistanceToLvlEnd))
            c.target = new Vector2((player.playerRect.x + 250), (GameManager.screenHeight / 2));

        else if (levelWidth - player.playerRect.x <= playerMaxDistanceToLvlEnd)
            c.target = new Vector2((levelWidth - (GameManager.screenWidth / 2)), GameManager.screenHeight / 2);

        else
            c.target = new Vector2((GameManager.screenWidth / 2), (GameManager.screenHeight / 2));
    }
}