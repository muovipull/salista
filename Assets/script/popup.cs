using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class popup : MonoBehaviour
{

    public GameObject pop;
    private int auki=0;
    [Header("2_nappi")]
    public TMP_Text salasanana_näyttö;
    public GameObject salasanateksti;
    public GameObject näytä;

    
    

    // Update is called once per frame
    void Update()
    {
        
    }
    void Start()
    {

    }
    public void avaa()
    {
        if (auki == 0)
        {
            pop.SetActive(true);
            auki = 1;
            avaa_oikea_sivu();
        }
        else
        {
            pop.SetActive(false);
            auki = 0;
            sivu = 1;
        }
    }
    public void sulje()
    {
        pop.SetActive(false);
        auki = 0;
    }
    //2 nappi
    public void salasana()
    {
        salasanana_näyttö.text=salasananvaihto.salistatlalla.ToString();
        salasanateksti.gameObject.SetActive(true);
        näytä.gameObject.SetActive(false);

    }
    public void poistusalasana()
    {
        salasanateksti.gameObject.SetActive(false);

        näytä.gameObject.SetActive(true);



    }
    public void kopioi_slasana()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = salasanana_näyttö.text;
        textEditor.SelectAll();
        textEditor.Copy();
        print("kopioitu");



    }
    // 3 nappi
    public void resetoi_kaikki()
    {
            
            //PlayerPrefs.DeleteAll();
            //Application.Quit();
            //UnityEditor.EditorApplication.ExitPlaymode();

    }
    // 4 nappi
    //asetukset

    [Header("sivun muokkaus")]
    public GameObject sivu_1;
    public GameObject sivu_2;
    public GameObject nappi_eteen;
    public GameObject nappi_taakse;
    public int sivu = 1;

    public void eteenpäin()
    {
        if (sivu == 1) {
            sivu_1.gameObject.SetActive(false);
            sivu_2.gameObject.SetActive(true);
            nappi_eteen.gameObject.SetActive(false);
            nappi_taakse.gameObject.SetActive(true);
            sivu = 2;
        }


    }
    public void taakesepäin()
    {
        if (sivu == 2)
        {
            nappi_taakse.SetActive(false);
            nappi_eteen.SetActive(true);
            sivu_2.gameObject.SetActive(false);
            sivu_1.gameObject.SetActive(true);
            sivu = 1;
        }

    }
    public void avaa_oikea_sivu()
    {
        if (sivu == 2)
        {
            nappi_taakse.SetActive(true);
            nappi_eteen.SetActive(false);
            sivu_2.gameObject.SetActive(true);
            sivu_1.gameObject.SetActive(false);
            sivu = 2;
        }
        if (sivu == 1)
        {
            sivu_1.gameObject.SetActive(true);
            sivu_2.gameObject.SetActive(false);
            nappi_eteen.gameObject.SetActive(true);
            nappi_taakse.gameObject.SetActive(false);
            sivu = 1;
        }



    }
    


}
