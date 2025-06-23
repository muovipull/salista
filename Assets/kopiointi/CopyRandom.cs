using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class CopyRandom : MonoBehaviour
{
    public TextMeshProUGUI textToCopy;

    // Tuo JavaScript-funktio (vain WebGL-k�yt�ss�)
    #if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void CopyTextToClipboard(string text);
    #endif

    public void CopyText()
    {
        if (textToCopy != null)
        {
            // Tarkista, ollaanko WebGL-alustalla
            #if UNITY_WEBGL
                CopyTextToClipboard(textToCopy.text);
                Debug.Log("Teksti l�hetetty kopioitavaksi WebGL:ss�: " + textToCopy.text);
            #else
                TextEditor textEditor = new TextEditor
                {
                    text = textToCopy.text
                };
                textEditor.SelectAll();
                textEditor.Copy();
                print("Kopioitu");

            #endif
        }
        else
        {
            Debug.LogError("Teksti� ei voi kopioida, koska TextMeshProUGUI-viittaus puuttuu!");
        }
    }
}
