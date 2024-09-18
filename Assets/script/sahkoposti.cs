using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sahkoposti : MonoBehaviour
{
    public GameObject Kato;
    public GameObject sahko;
    public InputField salis;

    public void kaso()
    {
        Kato.SetActive(true);

    }
    
    public void sanko()
    {
        if (salis.text == salasananvaihto.salistatlalla)
        {
            sahko.gameObject.SetActive(true);
            Kato.SetActive(false);
            salis.Select();
            salis.text = "";
        }
        else 
        {
            aloitus_jos_aloitusvalmis.jäljellä_olevat_yritykset -= 1;
            
        }


    }
    public void peruna()
    {
        Kato.SetActive(false);




    }

    public void valmis()
    {
        sahko.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            sanko();
        }
    }

}
