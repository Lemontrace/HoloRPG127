using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmeSkills : MonoBehaviour
{

    int BulletCount = 20;
    float BulletSpread = 30; //spread in degrees
    float BulletSpeed = 15;
    float MaxBulletRange = 7;
    float BulletDamage = 2;

    float GroundPoundDamage = 150f;
    float GroundPoundRadious = 2f;
    float GroundPoundDelay = 1f;

    LinkedList<Vector3> PastPositions = new LinkedList<Vector3>();
    private float PositionRecordTimer = 0.0f;
    float RewindDelay = 1.5f;
    float RewindResolution = 0.1f;
    float RewindLength = 3f;

    public GameObject BulletPrefab;

    float Skill1CoolDown = 1f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 65f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 35f;
    private float Skill3Timer = 0f;

    void Start()
    {
        PositionRecordTimer = RewindResolution;


        //initialize PastPositions
        for(int i=0;i<RewindLength/RewindResolution;i++)PastPositions.AddLast(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        DecreaseTimers();

        //record position
        if (PositionRecordTimer <= 0) RecordPosition();


        //invoke skill 1
        if (Input.GetButtonDown("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            Shoot();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            GroundPound();
        }

        //invoke skill 3
            if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            Rewind();
        }


    }
    void DecreaseTimers()
    {
        PositionRecordTimer -= Time.deltaTime;
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    void Shoot()
    {
        for (int i = 0; i < BulletCount; i++)
        {
            //instantiate bullet
            GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

            //set its speed and damage
            bullet.GetComponent<LinearBullet>().Speed = BulletSpeed;
            bullet.GetComponent<LinearBullet>().Damage = BulletDamage;

            //add randomness to its direction
            Quaternion randomness = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-BulletSpread / 2, +BulletSpread / 2));
            bullet.GetComponent<LinearBullet>().Direction = randomness * GetComponent<Generic>().Facing;

            //set random range
            bullet.GetComponent<LinearBullet>().Range = UnityEngine.Random.Range(MaxBulletRange / 3, MaxBulletRange);

        }

    }

    void GroundPound()
    {
        //TODO : play ground pound animation
        
        Invoke(nameof(GroundPound_0), GroundPoundDelay);
    }
    void GroundPound_0()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), GroundPoundRadious);
        foreach (var obj in objectsInRange)
        {
            if (obj.CompareTag("Enemy")) obj.GetComponent<Generic>().Damage(GroundPoundDamage);
        }
    }


    private void RecordPosition()
    {
        PositionRecordTimer = RewindResolution;
        PastPositions.AddLast(transform.position);
        PastPositions.RemoveFirst();
    }

    Vector3 RewindPosition;
    float MovementSpeedTemp;

    void Rewind()
    {
        SimpleControl simpleControl = GetComponent<SimpleControl>();
        //save movement speed
        MovementSpeedTemp = simpleControl.MovementSpeed;
        //set movement speed to 0
        simpleControl.MovementSpeed = 0;
        //save rewind position
        RewindPosition = PastPositions.First.Value;
        //start rewind animation
        //TODO
        Invoke(nameof(Rewind_0), RewindDelay);
    }

    void Rewind_0()
    {
        //rewind position
        transform.position = RewindPosition;
        //restore movement speed
        GetComponent<SimpleControl>().MovementSpeed = MovementSpeedTemp;
    }
}
