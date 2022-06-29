using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towa : MonoBehaviour
{
    /*x`
    토코야미 토와사마
    체력 800 방어력 8%
    평타 - 총을 쏴 공격하며 맞은적은 1초간 15%감속한다 각 데미지는 없다
    1스킬 - 쿨타임 8초 - 데미지 50짜리 검보라색의 큐브를 4개 소환하여 가장 가까운 대상에게 날린다 
    궁극기 -쿨타임 100초 - 시전시 40초간 각성토와 상태가 되며 주변 반지름2칸짜리 원안에 눈보라를 불러와 상대방에게 30% 감속효과를 준다
    또한 평타사용이 불가능해지고 대신 1스킬 큐브의 개수가 5개가되고 쿨타임이 5초가 된다 
    */

    [SerializeReference]
    GameObject CubePrefab;


    float CubeSpinRadious = Util.TileSize * 0.75f;
    float CubeDamage = 50;

    bool DevilForm = false;

    float UltDuration = 10;
    float UltSlowAmount = 30;
    float UltRadious = 2;
    List<GameObject> EnemiesInUltRange;


    float Skill1CoolDown = 1f;
    private float Skill1Timer = 0f;

    float Skill2CoolDown = 0f;
    private float Skill2Timer = 0f;

    float Skill3CoolDown = 40f;
    private float Skill3Timer = 0f;

    void Start()
    {
        EnemiesInUltRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        DecreaseTimers();


        //invoke skill 1
        if (Input.GetButton("Skill1") && Skill1Timer <= 0)
        {
            Skill1Timer = Skill1CoolDown;
            //Shoot();
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

        //slow down enemy when towa's in devilform
    }
    void DecreaseTimers()
    {
        Skill1Timer -= Time.deltaTime;
        Skill2Timer -= Time.deltaTime;
        Skill3Timer -= Time.deltaTime;
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
        Util.DelayedExecutionManager.ScheduleAction(() => DevilForm = false, UltDuration);

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
            if (DevilForm) Util.DelayedExecutionManager.ScheduleAction(() => { if (DevilForm) action(); }, 0.1f);
        }


        action();
    }
}
