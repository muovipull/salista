using System;
using TMPro;
using UnityEngine;

public class Luo_random : MonoBehaviour
{
    public TMP_InputField pituus;
    public TMP_Text n�ytt�salasana;
    public GameObject satuinnais;
    public GameObject paaruutu;

    private string salasanarandom = "";
    private char[] merkit = {
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l','m', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    

    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',


    '1', '2', '3', '4', '5', '6', '7', '8', '9', '0','!'
    };

    public void peruuta()
    {
        print("Peruutettu");
        satuinnais.SetActive(false);
        paaruutu.SetActive(true);
    }

    public void poistuttu_alkun�yt�sta()
    {
        satuinnais.SetActive(true);
        paaruutu.SetActive(false);
        n�ytt�salasana.text = "";
        pituus.Select();
        pituus.text = "";
    }

    public void kopioi()
    {
        TextEditor textEditor = new TextEditor
        {
            text = n�ytt�salasana.text
        };
        textEditor.SelectAll();
        textEditor.Copy();
        print("Kopioitu");
    }

    string luo_satunnainen(int maara)
    {
        salasanarandom = "";
        for (int i = 0; i < maara; i++)
        {
            salasanarandom += merkit[UnityEngine.Random.Range(0, merkit.Length)];
        }
        return salasanarandom;
    }

    public void luo()
    {
        int pituusint;

        if (int.TryParse(pituus.text, out pituusint))
        {
            if (pituusint < 1 || pituusint > 50)
            {
                print("Sy�t� pituus v�lilt� 1�50");
                return;
            }

            salasanarandom = luo_satunnainen(pituusint);
            Debug.Log("Satunnainen salasana: " + salasanarandom);
            n�ytt�salasana.text = salasanarandom;
        }
        else
        {
            print("Sy�t� kelvollinen numero.");
        }
    }
}
