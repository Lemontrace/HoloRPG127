using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chloe : MonoBehaviour
{
    float BasicAttackReach = Util.TileSize * 1.5f;
    float BasicAttackWidth = Util.TileSize * 0.5f;
    float BasicAttackDamage = 30f;

    [SerializeReference] private GameObject KnifePrefab;
    float KnifeThrowSpeed = Util.TileSize * 18;
    float KnifeThrowRange = Util.TileSize * 6;
    float KnifeThrowDamage = 50f;

    float AssasinationRadious = Util.TileSize * 6;
    float InvincibilityDuration = 1;
    float AssasinationDamage = 450;
    float AssasinationBleedDuration = 3;


    float Skill1CoolDown = 1;
    float Skill1Timer = 0;

    float Skill2CoolDown = 5;
    float Skill2Timer = 0;

    float Skill3CoolDown = 75;
    float Skill3Timer = 0;

    private void Update()
    {
        DecreaseTimers();

        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            BasicAttack();
        }

        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            KnifeThrow();
        }

        if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            Assasinate();
        }

    }

    private void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    void BasicAttack()
    {
        int numHit = 2;
        if (Random.value < 1f / 3) ++numHit;
        Attack(); //1st hit
        Util.DelayedExecutionManager.ScheduleAction(Attack, 1f / numHit); //2nd hit
        if (numHit == 3) Util.DelayedExecutionManager.ScheduleAction(Attack, 2f / numHit); //3rd hit

        void Attack()
        {
            Vector3 facing = GetComponent<Generic>().Facing;
            Vector3 point = transform.position + facing * (BasicAttackReach / 2 + 0.5f * Util.TileSize);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, facing);
            float basicAttackDamage = BasicAttackDamage;
            var colliders = Physics2D.OverlapBoxAll(new Vector2(point.x, point.y),
                new Vector2(BasicAttackWidth, BasicAttackReach), rotation.eulerAngles.z + 90f);
            foreach (var collider in colliders)
                if (collider.gameObject.CompareTag("Enemy")) collider.GetComponent<Generic>().Damage(basicAttackDamage);
        }
    }

    void KnifeThrow()
        => Util.SpawnLinearProjectile(gameObject, KnifePrefab, KnifeThrowDamage, KnifeThrowSpeed, KnifeThrowRange, false);

    void Assasinate()
    {
        Vector3 position = transform.position;
        var colliders = Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), AssasinationRadious);
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

            if (seekTarget == null) return;

            //teleport
            transform.position = seekTarget.transform.position + 0.5f * Util.TileSize * GetComponent<Generic>().Facing;
            //get invincivility
            GetComponent<EffectHandler>().AddEffect(new Effect.Invincibility(InvincibilityDuration));
            int bleedTimes = 3;
            for (int i = 0; i < bleedTimes; i++)
                Util.DelayedExecutionManager.ScheduleAction(() => seekTarget.GetComponent<Generic>().Damage(AssasinationDamage / bleedTimes),
                    i * AssasinationBleedDuration / bleedTimes);
        }
    }
}