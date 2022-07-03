using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public static List<ItemInfo> ItemInfoList;

    public int id;
    public Sprite icon;
    public delegate void Use(GameObject user, GameObject target);
    public Use useFunction;

    public ItemInfo(int id, Sprite icon, Use useFunction)
    {
        this.id = id;
        this.icon = icon;
        this.useFunction = useFunction;
    }
}
