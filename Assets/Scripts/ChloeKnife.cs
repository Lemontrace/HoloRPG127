    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChloeKnife : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        for (int delay = 1; delay <= 3; ++delay)
            Util.DelayedExecutionManager.ScheduleAction(()=> collision.GetComponent<Generic>().Damage(10), delay);
    }
}