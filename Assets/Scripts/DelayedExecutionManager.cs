using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedExecutionManager : MonoBehaviour
{
    public delegate void ActionDelegate();
    private int LastId = 0;
    internal class ActionInfo
    {
        internal int Id;
        internal readonly ActionDelegate Action;
        internal float Delay;

        internal ActionInfo(int id, ActionDelegate action, float delay)
        {
            Id = id;
            Action = action;
            Delay = delay;
        }
    }

    private List<ActionInfo> AllActions;

    void Start()
    {
        AllActions = new List<ActionInfo>();
    }

    void Update()
    {
        for (int i = AllActions.Count - 1; i >= 0; i--)
        {
            ActionInfo action = AllActions[i];
            action.Delay -= Time.deltaTime;
            if (action.Delay <= 0)
            {
                action.Action.Invoke();
                AllActions.RemoveAt(i);
            }
        }
    }

    public int ScheduleAction(ActionDelegate action, float delay)
    {
        AllActions.Add(new ActionInfo(++LastId, action, delay));
        return LastId;
    }

    public void UnscheduleAction(int id)
    {
        AllActions.RemoveAll((action) => action.Id == id);
    }
}