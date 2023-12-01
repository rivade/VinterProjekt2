using System;
using Raylib_cs;
using System.Numerics;

public class Camera
{
    Player player;
    public Camera(Player inPlayer)
    {
        player = inPlayer;
    }
    public Camera2D c = new();
    public void InitializeCamera()
    {
        c.zoom = 1;
        c.rotation = 0;
        c.offset = new Vector2(GameManager.screenWidth / 2, GameManager.screenHeight / 2);
    }
    public void CameraBounds() //Gör så att kameran bara följer efter spelaren efter den passerat en viss punkt vilket gör det snyggare
    {
        if (p.rect.x >= 265)
        {
            c.target = new Vector2((p.rect.x + 250), (GameManager.screenHeight / 2));
        }
        else
        {
            c.target = new Vector2((GameManager.screenWidth / 2), (GameManager.screenHeight / 2));
        }
    }
}