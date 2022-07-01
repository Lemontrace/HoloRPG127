using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PekoraSkills : MonoBehaviour
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

    float Skill1CoolDown = 1f;
    float Skill1Timer = 0f;

    float Skill2CoolDown = 20f;
    float Skill2Timer = 0f;

    float Skill3CoolDown = 70f;
    float Skill3Timer = 0f;

    void Update()
    {
        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            CarrotThrow();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            CarrotHammer();
        }

        //invoke skill 3
        if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            CarrotBomb();
        }
    }

    void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    void CarrotThrow()
    {
        Util.SpawnLinearProjectile(gameObject, CarrotPrefab, CarrotThrowDamage, CarrotThrowSpeed, CarrotThrowRange, true);        
    }

    void CarrotHammer()
    {
        var facing = GetComponent<Generic>().Facing;
        var pos = transform.position + (float)(0.5 + 1.5) * Util.TileSize * facing;
        float angle;
        if (facing.x == 1 || facing.y == 1) angle = 0;
        else angle = 45;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x,pos.y),
            new Vector2(Util.TileSize*3,Util.TileSize*3),angle);
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
        for (int i=0;i<5;i++)
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
            Util.DelayedExecutionManager.ScheduleAction(bombThrow, 0.5f * (i+1));
        }
    }
}
