using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class päivitys : MonoBehaviour
{
    public int avauskerta = 0;
    public GameObject päivitykset;
    public TextMeshProUGUI versio_text;
    public string versio;
    public int sivu;
    public GameObject seura_napppi;
    public GameObject edelli_nappi;

    public TextMeshProUGUI sivu_leipa;
    public TextMeshProUGUI sivu_tekst;
    public GameObject haly;

    [Serializable]
    public class sivudata
    {
        public string otsikko;
        [TextArea(1, 15)]
        public string tesksti;
    }

    public List<sivudata> sivut;

    void Start()
    {
        versio = PlayerPrefs.GetString("version", Application.version);
        versio_text.text = $"V {Application.version}";
        avauskerta = PlayerPrefs.GetInt("avauskerta", 0);

        if (avauskerta == 1)
        {
            avaa();
            avauskerta = 2;
            PlayerPrefs.SetInt("avauskerta", avauskerta);
        }
        else if (avauskerta == 0)
        {
            Debug.Log("avatuu kerta " + avauskerta);
            avauskerta = 1;
            PlayerPrefs.SetInt("avauskerta", avauskerta);
        }

        // Tarkistetaan onko versio muuttunut
        if (versio != Application.version && avauskerta == 2)
        {
            avaa();
            // Päivitetään tallennettu versio, jotta tämä ei toistu samalla versiolla
            PlayerPrefs.SetString("version", Application.version);
        }
    }

    public void avaa()
    {
        // --- KORJAUS ALKAA ---
        // Tarkistetaan, onko LadattuOtsikko tyhjä tai onko se jo lisätty listaan
        if (string.IsNullOrEmpty(Päälataus.LadattuOtsikko))
        {
            Debug.LogWarning("Otsikko on tyhjä, ei lisätä sivua.");
            return;
        }

        // Estetään tuplakopiot: etsitään löytyykö otsikko jo listasta
        bool onJoListalla = sivut.Exists(s => s.otsikko == Päälataus.LadattuOtsikko);

        if (!onJoListalla)
        {
            // Lisätään vain jos ei ole jo olemassa
            sivudata uusiSivu = new sivudata
            {
                otsikko = Päälataus.LadattuOtsikko,
                tesksti = Päälataus.LadattuTeksti
            };

            // Lisätään uusin päivitys listan alkuun (indeksiin 0)
            sivut.Insert(0, uusiSivu);
            Debug.Log("Uusi päivitys sivu lisätty listan alkuun.");
        }
        else
        {
            Debug.Log("Sivu on jo listalla, ei luoda kopiota.");
        }
        // --- KORJAUS PÄÄTTYY ---

        päivitykset.SetActive(true);
        sivu = 0;
        paivita_Sivu();
    }

    public void sulje()
    {
        päivitykset.SetActive(false);
        PlayerPrefs.SetInt("avauskerta", avauskerta);
    }

    public void seuraava()
    {
        if (sivu < sivut.Count - 1)
        {
            sivu++;
            paivita_Sivu();
        }
    }

    public void edllinen()
    {
        if (sivu > 0)
        {
            sivu--;
            paivita_Sivu();
        }
    }

    public void paivita_Sivu()
    {
        if (sivut.Count > 0)
        {
            sivu_tekst.text = "sivu " + (sivu + 1) + " " + sivut[sivu].otsikko;
            sivu_leipa.text = sivut[sivu].tesksti;
        }

        seura_napppi.SetActive(sivu < sivut.Count - 1);
        edelli_nappi.SetActive(sivu > 0);
    }
}