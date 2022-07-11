using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void onHitDelegate(GameObject target);
    public event onHitDelegate OnHit;
    protected void InvokeOnHit(GameObject target) => OnHit?.Invoke(target);
}
