using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowingMob : ActiveMob
{
    protected override void onAggroUpdate()
    {
        Vector3 diretion = (Util.Player.transform.position - transform.position).normalized;
        var speed = GetComponent<Generic>().MovementSpeed;
        transform.Translate(speed * Time.deltaTime * diretion);
    }

    protected override void onIdleUpdate()
    {
        //???
    }
}
