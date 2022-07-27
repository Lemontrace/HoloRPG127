using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ollie : PlayableCharacter
{
    float basicAttackReach = Util.TileSize * 0.5f;
    float basicAttackDamage = 80f;

    [SerializeField] GameObject summonPrefab;
    int summonAmount = 7;
    float summonDuration = 4f;
    float summonRadius = Util.TileSize;

    float ultimateDamage = 180f;
    float ultimateReach = Util.TileSize * 0.5f;
    float maxHPIncrement = 20f;

    override protected void Start()
    {
        Skill1 = BasicAttack;
        Skill1Cooldown = 0.8f;
        Skill2 = SummonZombie;
        Skill2Cooldown = 25;
        Skill3 = SwordStrike;
        Skill3Cooldown = 80;
    }

    void BasicAttack()
    {
        Vector3 facing = GetComponent<Generic>().Facing;
        Vector3 point = transform.position + facing * (basicAttackReach / 2 + 0.5f * Util.TileSize);
        float basicAttackDamage = this.basicAttackDamage + DamageBuff;

        var colliders = Physics2D.OverlapCircleAll(
            new Vector2(point.x, point.y),
            basicAttackReach
        );

        Collider2D nearestEnemyCollider = Util.GetNearestEnemyFromPoint(colliders, point);
        if (nearestEnemyCollider)
        {
            nearestEnemyCollider.GetComponent<Generic>().Damage(basicAttackDamage);
        }
    }

    void SummonZombie()
    {
        for (int i = 0; i < summonAmount; ++i)
        {
            Vector3 summonPosition = transform.position + summonRadius * Random.onUnitSphere;
            var summon = Instantiate(summonPrefab, summonPosition, Quaternion.identity);
            Util.DelayedExecutionManager.ScheduleAction(() => { Destroy(summon); }, summonDuration);
        }
    }

    void SwordStrike()
    {
        Vector3 facing = GetComponent<Generic>().Facing;
        Vector3 point = transform.position + facing * (ultimateReach / 2 + 0.5f * Util.TileSize);
        float ultimateDamage = this.ultimateDamage + DamageBuff;

        var colliders = Physics2D.OverlapCircleAll(
            new Vector2(point.x, point.y),
            ultimateReach
        );

        Collider2D nearestEnemyCollider = Util.GetNearestEnemyFromPoint(colliders, point);
        Generic nearestEnemyGeneric = nearestEnemyCollider.GetComponent<Generic>();

        if (nearestEnemyGeneric.HitPoint > 0f && nearestEnemyGeneric.HitPoint - ultimateDamage <= 0f)
        {
            nearestEnemyGeneric.OnZeroHP += () => { GetComponent<Generic>().MaxHitPoint += maxHPIncrement; };
        }

        if (nearestEnemyCollider)
        {
            nearestEnemyGeneric.Damage(ultimateDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 facing = GetComponent<Generic>().Facing;
        Vector3 point = transform.position + facing * (basicAttackReach / 2 + 0.5f * Util.TileSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point, basicAttackReach);
    }
}
