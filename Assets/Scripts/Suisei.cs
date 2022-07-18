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

    float metoeriteDamage = 110f;
    float enemyDetectionRadius = Util.TileSize * 4;
    float metoeriteDamageRadius = Util.TileSize * 4;
    int metoeriteTickDamageDuration = 3;

    override protected void Start()
    {
        Skill1 = NormalHit;
        Skill1CoolDown = 1;
        Skill2 = SkillOne;
        Skill2CoolDown = 15;
        Skill3 = Ultimate;
        Skill3CoolDown = 80;
    }

    void NormalHit()
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

    void SkillOne()
    {
        float starTravelSpeed = starTravelDistance / starTravelDuration;
        var star = Util.SpawnLinearProjectile(gameObject, starPrefab, starTravelSpeed, starTravelDistance).GetComponent<FriendlyObject>();
        star.Damage = starDamage;
        star.DestroyOnHit = true;
        star.Explosive = true;
        star.ExplodeRadius = starDamageRadius;
    }

    void Ultimate()
    {
        var center = transform.position;
        center.y -= 0.5f * Util.TileSize;
        var colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), enemyDetectionRadius);

        Collider2D nearestEnemyCollider = getNearestEnemy(colliders);
        if (!nearestEnemyCollider) return;

        center = getNearestEnemy(colliders).transform.position;
        colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), metoeriteDamageRadius);

        float metoeriteDamage = this.metoeriteDamage + DamageBuff;

        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue;
            for (int tick = 0; tick < metoeriteTickDamageDuration; ++tick)
            {
                Util.DelayedExecutionManager.ScheduleAction(() => { collider.GetComponent<Generic>().Damage(metoeriteDamage); }, tick + 1);
            }
        }
    }

    private Collider2D getNearestEnemy(Collider2D[] colliders)
    {
        if (colliders.Length == 0) return null;

        int nearestIndex = 0;
        float nearestDistance = float.PositiveInfinity;
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (!colliders[i].gameObject.CompareTag("Enemy")) continue;

            float distance = Mathf.Abs(Vector3.Distance(transform.position, colliders[i].transform.position));
            if (distance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = distance;
            }
        }

        return colliders[nearestIndex];
    }
}
