using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "Mob/MobDrop", order = 1)]
public class MobDropSO : ScriptableObject
{
    public List<DropInfo> Drops;
}
