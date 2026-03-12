using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;



public class qrmaker : MonoBehaviour
{


    [Header("poistu")]
    [SerializeField]
    private poistu paakoodi;
    


    [Header("muut")]
    public RawImage kuva;
    public TextMeshProUGUI siirtoavain_tekstiulos;
    public TextMeshProUGUI aika_teskstiulos;
    public TextMeshProUGUI info;

    public GameObject qrvalikko;
    public GameObject nautaqrvalikko;
    public GameObject qrkuva;

    public void teeqr() 
    {

        string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + UnityWebRequest.EscapeURL(paakoodi.currentUserId);
        StartCoroutine(LoadQRCode(qrCodeUrl));

        paakoodi.PyydaSiirtoavain(siirtoavain_tekstiulos, aika_teskstiulos);
        info.text = "Tämä on sinun käyttäjä id qr koodina";
        qrkuva.SetActive(true);

    }

    IEnumerator LoadQRCode(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D qrCodeTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            kuva.texture = qrCodeTexture;
        }
        else
        {
            Debug.LogError("QR-koodin lataaminen epäonnistui: " + www.error);
            info.text = "QR-koodin lataaminen epäonnistui, yritä uudestaan";

        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info.text = "Paina pyydäsiirtoavain siirtääksesi tiedot";
        qrkuva.SetActive(false);
    }

    public void sulje()
    {
        qrvalikko.SetActive(true);
        nautaqrvalikko.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
