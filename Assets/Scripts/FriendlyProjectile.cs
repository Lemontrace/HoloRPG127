using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyProjectile : Projectile
{
    public float Damage;
    public bool DestroyOnHit = true;
    public bool Explosive = false;
    public float ExplodeRadius = 0f;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (DestroyOnHit) Destroy(gameObject);
            return;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (DestroyOnHit) Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (Explosive)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, ExplodeRadius);
            foreach (Collider2D collider in colliders)
            {
                collider.GetComponent<Generic>().Damage(Damage);
            }
        }
        else
        {
            GetComponent<Generic>().Damage(Damage);
        }
    }
}
