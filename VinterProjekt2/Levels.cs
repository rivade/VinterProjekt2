using System.Runtime.InteropServices;
using Raylib_cs;

public class Level
{
    public const int blockWidth = GameManager.screenWidth / 14;
    public const int blockHeight = GameManager.screenHeight / 9;

    public int[,] layout;
    public List<Rectangle> walls = new();
    private Rectangle goal;
    public Level nextLevel;


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
                    case 4:
                        Raylib.DrawRectangle(x * blockWidth, y * blockHeight, blockWidth, blockHeight, Color.GREEN);
                        break;

                }
            }
        }

    }

    public void GenerateWallsGoal()
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
    public LevelOne(Level nxtLvl)
    {
        nextLevel = nxtLvl;
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 3, 3, 3, 3, 3},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 3, 3, 3, 3, 1, 3, 3, 3, 3, 3}
        };
        GenerateWallsGoal();
    }
}

public class LevelTwo : Level
{
     public LevelTwo(Level nxtLvl)
    {
        nextLevel = nxtLvl;
        layout = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 3, 3, 3, 3, 3},
            {0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {2, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            {2, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4},
            {1, 1, 1, 1, 3, 3, 3, 3, 1, 3, 3, 3, 3, 3}
        };
        GenerateWallsGoal();
    }
}
