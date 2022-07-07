using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PassiveMob : Mob
{

    protected virtual void Start()
    {
        GetComponent<Generic>().OnHit += (_) =>
        {
            Aggroed = true;
        };
    }

    // Update is called once per frame
    protected override void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distanceToPlayer = Vector2.Distance((Vector2)player.transform.position, (Vector2)transform.position);

        if (distanceToPlayer > UnAggroRadious) Aggroed = false;

        base.Update();
    }

}
