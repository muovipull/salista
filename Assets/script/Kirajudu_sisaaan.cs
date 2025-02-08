using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Kirajudu_sisaaan : MonoBehaviour
{
    [Header("Sisalla")]
    public GameObject paa_nautt;

    [Header("tallennus")]
    public GameObject tallennus;
    public TMP_InputField uusi1;
    public TMP_InputField uusi2;
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


        }
        if(tallennettu == 1)
        {
            kirajaudu_sivu.SetActive(true);

        }






    }
    public void tallenna()
    {
        if(uusi1.text.ToString()== uusi2.text.ToString() && uusi1.text.ToString() != "" && uusi2.text.ToString() != "")
        {
            tallennettu = 1;
            PlayerPrefs.SetInt("tallennettu", tallennettu);
            salasananvaihto.salistatlalla = uusi1.text;
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
        if (uusi1.text.ToString()=="" || uusi2.text.ToString()=="")
        {

            uusi1.text = "";
            uusi2.text = "";
            valitus_tallennus.text = "kaikki kohdat pit‰‰ t‰ytt‰‰";


        }



    }
    public void kirjaudu()
    {
        if (salasana.text.ToString() != salasananvaihto.salistatlalla)
        {
            salasana.text = "";
            valitus_kirja.text = "Salasana ei ole oikea";



        }
        if(salasana.text.ToString()== salasananvaihto.salistatlalla && Kaksi_vaiheinen_tunnistus.otettu == "false")
        {
            paa_nautt.SetActive (true);
            kirajaudu_sivu.SetActive(false);
        }
        if (salasana.text.ToString() == salasananvaihto.salistatlalla && Kaksi_vaiheinen_tunnistus.otettu == "true")
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
