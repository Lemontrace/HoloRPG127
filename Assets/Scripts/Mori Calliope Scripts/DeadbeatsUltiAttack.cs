using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadbeatsUltiAttack : MonoBehaviour
{
    float timer = 0f;
    [SerializeField] GameObject deadbeatsPrefab;
    List<Collider2D> listOfEnemies = new List<Collider2D>();

    private void Update()
    {
        if (timer < 5f)
            timer += Time.deltaTime;
        else
        {
            InstantiateDeadbeatsMinions();
            Destroy(gameObject);
        }
       
    }

    // Wait for 5 seconds, then deadbeats minions will appear
    void InstantiateDeadbeatsMinions()
    {
        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            if (listOfEnemies[i] == null)
                continue;

            foreach (var item in listOfEnemies)
            {
                var g = Instantiate(deadbeatsPrefab, item.transform);
                // Do attack to enemies
                Destroy(g, 2f);
            }
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
