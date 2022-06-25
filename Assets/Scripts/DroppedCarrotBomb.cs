using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCarrotBomb : MonoBehaviour
{
    public float Speed;
    public float Height;
    private float distance = 0;
    public float ExplosionDamage;
    public float ExplosionRadious;

    void FixedUpdate()
    {
        distance += Time.deltaTime * Speed;
        if (distance <= Height) transform.position += Time.deltaTime * Speed * Vector3.down;
        else Explode();
    }

    void Explode()
    {
        var colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), ExplosionRadious);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy")||collider.gameObject.CompareTag("player"))
            {
                collider.gameObject.GetComponent<Generic>().Damage(ExplosionDamage);
            }
        }
        Destroy(gameObject);
    }
}
