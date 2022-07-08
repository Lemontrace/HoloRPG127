using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fubuki : PlayableCharacter
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

    override protected void Start()
    {
        MaxHp = 1100;
        BaseDefence = 17;
        BaseMovementSpeed = Util.SpeedUnitConversion(355);

        Skill1 = BasicAttack;
        Skill1CoolDown = 1.2f;
        Skill2 = Buff;
        Skill2CoolDown = 7f;
        Skill3 = Ult;
        Skill3CoolDown = 60f;


        //note : could use effect handler to implement shield
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
                var damage = BasicAttackDamage + DamageBuff / 2;
                if (Buffed) damage *= BuffDamageMultiplier;
                collider.GetComponent<Generic>().Damage(damage);
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

            var damage = SwordAuraDamage + DamageBuff;
            if (Buffed) damage *= BuffDamageMultiplier;
            swordAura.Damage = damage;
            swordAura.Range = SwordAuraReach;
            swordAura.Piercing = true;
        }

        Util.DelayedExecutionManager.ScheduleAction(Shoot, SwordAuraDelay);
        Util.DelayedExecutionManager.ScheduleAction(Shoot, SwordAuraDelay * 2);
    }
}