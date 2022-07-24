using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chloe : PlayableCharacter
{
    float BasicAttackReach = Util.TileSize * 1.5f;
    float BasicAttackWidth = Util.TileSize * 0.5f;
    float BasicAttackDamage = 30f;

    [SerializeReference] private GameObject KnifePrefab;
    float KnifeThrowSpeed = Util.TileSize * 18;
    float KnifeThrowRange = Util.TileSize * 6;
    float KnifeThrowDamage = 50f;

    float AssassinationRadious = Util.TileSize * 6;
    float InvincibilityDuration = 1;
    float AssasinationDamage = 450;
    float AssasinationBleedDuration = 3;

    private void Awake()
    {
        MaxHp = 800;
        BaseDefence = 9;
        BaseMovementSpeed = Util.SpeedUnitConversion(380);

        Skill1 = BasicAttack;
        Skill1Cooldown = 1;
        Skill2 = KnifeThrow;
        Skill2Cooldown = 5;
        Skill3 = Assasinate;
        Skill3Cooldown = 75;
    }

    void BasicAttack()
    {
        int numHit = 2;
        if (Random.value < 1f / 3) ++numHit;
        SingleAttack(); //1st hit
        Util.DelayedExecutionManager.ScheduleAction(SingleAttack, 1f / numHit); //2nd hit
        if (numHit == 3) Util.DelayedExecutionManager.ScheduleAction(SingleAttack, 2f / numHit); //3rd hit

        void SingleAttack()
        {
            Vector3 facing = GetComponent<Generic>().Facing;
            Vector3 point = transform.position + facing * (BasicAttackReach / 2 + 0.5f * Util.TileSize);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, facing);
            float basicAttackDamage = BasicAttackDamage + DamageBuff / numHit;
            var colliders = Physics2D.OverlapBoxAll(new Vector2(point.x, point.y),
                new Vector2(BasicAttackWidth, BasicAttackReach), rotation.eulerAngles.z + 90f);
            foreach (var collider in colliders)
                if (collider.gameObject.CompareTag("Enemy")) collider.GetComponent<Generic>().Damage(basicAttackDamage);
        }
    }

    void KnifeThrow()
    {
        var knife = Util.SpawnLinearProjectile(gameObject, KnifePrefab, KnifeThrowSpeed, KnifeThrowRange).GetComponent<FriendlyProjectile>();
        knife.Damage = KnifeThrowDamage;
        knife.DestroyOnHit = true;
        knife.OnHit += (target) =>
        {
            for (int delay = 1; delay <= 3; ++delay)
                Util.DelayedExecutionManager.ScheduleAction(() => target.GetComponent<Generic>().Damage(10), delay);
        };
    }
    void Assasinate()
    {
        //get furthest target in AssassinationRadious
        Vector3 position = transform.position;
        var colliders = Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), AssassinationRadious);
        float maxDistance = float.NegativeInfinity;
        GameObject seekTarget = null;
        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue;

            float distance = Vector3.Distance(position, collider.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                seekTarget = collider.gameObject;
            }
        }

        if (seekTarget == null)
        {
            Skill3Timer = 0;
            return;
        }

        //teleport
        transform.position = seekTarget.transform.position + 0.5f * Util.TileSize * GetComponent<Generic>().Facing;
        //get invincivility
        GetComponent<EffectHandler>().AddEffect(new Effect.Invincibility(InvincibilityDuration));

        //apply bleeding effect
        int bleedTimes = 3;
        for (int i = 0; i < bleedTimes; i++)
            Util.DelayedExecutionManager.ScheduleAction(() => seekTarget.GetComponent<Generic>().Damage(AssasinationDamage / bleedTimes),
                i * AssasinationBleedDuration / bleedTimes);
    }
}