using UnityEngine;
abstract public class Mob : MonoBehaviour
{
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