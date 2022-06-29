using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{
    
    float InterpolationFactor = 0.5f;
    float Dead = 0.1f;

    void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        
        //implement "Dead"
        if (Mathf.Abs(v) <Dead && Mathf.Abs(h) < Dead) return;
        //interpolate between input and current direction
        Vector3 InputVector = new Vector3(h, v, 0).normalized;
        Vector3 MovementDirection = Vector3.Slerp(InputVector, GetComponent<Generic>().Facing, InterpolationFactor);
        MovementDirection.Normalize();
        Vector3 movement = GetComponent<Generic>().MovementSpeed * Time.deltaTime * MovementDirection;
        GetComponent<Generic>().Facing = MovementDirection;
        transform.position += movement;
    }
}
