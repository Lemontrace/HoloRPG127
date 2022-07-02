using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class Dialogue
{
    public string name;
    public LocalizedString localizedString;
    private string localizedText;
    public Sprite portraitSprite = null;

    public void OnEnable()
    {
        OnDisable(); // Prevent multiple subscription to an event
        localizedString.StringChanged += UpdateString;
    }

    public void OnDisable()
    {
        localizedString.StringChanged -= UpdateString;
    }

    void UpdateString(string s)
    {
        localizedText = s;
    }

    public string GetLocalizedText()
    {
        return localizedText;
    }
}