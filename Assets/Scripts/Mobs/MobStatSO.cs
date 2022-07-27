using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Mob/MobStat", order = 1)]
public class MobStatSO : ScriptableObject
{
    public float HitPoint;
    public float Attack;
    public float BaseMovementSpeed;
    public float BaseDefence;
}
