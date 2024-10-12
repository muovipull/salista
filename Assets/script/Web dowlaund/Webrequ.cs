using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Webrequ : MonoBehaviour
{
    public RawImage rawImage;  // Ved‰ ja pudota Raw Image t‰h‰n Unity-editorissa
    public string imageUrl = "https://muovipull.github.io/WIN_20240626_21_20_20_Pro.jpg";  // Kuvan URL

    void Start()
    {
        StartCoroutine(LoadImageFromURL(imageUrl));
    }

    IEnumerator LoadImageFromURL(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);  // Pyydet‰‰n kuvaa URL:st‰
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Virhe ladatessa kuvaa: " + request.error);
        }
        else
        {
            // Ladataan Texture2D ja asetetaan se Raw Imageen
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            rawImage.texture = texture;
            rawImage.SetNativeSize();  // Asetetaan kuvan alkuper‰inen koko, jos tarvitaan
            Debug.Log("Kuva ladattu ja asetettu Raw Imageen!");
        }
    }
}
