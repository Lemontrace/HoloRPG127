using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActiveMob : Mob
{
    protected float AggroRadious = 5f;
    // Update is called once per frame
    protected override void Update()
    {
        Debug.Log("myaaaa");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distanceToPlayer = Vector2.Distance((Vector2)player.transform.position, (Vector2)transform.position);

        if (distanceToPlayer < AggroRadious) Aggroed = true;
        else if (distanceToPlayer > UnAggroRadious) Aggroed = false;

        base.Update();
    }

}
