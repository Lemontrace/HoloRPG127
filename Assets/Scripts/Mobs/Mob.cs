using System.Collections.Generic;
using UnityEngine;
public abstract partial class Mob : MonoBehaviour
{

    [SerializeReference] protected MobDropSO Drops;
    [SerializeReference] protected MobStatSO Stats;

    protected Timer AttackTimer;

    protected bool Aggroed = false;
    protected float UnAggroRadious = Util.TileSize * 7;

    protected virtual void Start()
    {
        var generic = GetComponent<Generic>();
        generic.OnZeroHP += Die;

        AttackTimer = new Timer();
        generic.MaxHitPoint = generic.HitPoint = Stats.HitPoint;
        generic.BaseMovementSpeed = Util.SpeedUnitConversion(Stats.BaseMovementSpeed);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        //TODO drop items
    }

    protected virtual void Update()
    {
        if (Aggroed) onAggroUpdate();
        else onIdleUpdate();
    }

    protected void FollowPlayer()
    {
        Vector3 direction = (Util.Player.transform.position - transform.position).normalized;
        var generic = GetComponent<Generic>();
        generic.Facing = direction;
        var speed = generic.MovementSpeed;
        transform.Translate(speed * Time.deltaTime * direction);
    }

    abstract protected void onAggroUpdate();
    abstract protected void onIdleUpdate();
}