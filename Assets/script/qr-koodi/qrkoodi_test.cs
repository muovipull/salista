using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class qrkoodi_test : MonoBehaviour
{
    [Header("UI Elementit")]
    public TextMeshProUGUI resultText;
    public RawImage cameraDisplay;
    public Button scanButton;

    private WebCamTexture camTexture;
    private BarcodeReader reader; // K‰ytet‰‰n t‰t‰
    private bool isScanning = false;

    void Start()
    {
        // Alustetaan lukija
        reader = new BarcodeReader();

        camTexture = new WebCamTexture();
        if (cameraDisplay != null)
        {
            cameraDisplay.texture = camTexture;
        }
        camTexture.Play();

        if (scanButton != null)
        {
            scanButton.onClick.AddListener(StartScanning);
        }

        resultText.text = "Paina nappia skannataksesi";
    }

    public void StartScanning()
    {
        isScanning = true;
        resultText.text = "Etsit‰‰n QR-koodia...";
        scanButton.interactable = false;
    }

    void Update()
    {
        if (isScanning && camTexture.isPlaying && Time.frameCount % 10 == 0)
        {
            Scan();
        }
    }

    private void Scan()
    {
        try
        {
            // Haetaan kameran v‰rit
            Color32[] pixels = camTexture.GetPixels32();
            int width = camTexture.width;
            int height = camTexture.height;

            // Luodaan LuminanceSource, joka muuntaa Unityn v‰rit ZXing-muotoon
            var source = new Color32LuminanceSource(pixels, width, height);

            // Dekoodataan k‰ytt‰en l‰hdett‰
            var result = reader.Decode(source);

            if (result != null)
            {
                resultText.text = "Tulos: " + result.Text;
                isScanning = false;
                scanButton.interactable = true;
                Handheld.Vibrate();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("QR-virhe: " + e.Message);
        }
    }
}