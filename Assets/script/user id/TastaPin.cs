using TMPro;
using UnityEngine;

public class TastaPin : MonoBehaviour
{

    public poistu poistu;

    public GameObject pinvalikko;
    public GameObject nautapinvalikko;


    [Header("ylä")]

    public TextMeshProUGUI info_ja_aika;
    public TextMeshProUGUI siiroavain;
    public GameObject alku_laatiokko_yla;
    public GameObject painettu_laatiokko_yla;


    [Header("ala")]

    public GameObject ala_laatiokko;
    public TextMeshProUGUI id;
    public TextMeshProUGUI info;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ala_laatiokko.SetActive(false);
        painettu_laatiokko_yla.SetActive(false);

        alku_laatiokko_yla.SetActive(true);

    }

    public void pyyda_siirtoavain_ja_nauta_id()
    {
        poistu.PyydaSiirtoavain(siiroavain, info_ja_aika);

        ala_laatiokko.SetActive(true);
        painettu_laatiokko_yla.SetActive(true);

        alku_laatiokko_yla.SetActive(false);

        id.text = poistu.currentUserId;


    }




    public void sulje()
    {
        nautapinvalikko.SetActive(false);
        pinvalikko.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
