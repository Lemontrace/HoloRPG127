using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Conversation/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public LocalizedStringTable localizedStringTable;
    public List<Dialogue> dialogues;
}
