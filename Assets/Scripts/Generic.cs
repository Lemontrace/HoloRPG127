using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic : MonoBehaviour
{
    public delegate void OnHitCallback(float damage);
    public event OnHitCallback OnHit;

    public delegate void OnZeroHPCallback();
    public event OnZeroHPCallback OnZeroHP;

    //TODO : maybe OnHeal if needed

    private EffectHandler effectHandler;

    public Vector3 Facing = Vector3.right;

    public float MaxHitPoint;

    public float HitPoint;

    public float BaseDefence;
    public float Defence
    {
        get;set;
        /*
        get
        {
            float value = BaseDefence;
            foreach (var effect in effectHandler.AllEffects)
                if (effect is Effect.DefenceMuliplier) value *= ((Effect.DefenceMuliplier)effect).Amount;
            return value;
        }
        */
    }

    public float BaseMovementSpeed;
    public float MovementSpeed {
        get {
            if (Rooted) return 0;

            float value = BaseMovementSpeed;
            foreach (var effect in effectHandler.AllEffects)
                if (effect is Effect.SpeedMuliplier) value *= ((Effect.SpeedMuliplier)effect).Amount;
            return value;
        }
    }

    public bool Stuned { get { return effectHandler.AllEffects.Exists((effect) => effect is Effect.Stun);  } }
    public bool Rooted { get { return Stuned || effectHandler.AllEffects.Exists((effect) => effect is Effect.Root); } }

    private void Start()
    {
        effectHandler = GetComponent<EffectHandler>();
    }


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
