using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBeat : ActiveMob
{
    enum AggroState
    {
        Chasing, Charging, Jumping
    }

    AggroState State = AggroState.Chasing;
    int SkillCounter = 1;
    int SkillStack = 0;


    float ChargeDistance = Util.TileSize * 5;
    float ChargeSpeed = Util.TileSize * 6;
    Vector3 ChargeDirection;

    float JumpDuration = 2;
    float StunRadious = Util.TileSize * 2;
    float StunDuration = 1;

    protected override void onAggroUpdate()
    {
        var generic = GetComponent<Generic>();
        if (State == AggroState.Chasing)
        {   
            if (generic.HitPoint <= generic.MaxHitPoint * (100 - 15 * SkillCounter) / 100)
            {
                SkillCounter += 1;

                Util.DelayedExecutionManager.ScheduleAction(() => State = AggroState.Jumping, ChargeDistance / ChargeSpeed);
                Vector3 playerPosition = Util.Player.transform.position;
                ChargeDirection = (playerPosition - transform.position).normalized;
                State = AggroState.Charging;
            }
            else
            {
                FollowPlayer();
                return;
            }
        }

        if (State == AggroState.Charging)
        {
            transform.Translate(ChargeSpeed * Time.deltaTime * ChargeDirection);
        } else if (State == AggroState.Jumping)
        {
            //play jumping animation
            GetComponent<Collider2D>().enabled = false;
            Util.DelayedExecutionManager.ScheduleAction(() => {
                GetComponent<Collider2D>().enabled = true;
                if (Vector3.Distance(transform.position, Util.Player.transform.position) < StunRadious)
                    Util.Player.GetComponent<EffectHandler>().AddEffect(new Effect.Stun(StunDuration));
                Util.DelayedExecutionManager.ScheduleAction(() => State = AggroState.Chasing, StunDuration);
            },JumpDuration);
        }
    }

    protected override void onIdleUpdate()
    {
        //TODO
    }
}
