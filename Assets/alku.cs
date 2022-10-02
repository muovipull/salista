using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class alku : MonoBehaviour
{
    public InputField uusi1;
    public InputField uusi2;

    public GameObject ero;
    public GameObject tyhja;
    public GameObject a1;
    public GameObject jos_laitettu;
    public GameObject salista;
    public static string onko = "1";
    public aloitus_jos_aloitusvalmis aloitus_Jos_Aloitusvalmis;

    public void seuraava()
    {
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
                alku.onko = "2";
                alku.onko = PlayerPrefs.GetString("Tallennus", "3");
                salasananvaihto.salistatlalla = uusi1.text;




                PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);
                print(alku.onko);



            }
            else
            {
                ero.SetActive(true);

            }

        }



    }



    // Start is called before the first frame update
    void Start()
    {

        print(onko + "onko");
        alku.onko = PlayerPrefs.GetString("Tallennus", "1");
        print(salasananvaihto.salistatlalla);
        print(alku.onko);
        if (alku.onko == "1")
        {
            alku.onko = "2";
            a1.SetActive(true);
            PlayerPrefs.SetString("Tallennus", alku.onko);
        }

        else
        {
            if (aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset == 0)
            {
                aloitus_Jos_Aloitusvalmis.alku();

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.DeleteAll();
            print(alku.onko);
            print(salasananvaihto.salistatlalla);
            print("tyhjä");
        }
    }
}
