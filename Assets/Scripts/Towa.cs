using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towa : MonoBehaviour
{
    /*x`
    ���ھ߹� ��ͻ縶
    ü�� 800 ���� 8%
    ��Ÿ - ���� �� �����ϸ� �������� 1�ʰ� 15%�����Ѵ� �� �������� ����
    1��ų - ��Ÿ�� 8�� - ������ 50¥�� �˺������ ť�긦 4�� ��ȯ�Ͽ� ���� ����� ��󿡰� ������ 
    �ñر� -��Ÿ�� 100�� - ������ 40�ʰ� ������� ���°� �Ǹ� �ֺ� ������2ĭ¥�� ���ȿ� ������ �ҷ��� ���濡�� 30% ����ȿ���� �ش�
    ���� ��Ÿ����� �Ұ��������� ��� 1��ų ť���� ������ 5�����ǰ� ��Ÿ���� 5�ʰ� �ȴ� 
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
