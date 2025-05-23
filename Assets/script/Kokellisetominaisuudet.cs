using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Kokellisetominaisuudet : MonoBehaviour
{
    public GameObject lisäys;
    public GameObject valikko;
    public TextMeshProUGUI ilmoitus;
    public TMP_InputField input;
    public string koodi;
    public GameObject teema_nappu;
    public string teema_otettu;
    public string resetointi_otettu;
    public GameObject resetointi_nappu;

    private void Start()
    {
        teema_otettu = PlayerPrefs.GetString("teema_tieto", "false").ToString();
        resetointi_otettu = PlayerPrefs.GetString("resetointi_otettu", "false").ToString();

        if ( teema_otettu == "true")
        {
            teema_nappu.SetActive(true);

        }
        if (teema_otettu == "false")
        {
            teema_nappu.SetActive(false);

        }
        if (resetointi_otettu == "true")
        {
            resetointi_nappu.SetActive(true);

        }
        if (resetointi_otettu == "false")
        {
            resetointi_nappu.SetActive(false);

        }


    }
    public void avaa_lisäys()
    {
        lisäys.SetActive(false);
        valikko.SetActive(true);
        ilmoitus.text = "Syötä koodi";

    }
    public void tarkista()
    {
        koodi = input.text.ToString();
        if(koodi == "1111")
        {
            if (teema_otettu == "true")
            {

                ilmoitus.text = "POISTETTU TEEMAN VAIHTO";
                teema_otettu = "false";
                PlayerPrefs.SetString("teema_tieto", teema_otettu);

                resetointi_nappu.SetActive(false);
                return;
            }
            if (teema_otettu == "false")
            {
                ilmoitus.text = "LISÄTTY TEEMAN VAIHTO";
                teema_otettu = "true";
                PlayerPrefs.SetString("teema_tieto", teema_otettu);
                resetointi_nappu.SetActive(true);
                return;

            }
            
            




        }
        if (koodi == "2222")
        {

            if (resetointi_otettu == "true")
            {

                ilmoitus.text = "POISTETTU KAIKEN RESETOINTI";
                resetointi_otettu = "false";
                PlayerPrefs.SetString("resetointi_otettu", resetointi_otettu);

                teema_nappu.SetActive(false);
                return;
            }
            if (resetointi_otettu == "false")
            {
                ilmoitus.text = "LISÄTTY KAIKEN RESETOINTI";
                resetointi_otettu = "true";
                PlayerPrefs.SetString("resetointi_otettu", resetointi_otettu);
                teema_nappu.SetActive(true);
                return;

            }



        }
        else
        {
            ilmoitus.text = "ANTAMASI KOODI EI OLE OLEMASSA";




        }



    }
    public void poistu()
    {

        valikko.SetActive(false );
        lisäys.SetActive(true );


    }
    public void resetoi()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();

        

    }
   
}
