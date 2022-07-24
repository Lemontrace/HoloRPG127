using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCalliope : PlayableCharacter
{
    float BasicAttackDamage = 40f;
    float LifeDrainDamage = 0f;
    float UltDamage = 0f;

    [SerializeReference] GameObject scythePrefab;
    float ScytheAttackArc = 150f;

    [SerializeReference] GameObject lifeDrainPrefab;
    [SerializeReference] GameObject deadBeatsUltsPrefab;


    protected override void Start()
    {
        Skill1 = ScytheAttack;
        Skill1CoolDown = 0.9f;
        Skill2 = LifeDrain;
        Skill2CoolDown = 12;
        Skill3 = DeadBeats;
        Skill3CoolDown = 30;
    }

    void ScytheAttack()
    {
        GameObject scythe = Instantiate(scythePrefab, transform.position, Quaternion.FromToRotation(Vector3.right, GetComponent<Generic>().Facing));
        scythe.GetComponent<ScytheAttack>().AttackArc = ScytheAttackArc;
        scythe.GetComponent<HostileObject>().Damage = BasicAttackDamage;
    }

    void LifeDrain()
    {
        //TODO
    }
    
    void DeadBeats()
    {
        GameObject DeadBeat = Instantiate(deadBeatsUltsPrefab, transform);
        DeadBeat.GetComponent<HostileObject>().Damage = UltDamage;
    }

}