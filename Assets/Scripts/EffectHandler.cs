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
