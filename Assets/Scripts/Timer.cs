using UnityEngine;

public class Timer
{
    public float Seconds;
    public bool Done = true;

    public void Start(float seconds)
    {
        Done = false;
        Util.DelayedExecutionManager.ScheduleAction(() => Done = true, seconds);
    }
}
