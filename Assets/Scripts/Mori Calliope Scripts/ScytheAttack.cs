using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheAttack : MonoBehaviour
{

    float time = 0f;
    float AttackDuration = 0.3f;
    public float AttackArc;

    private void Start()
    {
        transform.Rotate(0, 0, AttackArc / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (time < AttackDuration)
            time += Time.deltaTime;
        else
            Destroy(gameObject);

        transform.Rotate(new Vector3(0, 0, -1 * AttackArc / AttackDuration * Time.deltaTime));
    }
}
