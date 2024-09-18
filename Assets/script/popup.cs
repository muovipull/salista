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

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void avaa()
    {
        if (auki == 0)
        {
            pop.SetActive(true);
            auki = 1;
        }
        else
        {
            pop.SetActive(false);
            auki = 0;
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


}
