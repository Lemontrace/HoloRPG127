using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private DialogueSO dialogueSO;
    private Queue<Dialogue> dialogues;

    void Start()
    {
        dialogues = new Queue<Dialogue>();
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        this.dialogueSO = dialogueSO;
        dialogues.Clear();

        foreach (Dialogue dialogue in dialogueSO.dialogues)
        {
            dialogues.Enqueue(dialogue);
        }

        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        if (!dialogueSO) return;

        foreach (Dialogue dialogue in dialogueSO.dialogues)
        {
            dialogue.OnDisable();
        }
        dialogueSO = null;
        nameText.text = "";
        dialogueText.text = "End of Dialogue";
    }

    public void DisplayNextSentence()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();

        Dialogue dialogue = dialogues.Dequeue();
        dialogue.OnEnable();
        nameText.text = dialogue.name;
        StartCoroutine(TypeSentence(dialogue.GetLocalizedText()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.01f);
            yield return null;
        }
    }
}
