using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yagoo : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Generic>().OnZeroHP += Die;
    }
    void Die()
    {
        //idk some epic death I guess;
        Destroy(gameObject);
    }


}
