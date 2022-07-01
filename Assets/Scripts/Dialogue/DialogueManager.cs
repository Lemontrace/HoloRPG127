using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<(string, string)> sentences;

    void Start()
    {
        sentences = new Queue<(string, string)>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach ((string, string) sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            return;
        }

        StopAllCoroutines();

        (string key, string text) sentence = sentences.Dequeue();
        nameText.text = sentence.key;
        StartCoroutine(TypeSentence(sentence.text));
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
