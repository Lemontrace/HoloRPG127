using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideToAttack : MonoBehaviour
{
    private Timer AttackTimer;
    float KnockbackDistance = Util.TileSize;
    Vector3 KnockbackDirection;
    float KnockbackTimer = 0;
    public delegate void OnAttackDelegate(GameObject target);
    public event OnAttackDelegate OnAttack;

    private void Start()
    {
        AttackTimer = new Timer(1);
    }

    private void Update()
    {
        if (KnockbackTimer > 0)
        {
            KnockbackTimer -= Time.deltaTime;
            Util.Player.transform.position += KnockbackDirection * KnockbackDistance / 0.2f * Time.deltaTime;   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != Util.Player) return;

        if (AttackTimer.Done)
        {
            AttackTimer.Start();
            float damage = GetComponent<Mob>().Stats.Attack;
            collision.gameObject.GetComponent<Generic>().Damage(damage);

            KnockbackDirection = collision.gameObject.transform.position - transform.position;
            KnockbackTimer = 0.2f;
            OnAttack?.Invoke(collision.gameObject);
        }
    }
}