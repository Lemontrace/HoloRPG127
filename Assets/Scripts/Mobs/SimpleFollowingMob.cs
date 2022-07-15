using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowingMob : ActiveMob
{
    protected override void onAggroUpdate()
    {
        FollowPlayer();
    }

    protected override void onIdleUpdate()
    {
        //???
    }
}
