using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static GameObject Player { get { return GameObject.FindGameObjectWithTag("Player"); } }

    public static DelayedExecutionManager DelayedExecutionManager
        => GameObject.Find("Managers").GetComponent<DelayedExecutionManager>();

    public const float TileSize = 1.2058f;

    //spawns single linear projectile
    public static GameObject SpawnLinearProjectile(GameObject thrower, GameObject projectilePrefab,
        float damage,float speed, float range, bool piercing)
    {
        var facing = thrower.GetComponent<Generic>().Facing;

        var position = thrower.transform.position + 0.5f * Util.TileSize * facing;
        var rotation = Quaternion.FromToRotation(Vector3.right, facing);

        var projectile = GameObject.Instantiate(projectilePrefab, position, rotation).GetComponent<LinearBullet>();
        projectile.Direction = facing;
        projectile.Speed = speed;

        projectile.Damage = damage;
        projectile.Range = range;
        projectile.Piercing = piercing;
        return projectile.gameObject;
    }
}