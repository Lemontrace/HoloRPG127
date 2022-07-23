using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCalliopeManager : MonoBehaviour
{
    public static MoriCalliopeManager instance;

    Generic characterSetUp;
    Rigidbody2D rb;
    Vector2 direction;
    float z = 0;

    public float directionIndex = 1;

    [SerializeField] float basicAttackDamage = 0f;
    [SerializeField] float lifeDrainDamage = 0f;
    [SerializeField] float ultiDamage = 0f;

    [SerializeField] GameObject scythePrefab;
    [SerializeField] GameObject lifeDrainPrefab;
    [SerializeField] GameObject deadBeatsUltsPrefab;

    [SerializeField] float skill1Cooldown = 0f;
    [SerializeField] float skill2Cooldown = 0f;
    [SerializeField] float skill3Cooldown = 0f;

    float skill1Counter = 0f;
    float skill2Counter = 0f;
    float skill3Counter = 0f;

    bool canUseSkill1 = true;
    bool canUseSkill2 = true;
    bool canUseSkill3 = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb?.GetComponent<Rigidbody2D>();
        characterSetUp?.GetComponent<Generic>();
    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetScytheDirection();

        if (Input.GetKeyDown(KeyCode.Z) && canUseSkill1)
        {
            GameObject g = Instantiate(scythePrefab, transform.position, Quaternion.Euler(new Vector3(0, directionIndex == 1 || directionIndex == 4 ? 0 : 180, z)), transform);
            g.GetComponent<CharacterAttack>().damage = AttackDamage(basicAttackDamage);
            canUseSkill1 = false;
            skill1Counter = 0f;
        }
        if (Input.GetKeyDown(KeyCode.X) && canUseSkill2)
        {
            GameObject g = Instantiate(lifeDrainPrefab, transform);
            g.GetComponent <CharacterAttack>().damage = AttackDamage(lifeDrainDamage);
            canUseSkill2 = false;
            skill2Counter = 0f;
        }
        if(Input.GetKeyDown(KeyCode.C) && canUseSkill3)
        {
            GameObject g = Instantiate(deadBeatsUltsPrefab, transform);
            g.GetComponent<CharacterAttack>().damage = AttackDamage(ultiDamage);
            canUseSkill3 = false;
            skill3Counter = 0f;
        }

        CoolDownSkill1();
        CoolDownSkill2();
        CoolDownSkill3();
    }

    void CoolDownSkill1()
    {
        if (canUseSkill1)
            return;

        if(skill1Counter < skill1Cooldown)
        {
            skill1Counter += Time.deltaTime;
        }
        else
        {
            skill1Counter = skill1Cooldown;
            canUseSkill1 = true;
        }
    }

    void CoolDownSkill2()
    {
        if (canUseSkill2)
            return;

        if (skill2Counter < skill2Cooldown)
        {
            skill2Counter += Time.deltaTime;
        }
        else
        {
            skill2Counter = skill2Cooldown;
            canUseSkill2 = true;
        }
    }

    void CoolDownSkill3()
    {
        if (canUseSkill3)
            return;

        if (skill3Counter < skill3Cooldown)
        {
            skill3Counter += Time.deltaTime;
        }
        else
        {
            skill3Counter = skill3Cooldown;
            canUseSkill2 = true;
        }
    }

    private void SetScytheDirection()
    {
        if (direction != Vector2.zero)
        {
            if (direction.x == 1)
                directionIndex = 1;
            else if (direction.y == 1)
                directionIndex = 2;
            else if (direction.x == -1)
                directionIndex = 3;
            else if (direction.y == -1)
                directionIndex = 4;
        }

        if (directionIndex == 2)
            z = 90;
        else if (directionIndex == 4)
            z = 270;
        else
            z = 0;

    }

    public float AttackDamage(float finalDamage)
    {
        return finalDamage < 0 ? 0 : finalDamage;
    }

}
