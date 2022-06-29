using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowaCube : MonoBehaviour
{
    enum CubePhase
    {
        Spin,Seek
    }

    public GameObject SpinCenter;
    public float Radious;
    public float ScaleY;
    public float Angle;
    public float Damage;


    float AngularVelocity = 180;
    float SpinTimer = 1.5f;

    GameObject SeekTarget;
    float SeekRadious = Util.TileSize * 5;
    float SeekSpeed = Util.TileSize * 10;
    float HitRadious = Util.TileSize * 0.5f;

    CubePhase Phase = CubePhase.Spin;

    void Update()
    {
        switch (Phase)
        {
            case CubePhase.Spin:
                UpdateOnSpin();
                break;
            case CubePhase.Seek:
                UpdateOnSeek();
                break;
            default:
                break;
        }
    }

    void UpdateOnSpin()
    {
        //calculate cube's rotation
        Angle += AngularVelocity * Time.deltaTime;
        Angle %= 360;
        //make sure cube's rendered correctly
        if (Angle > 180) GetComponent<SpriteRenderer>().sortingOrder = 1;
        else GetComponent<SpriteRenderer>().sortingOrder = -1;
        //apply cube's rotation
        var posOffset = Quaternion.Euler(0, 0, Angle) * Vector3.right * Radious;
        posOffset.y *= ScaleY;
        transform.position = SpinCenter.transform.position + posOffset;


        SpinTimer -= Time.deltaTime;
        if (SpinTimer <= 0) //set seek target and change phase
        {
            var center = SpinCenter.transform.position;
            var colliders = Physics2D.OverlapCircleAll(new Vector2(center.x, center.y), SeekRadious);

            var minDistance = float.PositiveInfinity;
            SeekTarget = null;
            foreach (var collider in colliders)
            {
                if (!collider.gameObject.CompareTag("Enemy")) continue;
                var distance = Vector3.Distance(center, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    SeekTarget = collider.gameObject;
                }
            }
            if (SeekTarget == null) Destroy(gameObject);
            else Phase = CubePhase.Seek;
        }
    }

    void UpdateOnSeek()
    {
        if (SeekTarget == null) { 
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, SeekTarget.transform.position, SeekSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, SeekTarget.transform.position) < HitRadious)
        {
            SeekTarget.GetComponent<Generic>().Damage(Damage);
            Destroy(gameObject);
        }
    }

    
}
