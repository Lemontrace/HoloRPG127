using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSnake : MonoBehaviour
{
    void Start()
    {
        GetComponent<CollideToAttack>().OnAttack += (target) =>
        {
            var manager = Util.DelayedExecutionManager;
            for (int i = 1; i <= 3; i++)
            {
                manager.ScheduleAction(() =>
                {
                    target?.GetComponent<Generic>().Damage(30);
                }, i);
            }
        };
    }
}
