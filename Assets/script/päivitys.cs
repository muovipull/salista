using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;
using System;

public class päivitys : MonoBehaviour
{
    public int avauskerta=0;
    public GameObject päivitykset;
    public TextMeshProUGUI versio_text;
    public string versio;
    public int sivu;
    public GameObject seura_napppi;
    public GameObject edelli_nappi;
    public GameObject sivu_1;
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
        




    // Start is called before the first frame update
    void Start()
    {


        

        Application.targetFrameRate = 30;
        


        versio = PlayerPrefs.GetString("version", Application.version);
        versio_text.text = $"V {Application.version}";
        avauskerta = PlayerPrefs.GetInt("avauskerta", 0);
        if (avauskerta == 1 )
        {
            avaa();
            avauskerta=2;
            PlayerPrefs.SetInt("avauskerta", avauskerta);

        }
        if (avauskerta == 0)
        {
            print("avatuu kerta " + avauskerta);
            avauskerta = 1;
            PlayerPrefs.SetInt("avauskerta", avauskerta);
        }
        if (versio != Application.version && avauskerta == 2)
        {
            avaa();


        }
        

        

    }
    
    public void avaa()
    {

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
        sivu++;
        paivita_Sivu();
        
        




    }
    public void paivita_Sivu()
    {
        sivu_tekst.text = "sivu " + (sivu+1) + " " + sivut[sivu].otsikko;
        sivu_leipa.text = sivut[sivu].tesksti;
        
        
        
        seura_napppi.SetActive(sivu<sivut.Count-1);
        edelli_nappi.SetActive(sivu>0);
    }
    
    public void edllinen()
    {
        sivu--;
        paivita_Sivu();
        

    }
}
