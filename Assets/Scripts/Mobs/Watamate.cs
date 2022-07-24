using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watamate : PassiveMob
{
    float ChargeDistance = Util.TileSize * 3;
    float ChargeSpeed = Util.TileSize * 6;
    bool Charging = false;
    Vector3 ChargeDirection;


    protected override void onAggroUpdate()
    {
        if (AttackTimer.Done) //initiate charge
        {
            StartCharging();
            AttackTimer.Start(Random.Range(8, 10));
        }
        else if (Charging) //charging
        {
            transform.Translate(ChargeSpeed * Time.deltaTime * ChargeDirection);
        }
        else FollowPlayer();
    }

    void StartCharging()
    {
        Charging = true;
        Util.DelayedExecutionManager.ScheduleAction(() => Charging = false, ChargeDistance / ChargeSpeed);
        Vector3 playerPosition = Util.Player.transform.position;
        ChargeDirection = (playerPosition - transform.position).normalized;
    }
    protected override void onIdleUpdate()
    {
        //TODO
    }
}
