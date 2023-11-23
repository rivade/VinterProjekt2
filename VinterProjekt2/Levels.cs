using System.Runtime.InteropServices;
using Raylib_cs;

public class Level
{
    public const int blockWidth = Game.screenWidth / 12;
    public const int blockHeight = Game.screenHeight / 9;

    public int[,] layout;
    public List<Rectangle> walls = new();

    public virtual void DrawLevel()
    {
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++)
            {
                switch (layout[y, x])
                {
                    case 1:
                        Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.BLACK);
                        break;

                    case 2:
                        Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.BLUE);
                        break;
                    case 3:
                        Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.RED);
                        break;

                }
            }
        }

    }

    public void GenerateWalls()
    {
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout.GetLength(1); x++)
            {
                if (layout[y, x] == 2)
                walls.Add(new Rectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight));
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
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0},
            {1, 1, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1}
        };

        GenerateWalls();
    }
}

public class LevelTwo : Level
{

}
