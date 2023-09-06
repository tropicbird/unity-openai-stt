using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public Text uiText;
    public SpeechToText speechToText;

    public void Start()
    {
        speechToText.OnTextReceived += UpdateText;
    }

    public void UpdateText(string text)
    {
        uiText.text = text;
    }
}
