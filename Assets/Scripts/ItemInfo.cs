using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public static List<ItemInfo> ItemInfoList;

    public int id;
    public delegate void Use(GameObject user, GameObject target);
    public Use useFunction;

    public ItemInfo(int id, Use useFunction = null)
    {
        this.id = id;
        this.useFunction = useFunction;
    }
}
