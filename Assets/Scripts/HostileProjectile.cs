using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileProjectile : Projectile
{
    public float Damage;
    public bool DestroyOnHit = true;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Generic>().Damage(Damage);
            InvokeOnHit(collision.gameObject);
            if (DestroyOnHit) Destroy(gameObject);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Generic>().Damage(Damage);
            InvokeOnHit(collision.gameObject);
            if (DestroyOnHit) Destroy(gameObject);
        }
    }
}
