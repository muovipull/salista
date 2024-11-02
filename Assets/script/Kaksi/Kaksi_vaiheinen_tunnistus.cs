using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Kaksi_vaiheinen_tunnistus : MonoBehaviour
{
    public static string tili = "santeri.taavitsainen.koodaaja@gmail.com";
    private static string secret = "AC3PLI3ZH2AOFP3LRUr4f5";
    public RawImage qrCodeImage;

    public static string GenerateSecretKey()
    {
        byte[] key = new byte[10]; // 10 bytes = 80 bits
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(key);
        }
        print("luotu");
        print(Base32Encode(key) + "avain");
        return Base32Encode(key);
    }

    // Base32 encoding to make it compatible with Google Authenticator
    private static string Base32Encode(byte[] data)
    {
        const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        StringBuilder result = new StringBuilder((data.Length + 7) * 8 / 5);
        int currentByte = 0;
        int bitsRemaining = 8;
        int mask = 0x1F; // 5 bits mask (since base32 uses 5 bits)

        for (int i = 0; i < data.Length; i++)
        {
            currentByte = (currentByte << 8) | (data[i] & 0xFF);
            bitsRemaining += 8;

            while (bitsRemaining >= 5)
            {
                result.Append(base32Chars[(currentByte >> (bitsRemaining - 5)) & mask]);
                bitsRemaining -= 5;
            }
        }

        if (bitsRemaining > 0)
        {
            result.Append(base32Chars[(currentByte << (5 - bitsRemaining)) & mask]);
        }

        return result.ToString();
    }







    void Start()

    {
        secret = GenerateSecretKey();
        string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + UnityWebRequest.EscapeURL("otpauth://totp/Salista:" + Kaksi_vaiheinen_tunnistus.tili + "?secret=" + Kaksi_vaiheinen_tunnistus.secret + "&issuer=Salista&digits=6");
        StartCoroutine(LoadQRCode(qrCodeUrl));
        //Kaksi_vaiheinen_tunnistus.secret = GenerateSecretKey();
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




