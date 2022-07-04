using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towa : PlayableCharacter
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
    float Skill2CoolDownWhileUlt = 5f;

    void Start()
    {
        Skill1 = Laser;
        Skill1CoolDown = 1f;
        Skill2 = SummonCubes;
        Skill2CoolDown = 5f;
        Skill3 = Ult;
        Skill3CoolDown = 60f;
    }


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
        Vector3 center = transform.position;
        int cubeCount;
        if (DevilForm) cubeCount = 5; else cubeCount = 4;
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
            towaCube.Damage = CubeDamage;
        }
    }

    void Ult()
    {
        //set DevilForm to true and then UltDuration seconds later, false
        DevilForm = true;
        var originalCooldown = Skill2CoolDown;
        Skill2CoolDown = Skill2CoolDownWhileUlt;
        Skill2Timer = 0;

        var snowstormSprite  = Instantiate(SnowstormPrefab);
        snowstormSprite.GetComponent<Follow>().ToFollow = gameObject;
        Util.DelayedExecutionManager.ScheduleAction(() => {
            DevilForm = false;
            Destroy(snowstormSprite);
            Skill2CoolDown = originalCooldown;
            }, UltDuration);


        //slow down enemy when towa's in devilform
        void action()
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
            if (DevilForm) Util.DelayedExecutionManager.ScheduleAction(() => { if (DevilForm) action(); }, 0.1f);
        }

        action();
    }
}
