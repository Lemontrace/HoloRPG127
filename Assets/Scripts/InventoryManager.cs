using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

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

    static string InventorySavePath = "Inventory.json";


    //returns copy of ItemList so it can't be modified from outside
    public List<InventoryEntry> Inventory {get { return new List<InventoryEntry>(ItemList); } }
    public int EntryCount { get { return ItemList.Count; } }


    public int MaxInventoryEntry = 30;


    //actual item data
    List<InventoryEntry> ItemList;


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

    //tries to remove @param count item from @param slot
    //returns if items are actually removed
    public bool TryToRemoveItem(int slot, int count)
    {
        var entry = ItemList[slot];
        if (entry.Count >= count)
        {
            entry.Count -= count;
            return true;
        }
        else return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }
    
    void Load()
    {
        //file path
        string savePath = Path.Combine(Application.persistentDataPath, InventorySavePath);
        if (!File.Exists(savePath))
        {
            //file doesn't exist, create empty inventory
            ItemList = new List<InventoryEntry>();
        } else
        {
            //load inventory from JSON
            string JSONString = File.ReadAllText(savePath);
            ItemList = JsonUtility.FromJson<List<InventoryEntry>>(JSONString);
        }
    }

    public void Save()
    {
        string savePath = Path.Combine(Application.persistentDataPath, InventorySavePath);
        File.WriteAllText(savePath, JsonUtility.ToJson(Inventory));
    }


    //save on quit
    private void OnApplicationQuit()
    {
        Save();
    }
}
