using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static DelayedExecutionManager DelayedExecutionManager
        => GameObject.Find("Managers").GetComponent<DelayedExecutionManager>();

    public const float TileSize = 1.2058f;
}