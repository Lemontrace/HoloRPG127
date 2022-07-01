using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

[System.Serializable]
public class Dialogue
{
    public LocalizedStringTable localizedStringTable;
    public string name;
    public string localizedText;

    public (string, string)[] sentences { get { return testing(); } }

    void UpdateString(string s)
    {
        localizedText = s;
    }

    public (string, string)[] testing()
    {
        Debug.Log(localizedStringTable.GetTable().Values);
        var test = localizedStringTable.GetTable().Values;
        var sentence = new List<(string, string)>();

        foreach (StringTableEntry entry in test)
        {
            Debug.Log($"key={entry.Key}, value={entry.Value}, localizeValue={entry.LocalizedValue}");
            sentence.Add((entry.Key, entry.Value));
        }

        return sentence.ToArray();
    }
}