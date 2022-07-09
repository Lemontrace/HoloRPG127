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
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    protected float DamageBuff => GetComponent<Generic>().DamageBuff;
}
