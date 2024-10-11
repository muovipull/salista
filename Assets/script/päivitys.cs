using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class päivitys : MonoBehaviour
{
    public int avauskerta=0;
    public GameObject päivitykset;
    public TextMeshProUGUI versio_text;
    public string versio;

    // Start is called before the first frame update
    void Start()
    {
        versio = PlayerPrefs.GetString("version", Application.version);
        versio_text.text = $"V {Application.version}";
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
        if (versio != Application.version && avauskerta > 1)
        {
            avaa();
            versio = Application.version;
            PlayerPrefs.SetString("version", versio);


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
