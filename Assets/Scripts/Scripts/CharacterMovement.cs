using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;

    Vector2 direction;
    Rigidbody2D rb;
    public float directionIndex = 1;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] GameObject lifeDrainPrefab;
    float z = 0;
    [SerializeField] float skill1Timer = 0f;
    [SerializeField] float skill2Timer = 0f;
    float skill1Counter = 0f;
    float skill2Counter = 0f;
    bool canUseSkill1 = true;
    bool canUseSkill2 = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetScytheDirection();

        if (Input.GetKeyDown(KeyCode.Z) && canUseSkill1)
        {

            Instantiate(scythePrefab, transform.position, Quaternion.Euler(new Vector3(0, directionIndex == 1 || directionIndex == 4 ? 0 : 180, z)), transform);
            canUseSkill1 = false;
            skill1Counter = 0f;
        }
        if (Input.GetKeyDown(KeyCode.X) && canUseSkill2)
        {
            Instantiate(lifeDrainPrefab, transform);
            canUseSkill2 = false;
            skill2Counter = 0f;
        }

        CoolDownSkill1();
        CoolDownSkill2();

    }

    void CoolDownSkill1()
    {
        if (canUseSkill1)
            return;

        if(skill1Counter < skill1Timer)
        {
            skill1Counter += Time.deltaTime;
        }
        else
        {
            skill1Counter = skill1Timer;
            canUseSkill1 = true;
        }
    }

    void CoolDownSkill2()
    {
        if (canUseSkill2)
            return;

        if (skill2Counter < skill2Timer)
        {
            skill2Counter += Time.deltaTime;
        }
        else
        {
            skill2Counter = skill2Timer;
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


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * 10 * Time.deltaTime);
    }
}
