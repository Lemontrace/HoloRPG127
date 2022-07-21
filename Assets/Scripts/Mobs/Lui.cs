using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lui : MonoBehaviour
{
    Timer BasicAttackTimer = new Timer(0.7f);
    Timer BasicSkillTimer = new Timer(7);

    private void Start()
    {
        BasicAttackTimer.Start();
        BasicSkillTimer.Start();
    }

}
