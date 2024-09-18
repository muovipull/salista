using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kokellisetominaisuudet : MonoBehaviour
{
    public GameObject varmistus_resetointi;
    public GameObject varmistus_koe_käyttö;
    public GameObject koe_poisto;
    public GameObject resetoi_nappi;
    public int koe = 0;
    // Start is called before the first frame update
    void Start()
    {
        koe=PlayerPrefs.GetInt("koe", 0);
        if(koe == 1 )
        {
            resetoi_nappi.SetActive(true);


        }
    }
    public void resetoi_ennen()
    {
        varmistus_resetointi.SetActive(true);

    }
    public void en()
    {
        varmistus_resetointi.SetActive(false);
        varmistus_koe_käyttö.SetActive(false);
        koe_poisto.SetActive(false);
    
    }
    public void resetoi()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();



    }
    public void varmistus_koe()
    {
        varmistus_koe_käyttö.gameObject.SetActive(true);



    }
    public void otakäyttöön()
    {
        if (koe == 0)
        {
            resetoi_nappi.gameObject.SetActive(true);
            koe = 1;
            PlayerPrefs.SetInt("koe", koe);
            varmistus_koe_käyttö.gameObject.SetActive(false);
        }
        else
        {
            koe_poisto.SetActive(true);
        }
    }
    public void poista_koe()
    {


            resetoi_nappi.gameObject.SetActive(false);
            koe = 0;
            PlayerPrefs.SetInt("koe", koe);
            varmistus_koe_käyttö.gameObject.SetActive(false);
            koe_poisto.SetActive(false) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
