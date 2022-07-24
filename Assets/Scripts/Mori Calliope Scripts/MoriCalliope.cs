using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCalliope : PlayableCharacter
{
    [SerializeReference] GameObject ScythePrefab;
    float BasicAttackDamage = 40f;
    float ScytheAttackArc = 60f;

    [SerializeReference] GameObject LifeDrainParticlePrefab;
    float LifeDrainRange = Util.TileSize * 5;
    float LifeDrainDuration = 3f;
    float LifeDrainDamage = 150f;
    float LifeDrainFrequency = 1;

    [SerializeReference] GameObject DeadBeatUltsPrefab;
    float DeadBeatDamage = 140f;
    float UltRange = Util.TileSize * 10;
    float DeadBeatDuration = 1f;

    private void Awake()
    {
        Skill1 = ScytheAttack;
        Skill1Cooldown = 0.9f;
        Skill2 = LifeDrain;
        Skill2Cooldown = 12;
        Skill3 = DeadBeat;
        Skill3Cooldown = 30;

        MaxHp = 1000;
        BaseDefence = 10;
        BaseMovementSpeed = Util.SpeedUnitConversion(350);
    }

    void ScytheAttack()
    {
        GameObject scythe = Instantiate(ScythePrefab, transform);
        scythe.transform.rotation = Quaternion.FromToRotation(Vector3.right, GetComponent<Generic>().Facing) * scythe.transform.rotation;
        scythe.GetComponent<ScytheAttack>().AttackArc = ScytheAttackArc;
        scythe.GetComponent<FriendlyProjectile>().Damage = BasicAttackDamage;
        scythe.GetComponent<FriendlyProjectile>().DestroyOnHit = false;
    }

    void LifeDrain()
    {
        var targetDistance = Util.TileSize * 0.33f;

        var count = LifeDrainDuration * LifeDrainFrequency;
        for (int i = 1; i <= count; i++)
        {
            Util.DelayedExecutionManager.ScheduleAction(() =>
            {
                var target = GetClosestEnemy(LifeDrainRange);
                if (target != null)
                {
                    target.GetComponent<Generic>().Damage(LifeDrainDamage);
                    GetComponent<Generic>().Heal(LifeDrainDamage);
                    Vector3 displacement = target.transform.position - transform.position;
                    var distance = displacement.magnitude;
                    var segmentCount = Mathf.FloorToInt(distance / targetDistance) - 1;
                    var distanceBetweenParticles = distance / segmentCount;

                    for (int i = 1; i <= segmentCount - 1; i++)
                    {
                        var particle = Instantiate(LifeDrainParticlePrefab,
                            transform.position + displacement.normalized * distanceBetweenParticles * i, Quaternion.identity);
                        Util.DelayedExecutionManager.ScheduleAction(() => Destroy(particle), 0.3f);
                    }
                }
                else Skill2Timer = 0;
            }, i / LifeDrainFrequency);
        }
    }
    
    void DeadBeat()
    {
        var closest = GetClosestEnemy(UltRange);

        if (closest != null)
        {
            GameObject deadBeat = Instantiate(DeadBeatUltsPrefab, closest.transform.position, Quaternion.identity);
            deadBeat.GetComponent<FriendlyProjectile>().Damage = DeadBeatDamage;
            deadBeat.GetComponent<FriendlyProjectile>().DestroyOnHit = false;
            deadBeat.GetComponent<FriendlyProjectile>().OnHit +=
                (target) => target.GetComponent<EffectHandler>().AddEffect(new Effect.Stun(1));
            Util.DelayedExecutionManager.ScheduleAction(() => Destroy(deadBeat), DeadBeatDuration);
        }
        else Skill3Timer = 0;
    }
}