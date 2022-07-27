using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : ActiveMob
{
    [SerializeReference] GameObject ProjectilePrefab;
    float ProjectileSpeed = Util.TileSize * 5;
    float AttackRange; //set at Start()
    float AttackCooldown = 1.5f;


    protected override void Start()
    {
        AttackRange = AggroRadious;
        base.Start();
    }

    protected override void onAggroUpdate()
    {
        var dp = Util.Player.transform.position - transform.position;
        float distance = dp.magnitude;
        Vector3 direction = dp.normalized;
        var generic = GetComponent<Generic>();
        generic.Facing = direction;

        if (distance < AggroRadious * 0.6f) direction *= -1;
        else if (distance > AggroRadious * 0.6f && distance < AggroRadious) direction = Vector3.zero;
        var speed = generic.MovementSpeed;
        transform.Translate(speed * Time.deltaTime * direction);


        if (AttackTimer.Done)
        {
            AttackTimer.Start(AttackCooldown);
            var slime = Util.SpawnLinearProjectile(gameObject, ProjectilePrefab, ProjectileSpeed, AttackRange).GetComponent<HostileProjectile>();
            slime.Damage = Stats.Attack;
        }
    }

    protected override void onIdleUpdate()
    {
        //TODO
    }
}
