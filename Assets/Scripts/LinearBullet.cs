using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : MonoBehaviour
{

    public float Speed;
    public Vector3 Direction;
    public float Range;
    private float distance = 0;
    public float Damage;
    public bool Piercing = false;

    void FixedUpdate()
    {
        distance += Time.deltaTime * Speed;
        if (distance <= Range) transform.position += Time.deltaTime * Speed * Direction.normalized;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Generic>().Damage(Damage);
            if (!Piercing) Destroy(gameObject);
            return;
        }
    }

    
}
