using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class salasananvaihto : MonoBehaviour
{
    public GameObject Asetusvalikko;
    public GameObject alku;
    public GameObject alkuperainen;
    public GameObject uusi;
    public GameObject vaihdettu_text;

    public GameObject vaarasalis;
    public TMP_InputField aikaisempi;
    public TMP_InputField uusi1;
    public TMP_InputField uusi2;
    public TMP_InputField kaksi_vaihe_input;

    public TextMeshProUGUI vali1_text;
    public TextMeshProUGUI vali2_text;
    public GameObject vali1;
    public GameObject vali2;
    public GameObject kaksi_gameobject;

    public GameObject varapin_sivu;
    public TMP_InputField varapin_uusi;
    public TextMeshProUGUI varapin_vaihto_ilmoitus;
    

    public static string salistatlalla;
    public static string varasalistalla;

    public void takaisin_paa()
    {
        Asetusvalikko.SetActive(false);
        alku.SetActive(true);
    }
    public void takaisin_asetukset()
    {
        Asetusvalikko.SetActive(true);
        alkuperainen.SetActive(false);


    }

    public void vaihdapinkoodi_laita_valikkoon()
    {
        Kaksi_vaiheinen_tunnistus.otettu = PlayerPrefs.GetString("true", "false");
        Asetusvalikko.SetActive(false);
        alkuperainen.SetActive(true);
        if (Kaksi_vaiheinen_tunnistus.otettu == "true")
        {
            kaksi_gameobject.SetActive(true);



        }
        else
        {
            kaksi_gameobject.SetActive(false);
        }
        

    }
    public void uusisalis()
    {
        string secretKey = Kaksi_vaiheinen_tunnistus.secret; // Esimerkkisalaus Base32-muodossa
        string sala_kaksi = TOTP.GenerateTOTP(secretKey);

        
        if ((aikaisempi.text == salasananvaihto.salistatlalla && Kaksi_vaiheinen_tunnistus.otettu == "true" && kaksi_vaihe_input.text==sala_kaksi.ToString()) || (aikaisempi.text == salasananvaihto.salistatlalla && Kaksi_vaiheinen_tunnistus.otettu == "true" && kaksi_vaihe_input.text == Kaksi_vaiheinen_tunnistus.vara) || (aikaisempi.text == salasananvaihto.salistatlalla && Kaksi_vaiheinen_tunnistus.otettu == "false"))
        {
            vali1_text.text = "";
            vali2_text.text = "";
            aikaisempi.text = "";
            alkuperainen.SetActive(false);
            print("toimii");
            uusi.SetActive(true);
            vaarasalis.SetActive(true);

        }
        else
        {

            vaarasalis.SetActive(true);
            aikaisempi.text =  "";
        }

    }
    public void uusisalista()
    {
        
        //or ||
        // and &&

        if (uusi1.text == "")
        {
            vali1_text.text = "kohta ei saa j‰‰d‰ tyhj‰ksi";
            vali1.SetActive(true);

        
        
        
        
        }
        if (uusi2.text == "")
        {
            vali2_text.text = "kohta ei saa j‰‰d‰ tyhj‰ksi";
            vali2.SetActive(true);





        }
        else
        {

            if (uusi1.text == uusi2.text)
            {
                print("toimii");
                Asetusvalikko.SetActive(true);
                vaihdettu_text.SetActive(true);

                uusi.SetActive(false);
                salasananvaihto.salistatlalla = uusi1.text;
                uusi1.text = "";
                uusi2.text = "";
                vali1_text.text = "";
                vali2_text.text = "";

                kaksi_gameobject.SetActive(false);

                PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);
                print(salasananvaihto.salistatlalla);

            }

            else
            {
                vali2_text.text = "salasanat pit‰‰ ola yhtein‰iset";
                vali1_text.text = "salasanat pit‰‰ ola yhtein‰iset";

                uusi2.text = "";
                uusi1.text = "";

            }
        }





    }
    public void avaa_varapinvaihto()
    {
        Asetusvalikko.SetActive(false);
        varapin_sivu.SetActive(true);
        varapin_vaihto_ilmoitus.text = "";

    }
    public void aseta_varapin()
    {
        salasananvaihto.varasalistalla = varapin_uusi.text.ToString();
        varapin_vaihto_ilmoitus.text = "vara pinkoodi vaihdettu";
        PlayerPrefs.SetString("varasala", salasananvaihto.varasalistalla);
    }
    public void poistu_varapinvaihto()
    {

        Asetusvalikko.SetActive(true);
        varapin_sivu.SetActive(false);
        varapin_vaihto_ilmoitus.text = "";

    }

    // Start is called before the first frame update
    void Start()
    {
        salasananvaihto.salistatlalla = PlayerPrefs.GetString("Tallennus1", "1234");
        print(salasananvaihto.salistatlalla);
        salasananvaihto.varasalistalla = PlayerPrefs.GetString("varasala", "");
        print(salasananvaihto.varasalistalla);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
