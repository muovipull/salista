using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class alku : MonoBehaviour
{
    [Header("input")]
    public InputField uusi1;
    public InputField uusi2;
    [Header("gameobject")]
    public GameObject ero;
    public GameObject tyhja;
    public GameObject a1;
    public GameObject jos_laitettu;
    public GameObject salista;
    public static string onko = "1";
    //public aloitus_jos_aloitusvalmis aloitus_Jos_Aloitusvalmis;

    public void seuraava()
    {
        tyhja.SetActive(false);
        ero.SetActive(false);

        if (uusi2.text == "" || uusi1.text == "")
        {
            tyhja.SetActive(true);


        }
        else
        {
            if (uusi1.text == uusi2.text)
            {

                tyhja.SetActive(false);
                ero.SetActive(false);

                a1.SetActive(false);


                salista.SetActive(true);
                //alku.onko = "2";
                alku.onko = PlayerPrefs.GetString("Tallennus", "3");
                salasananvaihto.salistatlalla = uusi1.text;




                PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);
                alku.onko = "2";
                PlayerPrefs.SetString("Tallennus", alku.onko);
                //print(alku.onko);



            }
            else
            {
                ero.SetActive(true);

            }

        }



    }

    public void aha()
    {
        print(onko + "  " + "onko");
        alku.onko = PlayerPrefs.GetString("Tallennus", "1");
        print(salasananvaihto.salistatlalla);
        if (alku.onko == "1")
        {
            
            a1.SetActive(true);
            
            PlayerPrefs.SetString("Tallennus", alku.onko);
            
        }

        else
        {
            if (aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset == 0)
            {
                //aloitus_Jos_Aloitusvalmis.alku();

            }
            else
            {
                jos_laitettu.SetActive(true);
                tyhja.SetActive(false);
                ero.SetActive(false);
                a1.SetActive(false);

            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        aha();
        print(alku.onko + " onko");



    }

    // Update is called once per frame
    #if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))

        {
            PlayerPrefs.DeleteAll();
            print(alku.onko);
            print(salasananvaihto.salistatlalla);
            print("tyhjä");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print(aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset);
            aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset += 5;

        }
    }
    #endif
}
