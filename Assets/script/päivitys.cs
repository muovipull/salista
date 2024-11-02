using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject sivu_2;
    public TextMeshProUGUI sivu_tekst;

    // Start is called before the first frame update
    void Start()
    {


        sivu_1.SetActive(true);
        sivu_2.SetActive(false);
        seura_napppi.SetActive(true);
        edelli_nappi.SetActive(false);
        sivu_tekst.text = "sivu " + sivu;
        sivu = 1;

        Application.targetFrameRate = 30;
        ScreenCapture.CaptureScreenshot("kuva.png");


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
            versio = Application.version;
            PlayerPrefs.SetString("version", versio);


        }

        

    }

    public void avaa()
    {

        sivu_1.SetActive(true);
        sivu_2.SetActive(false);
        seura_napppi.SetActive(true);
        edelli_nappi.SetActive(false);
        sivu_tekst.text = "sivu " + sivu;
        sivu = 1;

        päivitykset.SetActive(true);



    }
    public void sulje()
    {
        päivitykset.SetActive(false);
        PlayerPrefs.SetInt("avauskerta", avauskerta);



    }
    public void seuraava()
    {
        if (sivu == 1)
        {
            sivu_2.SetActive(true);
            sivu_1.SetActive(false);
            edelli_nappi.SetActive(true) ;
            seura_napppi.SetActive(false);
            
            sivu = 2;

            sivu_tekst.text = "sivu " + sivu + " versiot 2.5.1-2.5.3";
        }



    }
    public void edllinen()
    {
        sivu_1.SetActive(true);
        sivu_2.SetActive(false);
        seura_napppi.SetActive(true);
        edelli_nappi.SetActive(false);
        sivu = 1;
        sivu_tekst.text = "sivu " + sivu + " versiot 2.6-2.6.4";

    }
}
