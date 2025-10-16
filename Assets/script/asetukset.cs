using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class asetukset : MonoBehaviour
{
    public int sivu;


    public GameObject Kosmetiikka; //sivu 2
    public GameObject kirajutuminen; //sivu 1
    public GameObject muut;   //sivu 3
    public GameObject Käyttäjä; //sivu 4

    public GameObject asetus_sivu;
    public GameObject paa_nautto;
    public GameObject vaihdettu_ilmoitus;



    public void paivita_Sivu()
    {
        if(sivu == 1)
        {
            kirajutuminen.SetActive(true);
            muut.SetActive(false);
            Kosmetiikka.SetActive(false);
            Käyttäjä.SetActive(false);


        }
        if (sivu == 2)
        {
            Kosmetiikka.SetActive(true);
            muut.SetActive(false);
            kirajutuminen.SetActive(false);
            Käyttäjä.SetActive(false);



        }
        if(sivu == 3)
        {
            muut.SetActive(true);
            Kosmetiikka.SetActive(false);
            kirajutuminen.SetActive(false);
            Käyttäjä.SetActive(false);
        }

        if(sivu == 4)
        {
            Käyttäjä.SetActive(true);
            Kosmetiikka.SetActive(false);
            kirajutuminen.SetActive(false);
            muut.SetActive(false);
        }




    }
    public void avaa()
    {
        asetus_sivu.SetActive(true);
        paa_nautto.SetActive(false);
        sivu = 1;
        paivita_Sivu();

    }
    public void sulje()
    { 
        asetus_sivu.SetActive(false);
        paa_nautto.SetActive(true);
        vaihdettu_ilmoitus.SetActive(false);
        

    }

    public void kosme()
    {
        sivu=2;
        paivita_Sivu();

    }

    public void kirja()
    {
        sivu=1;
        paivita_Sivu();


    }
    public void muu()
    {
        sivu=3;
        paivita_Sivu();


    }
    public void kayttaja()
    {
        sivu=4;
        paivita_Sivu();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
