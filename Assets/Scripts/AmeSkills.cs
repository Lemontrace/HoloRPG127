using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmeSkills : MonoBehaviour
{

    LinkedList<Vector3> PastPositions = new LinkedList<Vector3>();
    private float PositionRecordTimer = 0.0f;
    float RewindResolution = 0.1f;
    float RewindLength = 5f;

    public GameObject BulletPrefab;

    float Skill1CoolDown = 0.5f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 10f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 20f;
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

    int GroundPoundDamage;
    float GroundPoundDelay = 1f;


    void GroundPound()
    {
        //TODO : play ground pound animation

        Invoke(nameof(GroundPound0), GroundPoundDelay);
    }
    void GroundPound0()
    {
        
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.z), 2);
        foreach (var obj in objectsInRange)
        {
            if (obj.CompareTag("Enemy")) obj.GetComponent<Generic>().HitPoint -= GroundPoundDamage;
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
        GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<LinearBullet>().Speed = 10f;
        bullet.GetComponent<LinearBullet>().Direction = gameObject.GetComponent<Generic>().Facing;
    }





    private void RecordPosition()
    {
        if (PositionRecordTimer <= 0)
        {
            PositionRecordTimer = RewindResolution;
            PastPositions.AddLast(transform.position);
            PastPositions.RemoveFirst();
        }
    }

    float RewindDelay = 2f;
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
