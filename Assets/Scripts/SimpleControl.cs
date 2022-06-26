using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{


    void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 MovementDirection = new Vector3(h, v, 0).normalized;
        Vector3 movement = GetComponent<Generic>().MovementSpeed * Time.deltaTime * MovementDirection;
        if (MovementDirection.magnitude != 0f) GetComponent<Generic>().Facing = MovementDirection;
        transform.position += movement;
    }
}
