using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponent<Generic>().OnHit += (damage) =>
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Util.DelayedExecutionManager.ScheduleAction(() => { GetComponent<SpriteRenderer>().color = Color.white; },0.5f);
        };
    }
}
