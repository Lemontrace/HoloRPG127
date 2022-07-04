using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrainAttack : MonoBehaviour
{

    float timer = 0f;
    List<Collider2D> listOfEnemies = new List<Collider2D>();

    private void Update()
    {
        if (timer < 5f)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);

        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            if (listOfEnemies[i] == null)
                continue;

            listOfEnemies[i].gameObject.name = "Life Drain! " + Random.Range(0,5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            listOfEnemies.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            listOfEnemies.Remove(collision);
    }

}
