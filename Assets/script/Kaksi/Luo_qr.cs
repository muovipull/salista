
using UnityEngine;
using System.Net.NetworkInformation;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Luo_qr : MonoBehaviour
{
    public RawImage qrCodeImage;
    private string otpauthUrl = "otpauth://totp/Salista:santeri.taavitsainen.koodaaja@gmil.com?secret=ABWSTDQHG3OFIATTT4&issuer=Salista&digits=6";

    void Start()
    {
        string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + UnityWebRequest.EscapeURL(otpauthUrl);
        StartCoroutine(LoadQRCode(qrCodeUrl));
    }

    IEnumerator LoadQRCode(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D qrCodeTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            qrCodeImage.texture = qrCodeTexture;
        }
        else
        {
            Debug.LogError("QR-koodin lataaminen epäonnistui: " + www.error);
        }
    }
}
