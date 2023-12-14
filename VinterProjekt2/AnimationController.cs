using Raylib_cs;

public class AnimationController //Hanterar ett objekts
{
    private readonly float frameDuration; //Hur länge varje del av spritesheeten ska visas
    private readonly int totalFrames; //Säger hur många frames spritesheeten har
    public int frame = 1;
    private float elapsed = 0;

    // Hanterar vilken frame av spritesheeten som ska visas
    public void FrameLogic() 
    {
        elapsed += Raylib.GetFrameTime(); //Lägger till tid till 'elapsed'-timern

        if (elapsed >= frameDuration) //När timern nått tiden till att framen ska bytas
        {
            frame++; //Går framen vidare till nästa
            elapsed -= frameDuration; //Timern återställs
        }

        frame %= totalFrames; //Förhindrar frame från att gå över totala antalet frames
    }

    // Constructor
    public AnimationController(float inFrameDuration, int inTotalFrames)
    {
        frameDuration = inFrameDuration;
        totalFrames = inTotalFrames;
    }
}