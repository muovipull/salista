using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;



public class AsetaQR : MonoBehaviour
{
    public poistu poistu;


    public GameObject pinvalikko;
    public GameObject nautapinvalikko;

    [Header("input")]

    public TMP_InputField kerta_koodi_input;

    [Header("UI Elementit")]
    [SerializeField] private RawImage _cameraDisplay;
    [SerializeField] private AspectRatioFitter _aspectFitter;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _scanButton;
    [SerializeField] private Button _rotateButton;

    private WebCamTexture _webCamTexture;
    private bool _isScanning = false;
    private float _manualRotation = 0f;

    public string luettuQrKoodi = "";

    public TMP_InputField id_siirto;

    public void sulje()
    
    {
        pinvalikko.SetActive(true);
        nautapinvalikko.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Valitaan takakamera oletuksena
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            string camName = devices[0].name;
            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    camName = devices[i].name;
                    break;
                }
            }
            // Pyydetään hyvä resoluutio, Unity valitsee lähimmän mahdollisen
            _webCamTexture = new WebCamTexture(camName, 1280, 720);
        }
        else
        {
            if (_resultText != null) _resultText.text = "<color=red>Kameraa ei löydy!";
            return;
        }

        _cameraDisplay.texture = _webCamTexture;

        _scanButton.onClick.AddListener(StartScanning);

        if (_rotateButton != null)
            _rotateButton.onClick.AddListener(ManualRotate);

        if (_resultText != null)
            _resultText.text = "<color=blue>Paina nappia skannataksesi";

        // Varmistetaan, että RawImagen ankkurit ovat keskellä, jotta rotaatio toimii oikein
        _cameraDisplay.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _cameraDisplay.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _cameraDisplay.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
        {
            ApplyLayout();
        }
    }
    public void ManualRotate()
    {
        _manualRotation -= 90f;
    }
    private void ApplyLayout()
    {
        // 1. KORJATAAN ROTAATIO
        // Yhdistetään automaattinen kameran kulma ja manuaalinen korjaus
        float totalRotation = _webCamTexture.videoRotationAngle + _manualRotation;
        _cameraDisplay.rectTransform.localEulerAngles = new Vector3(0, 0, totalRotation);

        // 2. KORJATAAN PEILAUS (pystysuuntainen korjaus)
        float scaleY = _webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        _cameraDisplay.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        // 3. KORJATAAN ASPECT RATIO
        if (_aspectFitter != null && _webCamTexture.width > 100)
        {
            // Lasketaan kuvasuhde kameran datasta
            float ratio = (float)_webCamTexture.width / (float)_webCamTexture.height;

            // Jos kamera on kääntynyt 90 tai 270 astetta, suhde pitää kääntää ympäri
            if (Mathf.Abs(totalRotation) % 180 != 0)
            {
                ratio = 1f / ratio;
            }

            _aspectFitter.aspectRatio = ratio;
        }
    }

    public void StartScanning()
    {
        if (!_webCamTexture.isPlaying)
        {
            _webCamTexture.Play();
        }

        if (!_isScanning)
        {
            _isScanning = true;
            if (_resultText != null) _resultText.text = "<color=yellow>Etsitään QR-koodia...";
            StartCoroutine(ScanRoutine());
        }
    }

    private IEnumerator ScanRoutine()
    {
        IBarcodeReader barcodeReader = new BarcodeReader();

        while (_isScanning)
        {
            if (_webCamTexture.width > 100)
            {
                try
                {
                    // Haetaan kameran kuva
                    var snap = _webCamTexture.GetPixels32();
                    // Dekoodataan QR-koodi
                    var result = barcodeReader.Decode(snap, _webCamTexture.width, _webCamTexture.height);

                    if (result != null)
                    {

                        // Skannaus onnistui, näytä tulos

                        _isScanning = false;
                        luettuQrKoodi = result.Text;

                        id_siirto.text=result.Text;

                        _resultText.text = "<color=green>Qr koodi scannattu";
                        Debug.Log("Skannattu: " + result.Text);

                        // Jos haluat avata linkin automaattisesti:
                        // Application.OpenURL(result.Text);
                    }
                }
                catch (System.Exception e)
                {
                    _resultText.text = "<color=red>Skannausvirhe";

                    Debug.LogWarning("Skannausvirhe: " + e.Message);
                }
            }
            yield return new WaitForSeconds(0.2f); // Skannataan 5 kertaa sekunnissa
        }
    }

    public void aseta()
    {
        poistu.aseta_id(luettuQrKoodi, kerta_koodi_input, _resultText);


    }


    void OnDisable()
    {
        if (_webCamTexture != null) _webCamTexture.Stop();
    }
}
