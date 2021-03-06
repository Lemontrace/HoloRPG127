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

    private void Awake()
    {
        MaxHp = 1000;
        BaseDefence = 10;
        BaseMovementSpeed = Util.SpeedUnitConversion(350);

        Skill1 = Shoot;
        Skill1Cooldown = 0.6f;
        Skill2 = GroundPound;
        Skill2Cooldown = 20f;
        Skill3 = Rewind;
        Skill3Cooldown = 35f;
    }

    protected override void Start()
    {
        PositionRecordTimer = RewindResolution;
        //initialize PastPositions
        for (int i = 0; i < RewindLength / RewindResolution; i++) PastPositions.AddLast(transform.position);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        PositionRecordTimer -= Time.deltaTime;
        //record position
        if (PositionRecordTimer <= 0)
        {
            PositionRecordTimer = RewindResolution;
            RecordPosition();
        }
        base.Update();
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
            bullet.GetComponent<LinearMovement>().Speed = BulletSpeed;
            bullet.GetComponent<FriendlyProjectile>().Damage = BulletDamage + DamageBuff / BulletCount;

            //add randomness to its direction
            Quaternion randomness = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-BulletSpread / 2, +BulletSpread / 2));
            bullet.GetComponent<LinearMovement>().Direction = randomness * GetComponent<Generic>().Facing;

            //set random range
            bullet.GetComponent<LinearMovement>().Range = UnityEngine.Random.Range(MaxBulletRange / 3, MaxBulletRange);

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
                if (obj.CompareTag("Enemy")) obj.GetComponent<Generic>().Damage(GroundPoundDamage + DamageBuff);
            }
        }, GroundPoundDelay);
    }

    //TODO record and recover hp
    void Rewind()
    {
        //make the character unable to move during animation
        GetComponent<EffectHandler>().AddEffect(new Effect.Stun(RewindDelay));
        GetComponent<EffectHandler>().AddEffect(new Effect.Invincibility(RewindDelay));
        //save rewind position
        Vector3 rewindPosition = PastPositions.First.Value;
        //TODO : start rewind animation
        Util.DelayedExecutionManager.ScheduleAction(() => { transform.position = rewindPosition; }, RewindDelay);
    }
}
