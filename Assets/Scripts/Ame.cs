using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ame : PlayableCharacter
{

    [SerializeReference] GameObject BulletPrefab;
    int BulletCount = 20;
    float BulletSpread = 30; //spread in degrees
    float BulletSpeed = 15;
    float MaxBulletRange = 7;
    float BulletDamage = 2;

    float GroundPoundDamage = 150f;
    float GroundPoundRadious = 2f;
    float GroundPoundDelay = 1f;

    private LinkedList<Vector3> PastPositions = new LinkedList<Vector3>();
    private float PositionRecordTimer;
    float RewindDelay = 1.5f;
    float RewindResolution = 0.1f;
    float RewindLength = 3f;

    void Start()
    {
        Skill1 = Shoot;
        Skill1CoolDown = 0.6f;
        Skill2 = GroundPound;
        Skill2CoolDown = 20f;
        Skill3 = Rewind;
        Skill3CoolDown = 35f;

        PositionRecordTimer = RewindResolution;
        //initialize PastPositions
        for (int i = 0; i < RewindLength / RewindResolution; i++) PastPositions.AddLast(transform.position);
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        PositionRecordTimer -= Time.deltaTime;
        //record position
        if (PositionRecordTimer <= 0)
        {
            PositionRecordTimer = RewindResolution;
            RecordPosition();
        }
    }

    private void RecordPosition()
    {
        PastPositions.AddLast(transform.position);
        PastPositions.RemoveFirst();
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
        Util.DelayedExecutionManager.ScheduleAction(() =>
        {
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), GroundPoundRadious);
            foreach (var obj in objectsInRange)
            {
                if (obj.CompareTag("Enemy")) obj.GetComponent<Generic>().Damage(GroundPoundDamage);
            }
        }, GroundPoundDelay);
    }

    //TODO record and recover hp
    void Rewind()
    {
        //make the character unable to move during animation
        GetComponent<EffectHandler>().AddEffect(new Effect.Stun(RewindDelay));
        //save rewind position
        Vector3 rewindPosition = PastPositions.First.Value;
        //TODO : start rewind animation
        Util.DelayedExecutionManager.ScheduleAction(() => { transform.position = rewindPosition; }, RewindDelay);
    }
}
