using System.Runtime.InteropServices;
using Raylib_cs;

public class Level
{
    public const int blockWidth = GameState.screenWidth / 12;
    public const int blockHeight = GameState.screenHeight / 9;

    public int[,] layout;

    public void DrawLevel()
    {
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++)
            {
                switch (layout[y, x])
                {
                    case 1:
                        {
                            Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.BLACK);
                        }
                        break;
                }
            }
        }

    }
}

public class LevelOne : Level
{
    public LevelOne()
    {
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1}
        };
    }
}

public class LevelTwo : Level
{
    
}