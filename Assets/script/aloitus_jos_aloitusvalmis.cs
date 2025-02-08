using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;

public class aloitus_jos_aloitusvalmis : MonoBehaviour
{
    
    public static int j‰ljell‰_olevat_yritykset = 5;
    public static int kello = 0;
    [Header("omat")]
    public InputField salis;
    public GameObject vaara;
    public GameObject seuraaava;
    public GameObject jaa;
    public GameObject v‰‰r‰salasana;
    public GameObject tama;
    public GameObject uudelleen;
    public GameObject rangaistus;
    public TextMeshProUGUI text;
    public GameObject normi;

   
    public void ohita()
    {

        normi.SetActive(true);
        seuraaava.SetActive(false);
        jaa.SetActive(false);
        vaara.SetActive(false);
        tama.SetActive(false);

    }
    public void alku()
    
    {
        if (j‰ljell‰_olevat_yritykset <= 0)
        {
            kello = 1;
            j‰ljell‰_olevat_yritykset = 0;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            tama.SetActive(false);
            rangaistus.SetActive(true);

        }

    }


    public void eteenpain()
    {
        alku();
        if (salasananvaihto.salistatlalla == salis.text && Kaksi_vaiheinen_tunnistus.otettu == "true")
        {

            seuraaava.SetActive(true);
            jaa.SetActive(false);
            vaara.SetActive(false);
            tama.SetActive(false);
            j‰ljell‰_olevat_yritykset = 5;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
        }
        if (salasananvaihto.salistatlalla == salis.text && Kaksi_vaiheinen_tunnistus.otettu=="false")
        {
            normi.SetActive(true);
            seuraaava.SetActive(false);
            jaa.SetActive(false);
            vaara.SetActive(false);
            tama.SetActive(false);
            j‰ljell‰_olevat_yritykset = 5;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);


        }




        else if (j‰ljell‰_olevat_yritykset <= 0)
        {
            print("toimmiko");
            j‰ljell‰_olevat_yritykset = 0;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            tama.SetActive(false);
            rangaistus.SetActive(true);
            
            



        }
        
        else
        {
            j‰ljell‰_olevat_yritykset = j‰ljell‰_olevat_yritykset - 1;
            salis.text = "";

            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);

            text.text = "yrityksi‰ j‰ljell‰ " + j‰ljell‰_olevat_yritykset + " kappaletta";
            jaa.SetActive(true);
            v‰‰r‰salasana.SetActive(true);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uudelleen.SetActive(false);
        
        j‰ljell‰_olevat_yritykset =PlayerPrefs.GetInt("j‰ljell‰", 5);
        kello = PlayerPrefs.GetInt("ke", 0);
        alku();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            eteenpain();
        }








    }
    public void OnApplicationFocus(bool on)
    {
        if(on==false)
        {
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            PlayerPrefs.SetInt("ke", kello);
            



        }
    }
}