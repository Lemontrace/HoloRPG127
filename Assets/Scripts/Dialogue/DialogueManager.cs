using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image portraitLeft;
    public Image portraitRight;

    private DialogueSO dialogueSO;
    private Queue<Dialogue> dialogues;

    private Color portraitTint = new Color(.4f, .4f, .4f);

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
        portraitLeft.enabled = false;
        portraitRight.enabled = false;
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
        SetPortrait(dialogue);
        StartCoroutine(TypeSentence(dialogue.GetLocalizedText()));
    }

    /**
     * NOTE: currently only support 2 character portraits
     */
    private void SetPortrait(Dialogue dialogue)
    {
        if (!dialogue.portraitSprite)
        {
            return;
        }

        portraitLeft.color = portraitTint;
        portraitRight.color = portraitTint;

        if (dialogue.portraitSprite == portraitLeft.sprite)
        {
            portraitLeft.color = Color.white;
        }
        else if (dialogue.portraitSprite == portraitRight.sprite)
        {
            portraitRight.color = Color.white;
        }

        if (!portraitLeft.enabled)
        {
            portraitLeft.enabled = true;
            portraitLeft.color = Color.white;
            portraitLeft.sprite = dialogue.portraitSprite;
            return;
        }
        else if (!portraitRight.enabled)
        {
            portraitRight.enabled = true;
            portraitRight.color = Color.white;
            portraitRight.sprite = dialogue.portraitSprite;
            return;
        }

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
