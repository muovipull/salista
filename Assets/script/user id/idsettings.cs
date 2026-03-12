using UnityEngine;

public class idsettings : MonoBehaviour
{
    [Header("pin")][SerializeField]

    public GameObject tastapoispin;
    public GameObject tahantoisestapin;

    public GameObject pinvalikko;


    [Header("qr")]
    [SerializeField]

    public GameObject tastapoisqr;
    public GameObject tahantoisestaqr;

    public GameObject qrvalikko;

    [Header("id_kaikki")]
    public GameObject id_paakaikki_asiat;

    public void nautaidpin()
    {

        qrvalikko.SetActive(false);
        pinvalikko.SetActive(false);

        tastapoispin.SetActive(true);

    }

    public void asetaidpin()
    {
        qrvalikko.SetActive(false);
        pinvalikko.SetActive(false);

        tahantoisestapin.SetActive(true);

    }

    public void nautaidqr()
    {

        qrvalikko.SetActive(false);
        pinvalikko.SetActive(false);

        tastapoisqr.SetActive(true);

    }

    public void asetaidqr()
    {
        qrvalikko.SetActive(false);
        pinvalikko.SetActive(false);

        tahantoisestaqr.SetActive(true);

    }

    public void poistu()
    {
        qrvalikko.SetActive(false);
        pinvalikko.SetActive(false);
        id_paakaikki_asiat.SetActive(true);

    }

    public void avaavalikkopin()
    {
        pinvalikko.SetActive(true);
        id_paakaikki_asiat.SetActive(true);
    }
    public void avaavalikkoqr()
    {
        qrvalikko.SetActive(true);
        id_paakaikki_asiat.SetActive(true);

    }
}
