using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PassiveMob : Mob
{

    protected override void Start()
    {
        GetComponent<Generic>().OnHit += (_) => Aggroed = true;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceToPlayer > UnAggroRadious) Aggroed = false;

        base.Update();
    }

}
