using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayableCharacter : MonoBehaviour
{
    protected delegate void SkillDelegate();

    protected float MaxHp;
    protected float BaseMovementSpeed;
    protected float BaseDefence;

    public float Skill1Cooldown;
    public float Skill1Timer { get; private set; }
    protected SkillDelegate Skill1;


    public float Skill2Cooldown;
    public float Skill2Timer { get; protected set; }
    protected SkillDelegate Skill2;


    public float Skill3Cooldown;
    public float Skill3Timer { get; protected set; }
    protected SkillDelegate Skill3;

    virtual protected void Start()
    {
        var generic = GetComponent<Generic>();
        generic.BaseMovementSpeed = BaseMovementSpeed;
        generic.MaxHitPoint = generic.HitPoint = MaxHp;
        generic.BaseDefence = BaseDefence;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1Cooldown;
            Skill1();
        }

        //invoke skill 2
        if (Input.GetButton("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2Cooldown;
            Skill2();
        }

        //invoke skill 3
        if (Input.GetButton("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3Cooldown;
            Skill3();
        }
    }

    protected void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        if (Skill1Timer < 0) Skill1Timer = 0;
        Skill2Timer -= Time.deltaTime;
        if (Skill2Timer < 0) Skill2Timer = 0;
        Skill3Timer -= Time.deltaTime;
        if (Skill3Timer < 0) Skill3Timer = 0;
    }

    protected float DamageBuff => GetComponent<Generic>().DamageBuff;

    protected GameObject GetClosestEnemy(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, range);
        var minDist = float.PositiveInfinity;
        GameObject closest = null;
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy") &&
                Vector2.Distance((Vector2)transform.position, (Vector2)collider.transform.position) < minDist)
                closest = collider.gameObject;
        }
        return closest;
    }
}
