using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : MonoBehaviour
{

    public float Speed;
    public Vector3 Direction;
    public float Range;
    private float distance = 0;

    void FixedUpdate()
    {
        distance += Time.deltaTime * Speed;
        if (distance <= Range) transform.position += Time.deltaTime * Speed * Direction.normalized;
        else Destroy(gameObject);
    }
    
}
