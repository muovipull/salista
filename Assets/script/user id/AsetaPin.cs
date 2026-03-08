using TMPro;
using UnityEngine;

public class AsetaPin : MonoBehaviour
{
    public poistu poistu;


    public GameObject pinvalikko;
    public GameObject nautapinvalikko;

    [Header("input")]

    public TMP_InputField id_input;
    public TMP_InputField kerta_koodi_input;

    [Header("text")]
    
    public TextMeshProUGUI info;


    public void aseta_uusi()
    {
        poistu.aseta_id(id_input, kerta_koodi_input, info);

    }

    public void sulje()
    {
        pinvalikko.SetActive(true);

        nautapinvalikko.SetActive(false);


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info.text = "Syötä id ja kertakoodi";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
