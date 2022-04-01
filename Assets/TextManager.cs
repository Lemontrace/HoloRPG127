using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{

    public Text Label;
    public Image LabelBackground;
    public Text Text;
    public Image TextBackground;
    public bool TextVisibility { get { return TextVisibility; } set {
            if (value) Text.enabled = TextBackground.enabled = Label.enabled = LabelBackground.enabled = true;
            else Text.enabled = TextBackground.enabled = Label.enabled = LabelBackground.enabled = false;
        } }

    public void setLabel(string str)
    {
        Label.text = str;
    } 

    public void setText(string str)
    {
        Text.text = str;
    }
}
