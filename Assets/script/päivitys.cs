using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class päivitys : MonoBehaviour
{
    public int avauskerta=0;
    public GameObject päivitykset;

    // Start is called before the first frame update
    void Start()
    {
        avauskerta = PlayerPrefs.GetInt("avauskerta", 0);
        if (avauskerta == 1 )
        {
            avaa();
            avauskerta=2;
            PlayerPrefs.SetInt("avauskerta", avauskerta);
        }
        if (avauskerta == 0)
        {
            print("avatuu kerta " + avauskerta);
            avauskerta = 1;
            PlayerPrefs.SetInt("avauskerta", avauskerta);
        }
        

    }

    public void avaa()
    {
        päivitykset.SetActive(true);



    }
    public void sulje()
    {
        päivitykset.SetActive(false);
        PlayerPrefs.SetInt("avauskerta", avauskerta);



    }
}
