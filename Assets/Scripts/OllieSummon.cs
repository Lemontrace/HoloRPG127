using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OllieSummon : MonoBehaviour
{
    float interpolationFactor = 0.3f;

    float damage = 20f;
    float damageCooldown = 1f;
    float damageRange = Util.TileSize * 0.5f;

    float damageTimer;
    float detectionRadius = Util.TileSize * 4f;

    private Collider2D target = null;

    private void Start()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        target = Util.GetNearestEnemyFromPoint(colliders, transform.position);
    }

    private void Update()
    {
        DecreaseTimers();

        if (IsInRange(target.transform.position) && damageTimer <= 0)
        {
            damageTimer = damageCooldown;
            target.GetComponent<Generic>().Damage(damage);
        }
    }

    private void FixedUpdate()
    {
        Vector3 directionVector = target.transform.position - transform.position;
        Vector3 movementDirection = Vector3.Lerp(directionVector, GetComponent<Generic>().Facing, interpolationFactor);
        movementDirection.Normalize();
        Vector3 movement = GetComponent<Generic>().MovementSpeed * Time.deltaTime * movementDirection;
        GetComponent<Generic>().Facing = movementDirection;
        transform.position += movement;
    }

    private bool IsInRange(Vector3 enemyPosition)
    {
        if (Vector3.Distance(transform.position, enemyPosition) < damageRange)
            return true;

        return false;
    }

    private void DecreaseTimers()
    {
        damageTimer -= Time.deltaTime;
    }
}
