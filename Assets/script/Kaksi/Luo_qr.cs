
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class Luo_qr : MonoBehaviour
{
    [SerializeField] private RawImage displayImage; // Raahaa tähän UI:n RawImage

    void Start()
    {
        string teksti = "https://www.google.fi";
        Texture2D qrKoodi = GeneroiQR(teksti, 256, 256);
        displayImage.texture = qrKoodi;
    }

    private Texture2D GeneroiQR(string teksti, int leveys, int korkeus)
    {
        // Alustetaan kirjoittaja
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = korkeus,
                Width = leveys,
                Margin = 1 // Valkoinen reunus koodin ympärillä
            }
        };

        // Luodaan värit (Color32 array)
        Color32[] pikselit = writer.Write(teksti);

        // Luodaan uusi tekstuuri ja asetetaan pikselit
        Texture2D tekstuuri = new Texture2D(leveys, korkeus);
        tekstuuri.SetPixels32(pikselit);
        tekstuuri.Apply();

        return tekstuuri;
    }
}
