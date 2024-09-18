using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;    
using UnityEngine.UI;

public class Luo_random : MonoBehaviour
{
    public string salasana = "";
    public TMP_InputField pituus;
    public int pituusint =10;
    public GameObject satuinnais;
    public GameObject paaruutu;
    public TMP_Text n�ytt�salasana;
    public int minCharAmount = 10;
    public int maxCharAmount = 30;
    public string salasanarandom = "";
    public char[] merkit = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' };

    public void peruuta()
    {
        print("peruutettu");
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
        TextEditor textEditor = new TextEditor();
        textEditor.text = n�ytt�salasana.text;
        textEditor.SelectAll();
        textEditor.Copy();
        print("kopioitu");


    }
    string luo_satunnainen()
    {
        salasanarandom = "";
        int maara = UnityEngine.Random.Range(minCharAmount, maxCharAmount + 1);

        for (int i = 0; i < maara; i++)
        {
            salasanarandom += merkit[UnityEngine.Random.Range(0, merkit.Length)];
        }

        return salasanarandom;
    }
    public void luo()
    {
        try
        {
            pituusint = int.Parse(pituus.text);
            maxCharAmount = pituusint;
            minCharAmount = pituusint;
            salasanarandom = luo_satunnainen();
            Debug.Log("Satunnainen merkkijono: " + salasanarandom);
            n�ytt�salasana.text = salasanarandom;
        }
        catch
        {
            print("ei onnistunut");
            print(pituus.text);




        }




    }

      
    
}
    
    



