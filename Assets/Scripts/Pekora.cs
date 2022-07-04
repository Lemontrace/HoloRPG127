using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pekora : PlayableCharacter
{
    [SerializeReference] GameObject CarrotPrefab;
    float CarrotThrowSpeed = 15;
    float CarrotThrowRange = Util.TileSize * 5;
    float CarrotThrowDamage = 50;

    float CarrotHammerDamage = 50;
    float CarrotHammerStunDuration = 1;

    [SerializeReference] GameObject CarrotBombPrefab;
    float CarrotBombDropSpeed = 5;
    float CarrotBombDamage = 80;
    float CarrotBombExplosionRadious = 2;
    float CarrotBombHeight = 3 * Util.TileSize;

    void Start()
    {
        Skill1 = CarrotThrow;
        Skill1CoolDown = 0.7f;
        Skill2 = CarrotHammer;
        Skill2CoolDown = 14f;
        Skill3 = CarrotBomb;
        Skill3CoolDown = 50f;
    }

    void CarrotThrow() =>
        Util.SpawnLinearProjectile(gameObject, CarrotPrefab, CarrotThrowDamage, CarrotThrowSpeed, CarrotThrowRange, true);

    void CarrotHammer()
    {
        var facing = GetComponent<Generic>().Facing;
        var pos = transform.position + (float)(0.5 + 1.5) * Util.TileSize * facing;
        float angle;
        if (facing.x == 1 || facing.y == 1) angle = 0;
        else angle = 45;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x, pos.y),
            new Vector2(Util.TileSize * 3, Util.TileSize * 3), angle);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                //damage
                collider.gameObject.GetComponent<Generic>().Damage(CarrotHammerDamage);
                //stun
                collider.gameObject.GetComponent<EffectHandler>().AddEffect(new Effect.Stun(CarrotHammerStunDuration));
            }
        }
    }

    void CarrotBomb()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 pos2 = Random.insideUnitCircle * Util.TileSize * 5;
            Vector3 pos = transform.position + new Vector3(pos2.x, pos2.y + CarrotBombHeight, 0);
            DelayedExecutionManager.ActionDelegate bombThrow = () => {
                var bomb = Instantiate(CarrotBombPrefab, pos, Quaternion.identity);
                var bombComponent = bomb.GetComponent<DroppedCarrotBomb>();
                bombComponent.ExplosionRadious = CarrotBombExplosionRadious;
                bombComponent.ExplosionDamage = CarrotBombDamage;
                bombComponent.Height = CarrotBombHeight;
                bombComponent.Speed = CarrotBombDropSpeed;
            };
            Util.DelayedExecutionManager.ScheduleAction(bombThrow, 0.5f * (i + 1));
        }
    }
}