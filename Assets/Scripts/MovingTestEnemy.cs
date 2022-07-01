using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTestEnemy : MonoBehaviour
{
    public float distance;
    private float displacement;
    public bool right;
    // Start is called before the first frame update
    void Start()
    {
        if (!right) displacement = distance;
    }

    // Update is called once per frame
    void Update()
    {
        var dist = GetComponent<Generic>().MovementSpeed * Time.deltaTime;
        if (right)
        {
            transform.Translate(Vector3.right * dist);
            displacement += dist;
            if (displacement >= distance) right = !right;
        }
        else
        {
            transform.Translate(Vector3.left * dist);
            displacement -= dist;
            if (displacement<=0) right = !right;
        }


        GetComponent<Generic>().OnHit += (damage) =>
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Util.DelayedExecutionManager.ScheduleAction(() => { GetComponent<SpriteRenderer>().color = Color.white; }, 0.2f);
        };
    }
}
