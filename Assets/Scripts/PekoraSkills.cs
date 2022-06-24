using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PekoraSkills : MonoBehaviour
{

    public GameObject CarrotPrefab;
    float CarrotThrowSpeed = 15;
    float CarrotThrowRange = 15;
    float CarrotThrowDamage = 50;

    float CarrotHammerDamage = 50;
    float CarrotHammerStunDuration = 1;


    float Skill1CoolDown = 1f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 20f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 35f;
    private float Skill3Timer = 0f;

    void Update()
    {
        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButtonDown("Skill1") && Skill1Timer <= 0)
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
            //Rewind();
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
        //instantiate carrot
        GameObject carrot = Instantiate(CarrotPrefab, transform.position,
            Quaternion.FromToRotation(Vector3.right, GetComponent<Generic>().Facing));

        //set its speed, damage, range, and direction
        carrot.GetComponent<LinearBullet>().Speed = CarrotThrowSpeed;
        carrot.GetComponent<LinearBullet>().Damage = CarrotThrowDamage;
        carrot.GetComponent<LinearBullet>().Direction = GetComponent<Generic>().Facing;
        carrot.GetComponent<LinearBullet>().Range = CarrotThrowRange;
    }

    void CarrotHammer()
    {
        var facing = GetComponent<Generic>().Facing;
        var pos = transform.position + (0.5f + 1.5f) * Util.TileSize * facing;
        float angle;
        if (facing.x == 1 || facing.y == 1) angle = 0;
        else angle = 45;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x,pos.y),new Vector2(1.5f,1.5f),angle);
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
        for (int i=0;i>5;i++)
        {
            Vector2 pos2 = Random.insideUnitCircle * Util.TileSize * 5;
            Vector3 pos3 = new Vector3(pos2.x, pos2.y, 0);
        }

        DelayedExecutionManager.ActionDelegate bombThrow = () => {  };
    }
}
