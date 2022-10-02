using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class lluo : MonoBehaviour
{
    public TextMeshProUGUI nimi1;
    public TextMeshProUGUI numero1;
    public TextMeshProUGUI lisatieto1;
    public TextMeshProUGUI maara1;
    public GameObject poista_hyvaksy;
    public GameObject pinkoodi;
    public InputField salis;
    public GameObject peruuta;

    public void Valmistele(string nimi, string numero, string lisatieto, string maara)
    {
        nimi1.text = nimi.ToString();
        numero1.text = numero.ToString();
        lisatieto1.text = lisatieto.ToString();
        maara1.text = maara.ToString();
    
    }
    //tuotteen poisto
    public void poista_tuote()
    {
        poista_hyvaksy.SetActive(true);
        


    }
    public void kylla()//sopii poistaa
    {
        if (salis.text == salasananvaihto.salistatlalla)
        {
            poistu.Instance.poista_tuote(this);
            poista_hyvaksy.SetActive(false);
            Destroy(gameObject);
        }
        
       
    

    }
    public void ei_kay()
    {
        poista_hyvaksy.SetActive(false);





    }


}
