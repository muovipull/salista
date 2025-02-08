using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Kaksi_vaiheinen_tunnistus : MonoBehaviour
{
    public static string tili = "santeri.taavitsainen.koodaaja@gmail.com";
    public static string secret = "";
    public int eka;
    public RawImage qrCodeImage;
    public TMP_InputField sahkoposti;
    public TMP_InputField vara_salis;
    public TextMeshProUGUI tiedot;
    public GameObject ohje;
    public GameObject asetukset;
    public GameObject kaytto;
    public GameObject sisaiset_asetukset;
    public static string otettu ="false";
    public static string vara = "";


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

    public void avaa_sisainen()
    {

        sisaiset_asetukset.SetActive(true);
        asetukset.SetActive(false);



    }





    public void luo_tallennsu_koodi()
    {
        eka = PlayerPrefs.GetInt("eka", 0);
        if (eka == 0)
        {
            if (vara_salis.text.ToString() == "")
            {

                vara = salasananvaihto.salistatlalla;

            }
            else
            {
                vara = vara_salis.text.ToString();
            }

            PlayerPrefs.SetString("vara", Kaksi_vaiheinen_tunnistus.vara);
            Kaksi_vaiheinen_tunnistus.secret = GenerateSecretKey();
            PlayerPrefs.SetString("secret", Kaksi_vaiheinen_tunnistus.secret);

            eka = 1;
            PlayerPrefs.SetInt("eka", eka);
        }
        if (eka == 1)
        {
            Kaksi_vaiheinen_tunnistus.secret = PlayerPrefs.GetString("secret", "a");
        }

        Kaksi_vaiheinen_tunnistus.tili = sahkoposti.text.ToString();
        tiedot.text = "Nimi on Salista:" + Kaksi_vaiheinen_tunnistus.tili + "Salausavain on " + Kaksi_vaiheinen_tunnistus.secret;
        string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + UnityWebRequest.EscapeURL("otpauth://totp/Salista:" + Kaksi_vaiheinen_tunnistus.tili + "?secret=" + Kaksi_vaiheinen_tunnistus.secret + "&issuer=Salista&digits=6");
        StartCoroutine(LoadQRCode(qrCodeUrl));

    }
    public void Poistu()

    {
        if(eka==1)
        {
            asetukset.SetActive(true);
            kaytto.SetActive(false);
            Kaksi_vaiheinen_tunnistus.otettu = "true";
            PlayerPrefs.SetString("true", Kaksi_vaiheinen_tunnistus.otettu);
            print("eka "+ eka.ToString());

        }
        else
        {
            asetukset.SetActive(true);
            kaytto.SetActive(false);


        }
        


    }
    public void avaa_ohje()
    {
        asetukset.SetActive(false);
        ohje.SetActive(true);
        sisaiset_asetukset.SetActive(false);

    }
    public void sulje_ohje()
    {
        ohje.SetActive(false);
        asetukset.SetActive(true);
        


    }
    public void avaa_kaytto()
    {
        kaytto.SetActive(true);
        ohje.SetActive(false);


    }
    public void valtuutuksen_poisto()
    {
        otettu = "false";
        PlayerPrefs.SetString("true", Kaksi_vaiheinen_tunnistus.otettu);
        asetukset.SetActive(true);
        sisaiset_asetukset.SetActive(false);

    }
    IEnumerator LoadQRCode(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D qrCodeTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            qrCodeImage.texture = qrCodeTexture;
            print(" qr-code kuva laitettu");
        }
        else
        {
            Debug.LogError("QR-koodin lataaminen epäonnistui: " + www.error);
        }
    }
    private void Start()
    {
        Kaksi_vaiheinen_tunnistus.vara = PlayerPrefs.GetString("vara", salasananvaihto.salistatlalla);
        Kaksi_vaiheinen_tunnistus.secret = PlayerPrefs.GetString("secret", "a");
        Kaksi_vaiheinen_tunnistus.otettu = PlayerPrefs.GetString("true", "false");
    }
}




