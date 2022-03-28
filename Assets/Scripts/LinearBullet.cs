using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : MonoBehaviour
{

    public float Speed;
    public Vector3 Direction;

    void FixedUpdate()
    {
        transform.position += Time.deltaTime * Speed * Direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Generic>().HitPoint -= 3;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Generic>().HitPoint -= 3;
            Destroy(gameObject);
        }
    }
}
