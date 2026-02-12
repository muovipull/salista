using UnityEngine;

public class idsettings : MonoBehaviour
{
    [Header("pin")][SerializeField]

    public GameObject tastapoispin;
    public GameObject tahantoisestapin;

    [Header("qr")]
    [SerializeField]

    public GameObject tastapoisqr;
    public GameObject tahantoisestaqr;

    public GameObject valikko;

    public string tapa;

    public void nautaid()
    {
        valikko.SetActive(false);

        if (tapa == "pin")
        {
            valikko.SetActive(false);
            tahantoisestapin.SetActive(true);
        }

        if (tapa == "qr")
        {
            valikko.SetActive(false);
            tahantoisestaqr.SetActive(true);
        }


    }
    public void asetaid()
    {
        valikko.SetActive(false);

        if (tapa == "pin")
        {
            valikko.SetActive(false);
            tastapoispin.SetActive(true);
        }

        if (tapa == "qr")
        {
            valikko.SetActive(false);
            tastapoisqr.SetActive(true);
        }


    }
    public void poistu()
    {

        valikko.SetActive(false);

    }

    public void avaavalikkopin()
    {
        valikko.SetActive(true);
        tapa = "pin";


    }
    public void avaavalikkoqr()
    {
        valikko.SetActive(true);
        tapa = "qr";

    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
