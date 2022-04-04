using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public struct InventoryEntry
    {
        InventoryEntry(string id, int count = 1, string itemData = "{}")
        {
            Id = id;
            Count = count;
            ItemData = itemData;
        }

        string Id;
        int Count;
        string ItemData;
    }

    static string InventorySavePath = "Inventory.json";


    ArrayList Inventory;
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
            Inventory = new ArrayList();
        } else
        {
            string JSONString = File.ReadAllText(savePath);
            Inventory = JsonUtility.FromJson<ArrayList>(JSONString);
        }
    }

    void Save()
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
