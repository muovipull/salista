using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class kopioi : MonoBehaviour
{
    public TMP_Text kopio;


    public void kopi()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = kopio.text;
        textEditor.SelectAll();
        textEditor.Copy();
        print("kopioitu");


    }
}
