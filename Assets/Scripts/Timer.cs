using UnityEngine;

public class Timer
{
    public float Seconds;
    public bool Done = true;

    public Timer() { }
    public Timer(float seconds) {this.Seconds = seconds; this.Start(seconds); }

    public void Start(float seconds)
    {
        Seconds = seconds;
        Done = false;
        Util.DelayedExecutionManager.ScheduleAction(() => Done = true, seconds);
    }

    public void Start()
    {
        Done = false;
        Util.DelayedExecutionManager.ScheduleAction(() => Done = true, Seconds);
    }
}
