using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using System.Collections;

public class QRScannerTMP : MonoBehaviour
{
    [Header("UI Elementit")]
    [SerializeField] private RawImage _cameraDisplay;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _scanButton;

    private WebCamTexture _webCamTexture;
    private bool _isScanning = false;

    void Start()
    {
        _webCamTexture = new WebCamTexture();
        _cameraDisplay.texture = _webCamTexture;
        _scanButton.onClick.AddListener(StartScanning);
        _resultText.text = "Paina nappia skannataksesi";
    }

    public void StartScanning()
    {
        if (!_webCamTexture.isPlaying)
        {
            _webCamTexture.Play();

            // 1. AUTOMAATTITARKENNUS (Vain Android/iOS)
#if !UNITY_EDITOR
            _webCamTexture.autoFocusPoint = new Vector2(0.5f, 0.5f);
#endif

            // 2. KUVAN KÄÄNTÖ 90 ASTETTA
            // Korjataan RawImagen rotaatio, jotta kuva näkyy oikein puhelimessa
            _cameraDisplay.rectTransform.localEulerAngles = new Vector3(0, 0, -90f);
        }

        if (!_isScanning)
        {
            _isScanning = true;
            _resultText.text = "Etsitään QR-koodia...";
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
                // Huom: ZXing lukee pikselit datasta, vaikka näyttö olisi käännetty UI:ssa
                var snap = _webCamTexture.GetPixels32();
                var result = barcodeReader.Decode(snap, _webCamTexture.width, _webCamTexture.height);

                if (result != null)
                {
                    _resultText.text = "Tulos: <color=green>" + result.Text + "</color>";
                    _isScanning = false;
                    _webCamTexture.Stop();
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnDisable()
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying)
            _webCamTexture.Stop();
    }
}