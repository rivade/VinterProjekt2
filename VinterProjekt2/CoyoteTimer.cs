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

    public void StartTimer() //Den metod som startar coyotetimern
    {
        lock (lockObject) //Låser koden till endast det ett aktivt objekt
        {
            if (!isTimerActive) //Startar bara ny tråd om en timer inte redan är aktiv
            {
                isTimerActive = true; //Timern är nu aktiv
                Thread timerThread = new Thread(TimerFunction); //Skapar en ny tråd med timerfunktionen
                timerThread.Start(); //Startar tråden
            }
        }
    }

    private void TimerFunction()
    {
        player.canJump = true; //Spelaren kan hoppa i luften
        Thread.Sleep(150);  //Väntar 150 millisekunder
        player.canJump = false; //Spelaren kan inte längre hoppa

        lock (lockObject)
        {
            isTimerActive = false; //När tråden körts klart blir variablen false
        }
    }
}
