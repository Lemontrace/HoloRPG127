using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayableCharacter : MonoBehaviour
{
    protected delegate void SkillDelegate();

    public float Skill1CoolDown;
    public float Skill1Timer { get; private set; }
    protected SkillDelegate Skill1;


    public float Skill2CoolDown;
    public float Skill2Timer { get; protected set; }
    protected SkillDelegate Skill2;


    public float Skill3CoolDown;
    public float Skill3Timer { get; protected set; }
    protected SkillDelegate Skill3;

    // Update is called once per frame
    virtual protected void Update()
    {
        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            Skill1();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            Skill2();
        }

        //invoke skill 3
        if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            Skill3();
        }
    }

    protected void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }
}
