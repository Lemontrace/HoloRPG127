using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharaControl
{
    void DecreaseTimers(); // Cooltime reduce per frame
    float[] GetSkillsTimer(); // Get array of cooltime & current timer of each skills.
                              // INDEX : 0 = sk0 current / 1 = sk1 current / 2 = sk2 current / 3 = sk0 cool / 4 = sk1 cool / 5 = sk2 cool
    int GetHP();
    int GetMaxHP(); // Get current & max hp
    void GainDamage(int damage);
}
