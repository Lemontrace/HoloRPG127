using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic : MonoBehaviour
{
    public delegate void OnHitCallback(float damage);
    public event OnHitCallback OnHit;

    public delegate void OnZeroHPCallback();
    public OnZeroHPCallback OnZeroHP;

    //TODO : maybe OnHeal if needed

    public Vector3 Facing;
    public float HitPoint;
    public float Defence;

    public void Damage(float amount)
    {
        HitPoint -= amount * (100 - Defence) / 100;
        OnHit?.Invoke(amount * (100 - Defence) / 100);
        if (HitPoint <= 0) OnZeroHP?.Invoke();
    }

    public void Heal(float amount)
    {
        HitPoint += amount;
    }

}
