using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PekoraSkills : MonoBehaviour
{

    public GameObject CarrotPrefab;
    float CarrotSpeed = 15;
    float CarrotRange = 15;
    float CarrotDamage = 50;


    float Skill1CoolDown = 1f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 65f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 35f;
    private float Skill3Timer = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        DecreaseTimers();

        //invoke skill 1
        if (Input.GetButtonDown("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            Throw();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            //GroundPound();
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

    void Throw()
    {
        //instantiate carrot
        GameObject carrot = Instantiate(CarrotPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, GetComponent<Generic>().Facing));

        //set its speed, damage, range, and direction
        carrot.GetComponent<LinearBullet>().Speed = CarrotSpeed;
        carrot.GetComponent<LinearBullet>().Damage = CarrotDamage;
        carrot.GetComponent<LinearBullet>().Direction = GetComponent<Generic>().Facing;
        carrot.GetComponent<LinearBullet>().Range = CarrotRange;
    }
}
