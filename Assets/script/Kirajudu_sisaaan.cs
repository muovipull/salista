using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Kirajudu_sisaaan : MonoBehaviour
{
    [Header("Sisalla")]
    public GameObject paa_nautt;
    [Header("vara pin")]




    [Header("tallennus")]
    public GameObject tallennus;
    public TMP_InputField uusi1;
    public TMP_InputField uusi2;
    public TMP_InputField vara_uusi3;
    public TextMeshProUGUI valitus_tallennus;

    [Header("Kirjaudu")]
    public TMP_InputField salasana;
    public GameObject kirajaudu_sivu;
    public TextMeshProUGUI valitus_kirja;

    [Header("kaksi")]
    public GameObject kaksi_vaihe_kirja;

    [Header("muuttujat")]
    public int tallennettu;

    public void katso()
    {
        if(tallennettu == 0)
        {
            tallennus.SetActive(true);
            valitus_tallennus.text = "Pinkoodi mit‰ tulet k‰ytt‰m‰‰n sovellukseen kirjautumiseen";

        }
        if(tallennettu == 1)
        {
            kirajaudu_sivu.SetActive(true);

        }






    }
    public void tallenna()
    {
        if(uusi1.text.ToString() == uusi2.text.ToString() && uusi1.text.ToString() != "" && uusi2.text.ToString() != "" && vara_uusi3.text.ToString() != "")
        {
            tallennettu = 1;
            PlayerPrefs.SetInt("tallennettu", tallennettu);
            salasananvaihto.salistatlalla = uusi1.text;
            salasananvaihto.varasalistalla = vara_uusi3.text;
            PlayerPrefs.SetString("varasala", salasananvaihto.varasalistalla);
            PlayerPrefs.SetString("Tallennus1", salasananvaihto.salistatlalla);
            paa_nautt.SetActive(true);
            tallennus.SetActive(false);


        }
        if (uusi1.text.ToString() != uusi2.text.ToString())
        {
            uusi1.text = "";
            uusi2.text = "";
            valitus_tallennus.text = "salasanojen pit‰‰ olla yhten‰iset";

        }
        if (uusi1.text.ToString()=="" || uusi2.text.ToString()=="" || vara_uusi3.text.ToString() == "")
        {

            uusi1.text = "";
            uusi2.text = "";
            vara_uusi3.text = "";
            valitus_tallennus.text = "kaikki kohdat pit‰‰ t‰ytt‰‰";


        }



    }
    public void kirjaudu()
    {
        if (salasana.text.ToString() != salasananvaihto.salistatlalla && salasana.text.ToString() != salasananvaihto.varasalistalla)
        {
            salasana.text = "";
            valitus_kirja.text = "Salasana ei ole oikea";



        }
        if(((salasana.text.ToString() == salasananvaihto.salistatlalla) || (salasana.text.ToString() == salasananvaihto.varasalistalla)) && Kaksi_vaiheinen_tunnistus.otettu == "false")
        {
            paa_nautt.SetActive (true);
            kirajaudu_sivu.SetActive(false);
        }
        if (((salasana.text.ToString() == salasananvaihto.salistatlalla) || (salasana.text.ToString() == salasananvaihto.varasalistalla)) && Kaksi_vaiheinen_tunnistus.otettu == "true")
        {
            kaksi_vaihe_kirja.SetActive(true);
            kirajaudu_sivu.SetActive(false);
        }





    }
    public void ohita()
    {
        paa_nautt.SetActive (true);
        kirajaudu_sivu.SetActive (false);
        kaksi_vaihe_kirja.SetActive (false);


    }
    void Start()
    {
        tallennettu = PlayerPrefs.GetInt("tallennettu", 0);
        salasananvaihto.salistatlalla = PlayerPrefs.GetString("Tallennus1", "1234");
        katso();
        paa_nautt.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            kirjaudu();
            tallenna();
        }
    }
   
}
