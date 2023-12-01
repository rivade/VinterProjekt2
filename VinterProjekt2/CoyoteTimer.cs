using System;
using System.Threading;

public class CoyoteTimer
{
    private Player player;
    private bool isTimerActive;
    private object lockObject;

    public CoyoteTimer(Player inPlayer)
    {
        player = inPlayer;
        isTimerActive = false;
        lockObject = new();
    }

    public void StartTimer()
    {
        lock (lockObject) //Låser koden till endast det aktiva objektet
        {
            if (!isTimerActive) //Startar bara ny tråd om en timer inte redan är aktiv
            {
                isTimerActive = true;
                Thread timerThread = new Thread(TimerFunction);
                timerThread.Start();
            }
        }
    }

    private void TimerFunction()
    {
        player.canJump = true;
        Thread.Sleep(150);
        player.canJump = false;

        lock (lockObject)
        {
            isTimerActive = false; //När tråden körts klart blir variablen false
        }
    }
}
