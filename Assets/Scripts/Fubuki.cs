using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fubuki : MonoBehaviour
{

    float BasicAttackReach = Util.TileSize * 1;
    float BasicAttackWidth = Util.TileSize * 1;
    float BasicAttackDamage = 30f;


    bool Buffed = false;
    float BuffDuration = 4f;
    float BuffSpeedMultiplier = 1.15f;
    float BuffDamageMultiplier = 1.1f;

    float CurrentShield = 0;

    [SerializeReference] GameObject ShieldPrefab;
    float UltShieldAmount = 80f;
    float UltShieldDuration = 4f;
    [SerializeReference] GameObject SwordAuraPrefab;
    float SwordAuraDamage = 170;
    float SwordAuraReach = Util.TileSize * 10;
    float SwordAuraSpeed = Util.TileSize * 5;
    float SwordAuraDelay = 1f;


    float Skill1CoolDown = 1.2f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 7f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 60f;
    private float Skill3Timer = 0f;

    void Start()
    {
        var generic = GetComponent<Generic>();
        generic.OnHit += (damage) =>
        {
            if (CurrentShield <= 0) return;

            if (CurrentShield >= damage)
            {
                generic.HitPoint += damage;
                CurrentShield -= damage;
            }
            else
            {
                generic.HitPoint += CurrentShield;
                CurrentShield = 0;
            }
        };
    }

    void Update()
    {

        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            BasicAttack();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            Buff();
        }

        //invoke skill 3
        if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            Ult();
        }


    }
    void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    void BasicAttack()
    {
        void Attack()
        {
            var facing = GetComponent<Generic>().Facing;

            var position = transform.position + facing * ((BasicAttackReach / 2) + Util.TileSize * 0.5f);
            var rotation = Quaternion.FromToRotation(Vector3.right, facing);

            var colliders = Physics2D.OverlapBoxAll(position, new Vector2(BasicAttackWidth, BasicAttackReach), rotation.eulerAngles.z + 90);
            foreach (var collider in colliders)
            {
                if (!collider.gameObject.CompareTag("Enemy")) continue;
                collider.GetComponent<Generic>().Damage(BasicAttackDamage);
            }
        }
        Attack();
        Util.DelayedExecutionManager.ScheduleAction(Attack, 0.6f);
    }

    void Buff()
    {
        Buffed = true;
        Util.DelayedExecutionManager.ScheduleAction(() => Buffed = false, BuffDuration);
        GetComponent<EffectHandler>().AddEffect(new Effect.SpeedMuliplier(BuffDuration, BuffSpeedMultiplier));
    }

    void Ult()
    {
        CurrentShield = UltShieldAmount;
        var shieldSprite = Instantiate(ShieldPrefab);
        shieldSprite.GetComponent<Follow>().ToFollow = gameObject;

        Util.DelayedExecutionManager.ScheduleAction(() => {
            CurrentShield = 0;
            Destroy(shieldSprite);
        }, UltShieldDuration);


        void Shoot()
        {
            var facing = GetComponent<Generic>().Facing;

            var position = transform.position + facing * Util.TileSize * 0.5f;
            var rotation = Quaternion.FromToRotation(Vector3.right, facing);
            var swordAura = Instantiate(SwordAuraPrefab, position, rotation).GetComponent<LinearBullet>();
            swordAura.Direction = facing;
            swordAura.Speed = SwordAuraSpeed;

            var damage = SwordAuraDamage;
            if (Buffed) damage *= BuffDamageMultiplier;
            swordAura.Damage = damage;
            swordAura.Range = SwordAuraReach;
            swordAura.Piercing = true;
        }

        Util.DelayedExecutionManager.ScheduleAction(Shoot, SwordAuraDelay);
        Util.DelayedExecutionManager.ScheduleAction(Shoot, SwordAuraDelay * 2);
    }
}