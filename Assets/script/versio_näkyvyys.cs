using TMPro;
using UnityEngine;

public class versio_näkyvyys : MonoBehaviour
{
    public GameObject versio;
    public TextMeshProUGUI nappi_text;
    public int näkyvyys = 1; // 0 = piilota, 1 = näytä
    public string tallentaja;
    [Header("esiasetukset")]
    public string eka_nayta_versionumero ;
    public string eka_piilota_versionumero ;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        näkyvyys = PlayerPrefs.GetInt(tallentaja.ToString(), 1);
        if (näkyvyys == 0)
        {
            versio.SetActive(false);
            nappi_text.text = eka_nayta_versionumero.ToString();
        }
        else
        {
            versio.SetActive(true);
            nappi_text.text = eka_piilota_versionumero.ToString();
        }
    }
    public void vaihda_näkyvyys()
    {
        if (näkyvyys == 0)
        {
            näkyvyys = 1;
            versio.SetActive(true);
            nappi_text.text = eka_piilota_versionumero.ToString();
            PlayerPrefs.SetInt(tallentaja.ToString(), 1);
        }
        else
        {
            näkyvyys = 0;
            versio.SetActive(false);
            nappi_text.text = eka_nayta_versionumero.ToString();
            PlayerPrefs.SetInt(tallentaja.ToString(), 0);
        }
    }

}
