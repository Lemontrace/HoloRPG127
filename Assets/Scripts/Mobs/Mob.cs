using System.Collections.Generic;
using UnityEngine;
public abstract partial class Mob : MonoBehaviour
{

    [SerializeReference] MobDropSO Drops;

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