using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class salasananvaihto : MonoBehaviour
{
    public GameObject no;
    public GameObject alku;
    public GameObject alkuperainen;
    public GameObject uusi;
    public GameObject vaihdettu_text;
    public GameObject eroavatsalasanant_text;
    public GameObject tyhja;
    public GameObject vaarasalis;
    public InputField aikaisempi;
    public InputField uusi1;

    public InputField uusi2;
    public static string salistatlalla;

    public void takaisin()
    {
        no.SetActive(false);
        alku.SetActive(true);
    }
    public void taka()
    {
        no.SetActive(true);
        alkuperainen.SetActive(false);





    }
    public void takava()
    {
        uusi.SetActive(false);
        alkuperainen.SetActive(true);





    }
    public void vaihdasalis()
    {
        no.SetActive(false);

        alkuperainen.SetActive(true);

    }
    public void uusisalis()
    {
        if (aikaisempi.text == salasananvaihto.salistatlalla)
        {
            aikaisempi.text = "";
            alkuperainen.SetActive(false);
            print("toimii");
            uusi.SetActive(true);
            vaarasalis.SetActive(false);

        }
        else
        {
            vaarasalis.SetActive(true);
            aikaisempi.text =  "";
            print(salasananvaihto.salistatlalla);
            print(aikaisempi.text);
        }

    }
    public void uusisalista()
    {
        
        //or ||
        // and &&
        
        if(uusi1.text == "" || uusi2.text == "")
        {
            tyhja.SetActive(true);

        
        
        
        
        }
        else
        {

            if (uusi1.text == uusi2.text)
            {
                print("toimii");
                no.SetActive(true);
                vaihdettu_text.SetActive(true);
                eroavatsalasanant_text.SetActive(false);
                uusi.SetActive(false);
                tyhja.SetActive(false);
                salasananvaihto.salistatlalla = uusi1.text;
                uusi1.text = "";
                uusi2.text = "";



                PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);
                print(salasananvaihto.salistatlalla);

            }

            else
            {
                eroavatsalasanant_text.SetActive(true);
                uusi2.text = "";
                uusi1.text = "";

            }
        }





    }


    // Start is called before the first frame update
    void Start()
    {
        salasananvaihto.salistatlalla = PlayerPrefs.GetString("Tallennus1", "1234");
        print(salasananvaihto.salistatlalla);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
