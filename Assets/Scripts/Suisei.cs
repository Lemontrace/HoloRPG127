using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suisei : PlayableCharacter
{
    float basicAttackReach = Util.TileSize * 0.5f;
    float basicAttackWidth = Util.TileSize * 1.5f;
    float basicAttackDamage = 80f;

    [SerializeReference] private GameObject starPrefab;
    float starDamage = 110f;
    float starDamageRadius = Util.TileSize;
    float starTravelDistance = Util.TileSize * 4;
    float starTravelDuration = 4f;

    [SerializeReference] private GameObject meteoritePrefab;
    float metoeriteDamage = 110f;
    float enemyDetectionRadius = Util.TileSize * 4;
    float metoeriteDamageRadius = Util.TileSize * 4;
    int metoeriteTickDamageDuration = 3;

    override protected void Start()
    {
        MaxHp = 850;
        BaseDefence = 8;
        BaseMovementSpeed = Util.SpeedUnitConversion(355);

        Skill1 = BasicAttack;
        Skill1Cooldown = 1;
        Skill2 = ThrowingStar;
        Skill2Cooldown = 15;
        Skill3 = Meteorite;
        Skill3Cooldown = 80;
    }

    void BasicAttack()
    {
        Vector3 facing = GetComponent<Generic>().Facing;
        Vector3 point = transform.position + facing * (basicAttackReach / 2 + 0.5f * Util.TileSize);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, facing);
        float basicAttackDamage = this.basicAttackDamage + DamageBuff;
        var colliders = Physics2D.OverlapBoxAll(
            new Vector2(point.x, point.y),
            new Vector2(basicAttackWidth, basicAttackReach),
            rotation.eulerAngles.z + 90f
        );
        foreach (var collider in colliders)
            if (collider.gameObject.CompareTag("Enemy")) collider.GetComponent<Generic>().Damage(basicAttackDamage);
    }

    void ThrowingStar()
    {
        float starTravelSpeed = starTravelDistance / starTravelDuration;
        var star = Util.SpawnLinearProjectile(gameObject, starPrefab, starTravelSpeed, starTravelDistance).GetComponent<FriendlyExplosiveProjectile>();
        star.ExplodeDamage = starDamage;
        star.DestroyOnHit = true;
        star.ExplodeRadius = starDamageRadius;
    }

    void Meteorite()
    {
        var center = transform.position;
        center.y -= 0.5f * Util.TileSize;
        var colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), enemyDetectionRadius);

        Collider2D nearestEnemyCollider = Util.GetNearestEnemyFromPoint(colliders, transform.position);
        if (!nearestEnemyCollider) return;

        var meteorite = Instantiate(meteoritePrefab, nearestEnemyCollider.transform.position, Quaternion.identity);
        float metoeriteDamage = this.metoeriteDamage + DamageBuff;

        for (int tick = 0; tick < metoeriteTickDamageDuration; ++tick)
        {
            Util.DelayedExecutionManager.ScheduleAction(() =>
            {
                var colliders = Physics2D.OverlapCircleAll(meteorite.transform.position, metoeriteDamageRadius);

                foreach (var collider in colliders)
                {
                    if (!collider.gameObject.CompareTag("Enemy")) continue;
                    collider.GetComponent<Generic>().Damage(metoeriteDamage);
                }
            }, tick + 1);
        }

        Destroy(meteorite, metoeriteTickDamageDuration + 1);
    }
}
