using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTarantula : ActiveMob
{
    [SerializeReference] GameObject WebPrefab;
    float WebSpeed;
    float WebRange;

    float WebShootCooldown = 8;
    Timer WebShootTimer = new Timer();

    protected override void onAggroUpdate()
    {
        FollowPlayer();

        if (WebShootTimer.Done)
        {
            Util.SpawnLinearProjectile(gameObject, WebPrefab, WebSpeed, WebRange).GetComponent<HostileProjectile>().OnHit += (target) =>
            {
                target.GetComponent<EffectHandler>().AddEffect(new Effect.SpeedMuliplier(3, 0.7f));
            };
            WebShootTimer.Start(WebShootCooldown);
        }
    }

    protected override void onIdleUpdate()
    {
        //TODO
    }
}