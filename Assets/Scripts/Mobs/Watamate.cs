using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watamate : PassiveMob
{
    Timer ChargeTimer = new Timer();
    float ChargeDistance = Util.TileSize * 3;
    float ChargeSpeed = Util.TileSize * 6;
    bool Charging = false;
    Vector3 ChargeDirection;

    protected override void onAggroUpdate()
    {
        if (ChargeTimer.Done) //initiate charge
        {
            StartCharging();
            ChargeTimer.Start(Random.Range(8, 10));
        } else if (Charging) //charging
        {
            transform.Translate(ChargeSpeed * Time.deltaTime * ChargeDirection);
        } else //following player
        {
            Vector3 diretion = (Util.Player.transform.position - transform.position).normalized;
            var speed = GetComponent<Generic>().MovementSpeed;
            transform.Translate(speed * Time.deltaTime * diretion);
            
        }
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
        //???????
    }
}
