using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class salis : MonoBehaviour
{
    public GameObject Kato;
    public GameObject sahko;
    public InputField salainen;
    

    public void kaso()
    {
        Kato.SetActive(true);

    }

    public void sanko()
    {
        if (salainen.text == salasananvaihto.salistatlalla)
        {
            sahko.gameObject.SetActive(true);
            Kato.SetActive(false);
            salainen.Select();
            salainen.text = "";
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