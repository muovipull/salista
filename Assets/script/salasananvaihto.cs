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
        vaarasalis.SetActive(false);
        aikaisempi.text = "";
        kaksi_vaihe_input.text = "";
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

        aikaisempi.text = "";
        kaksi_vaihe_input.text = "";
        vaarasalis.SetActive(false);
    }

    public void uusisalis()
    {
        string secretKey = Kaksi_vaiheinen_tunnistus.secret;
        string currentTOTP = TOTP.GenerateTOTP(secretKey);

        bool oldPinMatches = (aikaisempi.text == salasananvaihto.salistatlalla);

        bool kaksiVaiheinenKaytossa = (Kaksi_vaiheinen_tunnistus.otettu == "true");

        bool twoFactorCodeMatches = (kaksi_vaihe_input.text == currentTOTP || kaksi_vaihe_input.text == Kaksi_vaiheinen_tunnistus.vara);

        bool authenticated = false;

        if (oldPinMatches)
        {
            if (kaksiVaiheinenKaytossa)
            {
                if (twoFactorCodeMatches)
                {
                    authenticated = true;
                }
            }
            else
            {
                authenticated = true;
            }
        }

        if (authenticated)
        {
            vali1_text.text = "";
            vali2_text.text = "";
            aikaisempi.text = "";
            kaksi_vaihe_input.text = "";
            vaarasalis.SetActive(false);

            alkuperainen.SetActive(false);
            uusi.SetActive(true);
        }
        else
        {
            vaarasalis.SetActive(true);
            aikaisempi.text = "";
            kaksi_vaihe_input.text = "";
        }
    }

    public void uusisalista()
    {
        vali1_text.text = "";
        vali2_text.text = "";
        vali1.SetActive(false);
        vali2.SetActive(false);

        bool uusi1Tyhja = string.IsNullOrEmpty(uusi1.text);
        bool uusi2Tyhja = string.IsNullOrEmpty(uusi2.text);

        if (uusi1Tyhja)
        {
            vali1_text.text = "kohta ei saa j‰‰d‰ tyhj‰ksi";
            vali1.SetActive(true);
        }
        if (uusi2Tyhja)
        {
            vali2_text.text = "kohta ei saa j‰‰d‰ tyhj‰ksi";
            vali2.SetActive(true);
        }

        if (uusi1Tyhja || uusi2Tyhja)
        {
            return;
        }

        if (uusi1.text == uusi2.text)
        {
            Asetusvalikko.SetActive(true);
            vaihdettu_text.SetActive(true);
            uusi.SetActive(false);

            salasananvaihto.salistatlalla = uusi1.text;
            PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);

            uusi1.text = "";
            uusi2.text = "";

            kaksi_gameobject.SetActive(false);
        }
        else
        {
            vali2_text.text = "salasanat pit‰‰ olla yhtein‰iset";
            vali1_text.text = "salasanat pit‰‰ olla yhtein‰iset";
            vali1.SetActive(true);
            vali2.SetActive(true);
            uusi2.text = "";
            uusi1.text = "";
        }
    }

    public void avaa_varapinvaihto()
    {
        Asetusvalikko.SetActive(false);
        varapin_sivu.SetActive(true);
        varapin_vaihto_ilmoitus.text = "";
        varapin_uusi.text = "";
    }

    public void aseta_varapin()
    {
        salasananvaihto.varasalistalla = varapin_uusi.text;
        PlayerPrefs.SetString("varasala", salasananvaihto.varasalistalla);
        varapin_vaihto_ilmoitus.text = "vara pinkoodi vaihdettu";
        varapin_uusi.text = "";
    }

    public void poistu_varapinvaihto()
    {
        Asetusvalikko.SetActive(true);
        varapin_sivu.SetActive(false);
        varapin_vaihto_ilmoitus.text = "";
    }

    void Start()
    {
        salasananvaihto.salistatlalla = PlayerPrefs.GetString("Tallennus1", "1234");

        salasananvaihto.varasalistalla = PlayerPrefs.GetString("varasala", "");

        if (vaihdettu_text != null) vaihdettu_text.SetActive(false);
        if (vaarasalis != null) vaarasalis.SetActive(false);
        if (vali1 != null) vali1.SetActive(false);
        if (vali2 != null) vali2.SetActive(false);
        if (varapin_vaihto_ilmoitus != null) varapin_vaihto_ilmoitus.text = "";
    }

    void Update()
    {

    }
}