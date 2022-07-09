using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileProjectile : MonoBehaviour
{
    public float Damage;
    public bool DestroyOnHit = true;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Generic>().Damage(Damage);
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
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Generic>().Damage(Damage);
            if (DestroyOnHit) Destroy(gameObject);
            return;
        }
    }
}
