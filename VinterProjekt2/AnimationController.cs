using Raylib_cs;

public class AnimationController
{
    public int frame = 1;
    private float elapsed = 0;
    private readonly float frameDuration;
    private readonly int totalFrames;

    public void FrameLogic()
    {
        elapsed += Raylib.GetFrameTime();

        if (elapsed >= frameDuration)
        {
            frame++;
            elapsed -= frameDuration;
        }

        frame %= totalFrames;
    }

    public AnimationController(float inFrameDuration, int inTotalFrames)
    {
        frameDuration = inFrameDuration;
        totalFrames = inTotalFrames;
    }
}