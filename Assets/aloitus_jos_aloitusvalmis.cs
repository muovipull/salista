using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class aloitus_jos_aloitusvalmis : MonoBehaviour
{
    public int loppuaikas = 1200;
    public int loppuaikam = 20;
    public static int j‰ljell‰_olevat_yritykset = 5;
    public InputField salis;
    public GameObject vaara;
    public GameObject seuraaava;
    public GameObject jaa;
    public GameObject v‰‰r‰salasana;
    public GameObject tama;
    public GameObject uudelleen;
    public GameObject rangaistus;
    public TextMeshProUGUI text;
    public Text rangaistus_texsti;


    public void alku()
    
    {
        if (j‰ljell‰_olevat_yritykset <= 0)
        {

            j‰ljell‰_olevat_yritykset = 0;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            tama.SetActive(false);
            rangaistus.SetActive(true);

        }

    }


    public void eteenpain()
    {
        alku();
        if (j‰ljell‰_olevat_yritykset <= 0)
        {

            j‰ljell‰_olevat_yritykset = 0;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            tama.SetActive(false);
            rangaistus.SetActive(true);
        }
        else if (salasananvaihto.salistatlalla == salis.text)
        {
            seuraaava.SetActive(true);
            jaa.SetActive(false);
            vaara.SetActive(false);
            tama.SetActive(false);
            j‰ljell‰_olevat_yritykset = 5;
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
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
        alku();
        j‰ljell‰_olevat_yritykset=PlayerPrefs.GetInt("j‰ljell‰", 5);
        loppuaikam = PlayerPrefs.GetInt("loppuaikam", 20);
        loppuaikas = PlayerPrefs.GetInt("loppuaikas", 1200);
        print(j‰ljell‰_olevat_yritykset);
    }

    // Update is called once per frame
    void Update()
    {
        if (j‰ljell‰_olevat_yritykset <= 0)
        {

            j‰ljell‰_olevat_yritykset = 0;
            rangaistus_texsti.text = "olet yritt‰nyt liian monta kertaa v‰‰rin siksi joudut odottamaan\n" +loppuaikam+"\n minuuttia ennen kuin voin taas kirjautua";
            
            
        }

    }
    public void OnApplicationFocus(bool on)
    {
        if(on==false)
        {
            PlayerPrefs.SetInt("j‰ljell‰", j‰ljell‰_olevat_yritykset);
            PlayerPrefs.SetInt("loppuaikam", loppuaikam);
            PlayerPrefs.SetInt("¥loppuaikas", loppuaikas);

        }
    }
}