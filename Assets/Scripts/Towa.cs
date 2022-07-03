using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towa : MonoBehaviour
{

    [SerializeReference] GameObject LaserPrefab;
    float LaserWidth = Util.TileSize * 0.5f;
    float LaserReach = Util.TileSize * 5;
    float LaserSlowAmount = 15;



    [SerializeReference] GameObject CubePrefab;
    float CubeSpinRadious = Util.TileSize * 0.75f;
    float CubeDamage = 50;

    bool DevilForm = false;

    [SerializeReference] GameObject SnowstormPrefab;

    float UltDuration = 10;
    float UltSlowAmount = 30;
    float UltRadious = 2;
    float CubeSKillCoolDownWhileUlt = 5f;


    float Skill1CoolDown = 1f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 8f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 40f;
    private float Skill3Timer = 0f;


    // Update is called once per frame
    void Update()
    {

        DecreaseTimers();


        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            Laser();
        }

        //invoke skill 2
        if (Input.GetButtonDown("Skill2") && Skill2Timer <= 0)
        {
            Skill2Timer = Skill2CoolDown;
            SummonCubes();
        }

        //invoke skill 3
        if (Input.GetButtonDown("Skill3") && Skill3Timer <= 0)
        {
            Skill3Timer = Skill3CoolDown;
            Ult();
        }
    }
    void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
    }

    private float DamageBuff => GetComponent<Generic>().DamageBuff;

    //Shoots Laser in a direction, slowing all enemies on path
    void Laser()
    {
        var facing = GetComponent<Generic>().Facing;

        var position = transform.position + facing * ((LaserReach / 2) + Util.TileSize * 0.5f);
        var rotation = Quaternion.FromToRotation(Vector3.right, facing);

        var LaserSprite = Instantiate(LaserPrefab, position, rotation);
        Util.DelayedExecutionManager.ScheduleAction(() => Destroy(LaserSprite), 0.1f);

        var colliders = Physics2D.OverlapBoxAll(position, new Vector2(LaserWidth, LaserReach), rotation.eulerAngles.z + 90);
        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue;
            collider.GetComponent<EffectHandler>().AddEffect(new Effect.SpeedMuliplier(1f, 1 - LaserSlowAmount / 100));
        }
    }

    //summon cubes that fly towards nearest enemy after brief delay
    void SummonCubes()
    {
        int cubeCount;
        if (DevilForm) cubeCount = 5; else cubeCount = 4;

        Vector3 center = transform.position;
        var angleOffset = 45;
        for (int i = 0; i < cubeCount; i++)
        {
            var angle = angleOffset + 360 / cubeCount * i;
            var posOffset = Quaternion.Euler(0, 0, angle) * Vector3.right * CubeSpinRadious;
            posOffset.y *= 0.8f;
            var towaCube = Instantiate(CubePrefab, center + posOffset,Quaternion.identity).GetComponent<TowaCube>();
            towaCube.SpinCenter = gameObject;
            towaCube.Radious = CubeSpinRadious;
            towaCube.ScaleY = 0.8f;
            towaCube.Angle = angle;
            towaCube.Damage = CubeDamage + DamageBuff/cubeCount;
        }
    }

    void Ult()
    {
        //set DevilForm to true and then UltDuration seconds later, false
        DevilForm = true;
        var originalCooldown = Skill2CoolDown;
        Skill2CoolDown = CubeSKillCoolDownWhileUlt;
        Skill2Timer = 0;

        var snowstormSprite  = Instantiate(SnowstormPrefab);
        snowstormSprite.GetComponent<Follow>().ToFollow = gameObject;
        Util.DelayedExecutionManager.ScheduleAction(() => {
            DevilForm = false;
            Destroy(snowstormSprite);
            Skill2CoolDown = originalCooldown;
            }, UltDuration);


        //slow down enemy when towa's in devilform
        void applySlowRecursive()
        {
            var center = transform.position;
            center.y -= 0.5f * Util.TileSize;
            var colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), UltRadious);
            foreach (var collider in colliders)
            {
                if (!collider.gameObject.CompareTag("Enemy")) continue;
                collider.GetComponent<EffectHandler>().AddEffect(new Effect.SpeedMuliplier(0.1f, 1 - UltSlowAmount / 100));
            }
            //re-apply slow every 0.1 seconds
            Util.DelayedExecutionManager.ScheduleAction(() => { if (DevilForm) applySlowRecursive(); }, 0.1f);
        }

        applySlowRecursive();
    }
}
