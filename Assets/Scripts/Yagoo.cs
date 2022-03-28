using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yagoo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Generic>().HitPoint <= 0) Die();
    }

    void Die()
    {
        //idk some epic death I guess;
        Destroy(gameObject);
    }


}
