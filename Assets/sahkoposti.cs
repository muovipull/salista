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

}
