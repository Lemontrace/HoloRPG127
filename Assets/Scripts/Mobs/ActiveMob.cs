using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActiveMob : Mob
{
    protected float AggroRadious = Util.TileSize * 5;
    // Update is called once per frame
    protected override void Update()
    {
        var player = Util.Player;
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceToPlayer < AggroRadious) Aggroed = true;
        else if (distanceToPlayer > UnAggroRadious) Aggroed = false;

        base.Update();
    }

}
