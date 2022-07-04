using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    float timer = 0f;
    float dir = 1f;
    float y = 0f;
    float z = 0f;
    float speed = 0f;

    public List<Collider2D> listOfEnemies = new List<Collider2D>();
    public List<Collider2D> attackedEnemies = new List<Collider2D>();

    private void Start()
    {
        dir = CharacterMovement.instance.directionIndex;
    }

    // Update is called once per frame
    void Update()
    {


        if (timer < 0.3f)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);

            transform.Rotate(new Vector3(0, 0,  -1 * 500 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            listOfEnemies.Add(collision);

            for (int i = 0; i < listOfEnemies.Count; i++)
            {
                if (listOfEnemies[i] == null)
                    continue;

                if (attackedEnemies.Contains(listOfEnemies[i]))
                    continue;

                else
                {
                    attackedEnemies.Add(collision);
                    attackedEnemies[i].gameObject.name = "Enemy Hit " + Random.Range(0,20);
                    //foreach (var enemy in collision.GetComponents<IEnemy>())
                    //    enemy.EnemyAttacked();

                }
            }
        }
    }
}
