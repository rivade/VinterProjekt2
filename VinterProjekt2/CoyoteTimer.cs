using System.Threading;
using System.Diagnostics;

public class CoyoteTimer
{
    private Player player;
    public Thread timerThread;

    public CoyoteTimer(Player inPlayer)
    {
        player = inPlayer;
    }

    public void StartTimer()
    {
        if (timerThread == null || !timerThread.IsAlive)
        {
            timerThread = new Thread(TimerFunction);
            timerThread.Start();
        }
    }

    public void TimerFunction()
    {
        player.canJump = true;
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (stopwatch.ElapsedMilliseconds >= 1000)
        {
            player.canJump = false;
            stopwatch.Stop();
        }
    }
}
