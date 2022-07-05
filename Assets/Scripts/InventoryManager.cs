using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    const int DataVersion = 0;
    public struct InventoryEntry
    {
        public InventoryEntry(string id, int count = 1, string itemData = "{}")
        {
            Id = id;
            Count = count;
            ItemData = itemData;
        }

        public string Id;
        public int Count;
        public string ItemData;
    }

    private struct InventoryInfo
    {
        public int DataVersion;
        public int MaxEntry;
        public List<InventoryEntry> InventoryEntries;
    }


    //returns copy of ItemList so it can't be modified from outside
    public List<InventoryEntry> Inventory {get { return new List<InventoryEntry>(ItemList); } }
    public int EntryCount { get { return ItemList.Count; } }

    public int MaxInventoryEntry = 30;


    //actual item data
    private List<InventoryEntry> ItemList;

    public InventoryManager(int maxEntry, List<InventoryEntry> inventoryEntries)
    {
        ItemList = inventoryEntries;
        MaxInventoryEntry = maxEntry;
    }


    public bool TryToAddItem(InventoryEntry entry)
    {
        return TryToAddItem(entry.Id, entry.Count, entry.ItemData);
    }


    //tries to add item to inventory
    //returns if item is actually added
    public bool TryToAddItem(string id, int count, string itemData = "{}")
    {
         //scan ItemList for matching item
        for (int i = 0; i < ItemList.Count; i++)
        {
            InventoryEntry entry = ItemList[i];
            if (entry.Id.Equals(id) && entry.ItemData.Equals(itemData))
            {
                //enty exists! just increment count
                entry.Count += count;
                return true;
            }
        }


        //if this is reached, entry doesn't already exist


        //inventory full!!
        if (ItemList.Count == MaxInventoryEntry) return false;

        //create new entry for list
        var newEntry = new InventoryEntry(id, count, itemData);
        //add it
        ItemList.Add(newEntry);
        return true;
    }

    //tries to remove count item from slot
    //returns if items are actually removed
    public bool TryToRemoveItem(int slot, int count)
    {
        var entry = ItemList[slot];
        if (entry.Count >= count)
        {
            entry.Count -= count;
            if (entry.Count == 0) ItemList.RemoveAt(slot);
            return true;
        }
        else return false;
    }
    
    //loads InventoryManager from path, returning null if the file doesn't exist
    public static InventoryManager LoadInventory(string JSONString)
    {        
        var inventoryInfo = JsonUtility.FromJson<InventoryInfo>(JSONString);
        return new InventoryManager(inventoryInfo.MaxEntry, inventoryInfo.InventoryEntries);
    }

    //save InventoryManager to path
    public string SaveInventory()
    {
        var inventoryInfo = new InventoryInfo();
        inventoryInfo.InventoryEntries = ItemList;
        inventoryInfo.MaxEntry = MaxInventoryEntry;
        inventoryInfo.DataVersion = DataVersion;
        return JsonUtility.ToJson(inventoryInfo);
    }
}