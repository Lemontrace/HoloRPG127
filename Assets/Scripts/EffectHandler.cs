using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public List<Effect> AllEffects;

    // Start is called before the first frame update
    void Start()
    {
        AllEffects = new List<Effect>();
    }
    
    void Update()
    {
        foreach (var effect in AllEffects) effect.Duration -= Time.deltaTime;
        AllEffects.RemoveAll((effect) => !effect.Alive);
    }

    

    public void AddEffect(Effect effect)
    {
        AllEffects.Add(effect);
    }

    public void RemoveEffect<T>()
    {
        AllEffects.RemoveAll((effect) => effect is T);
    }

}

abstract public class Effect
{
    public float Duration;

    public bool Alive { get { return Duration > 0; } }

    public Effect(float duration)
    {
        Duration = duration;
    }

    public class Stun : Root
    {
        public Stun(float duration) : base(duration) { }
    }

    public class SpeedMuliplier : Effect
    {
        public float Amount;
        public SpeedMuliplier(float duration, float amount) : base(duration)
        {
            Amount = amount; 
        }
    }

    public class Root : Effect
    {
        public Root(float duration) : base(duration) { }
    }
}