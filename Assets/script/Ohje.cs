using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Ohje : MonoBehaviour
{
    public GameObject ohje_sivu;
    public GameObject ohje_avaus_nappi;
    public TextMeshProUGUI sivu_leipa;
    public TextMeshProUGUI sivu_tekst;
    public int sivu;
    public GameObject seura_napppi;
    public GameObject edelli_nappi;
    [Serializable]
    public class sivudata
    {
        public string otsikko;
        [TextArea(1, 15)]
        public string tesksti;
    }

    public List<sivudata> sivut;

    public void avaa()
    {
        ohje_sivu.SetActive(true);
        ohje_avaus_nappi.SetActive(false);
        sivu = 0;
        paivita_Sivu();

    }
    public void sulje()
    {
        ohje_sivu.gameObject.SetActive(false);
        ohje_avaus_nappi.SetActive(true);

    }

    public void seuraava()
    {
        sivu++;
        paivita_Sivu();
    }

    public void edllinen()
    {
        sivu--;
        paivita_Sivu();


    }

    public void paivita_Sivu()
    {
        sivu_tekst.text = "sivu " + (sivu + 1) + " " + sivut[sivu].otsikko;
        sivu_leipa.text = sivut[sivu].tesksti;



        seura_napppi.SetActive(sivu < sivut.Count - 1);
        edelli_nappi.SetActive(sivu > 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
