using System.Collections.Generic;
using UnityEngine;
abstract public class Mob : MonoBehaviour
{
    protected struct DropInfo
    {
        int ItemId;
        int ItemCount;
        int DropChance;
    }

    protected List<DropInfo> Drops = new List<DropInfo>();

    protected bool Aggroed = false;
    protected float UnAggroRadious = 7f;
    protected virtual void Update()
    {
        if (Aggroed) onAggroUpdate();
        else onIdleUpdate();
    }

    abstract protected void onAggroUpdate();
    abstract protected void onIdleUpdate();
}