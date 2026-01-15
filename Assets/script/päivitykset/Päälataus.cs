using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class Päälataus : MonoBehaviour
{
    // URL-osoitteet
    public static string otsikkoUrl = "https://muovipull.github.io/paivitys/otsikko1.txt";
    public static string tekstiUrl = "https://muovipull.github.io/paivitys/leipa1.txt";

    public TextMeshProUGUI targetTextMeshPro;

    // Staattiset muuttujat sisällölle
    public static string LadattuOtsikko { get; private set; } = string.Empty;
    public static string LadattuTeksti { get; private set; } = string.Empty;
    public static string[] LadatunTekstinRivit { get; private set; } = new string[0];

    // Seurataan onko lataus jo tehty kerran pelin aikana
    public static bool OnkoLadattu { get; private set; } = false;

    // Tapahtumat
    public static event System.Action OnAllFilesLoaded;
    public static event System.Action OnOtsikkoLoaded;
    public static event System.Action OnTekstiLoaded;

    private int _filesToLoad = 0;
    private int _filesLoaded = 0;

    void Start()
    {
        // Estetään uusi lataus, jos tiedot on jo olemassa
        if (OnkoLadattu)
        {
            Debug.Log("Tiedot on jo haettu. Käytetään välimuistia.");
            PaivitaUIJosTarpeen();
            return;
        }

        Debug.Log("Ensimmäinen käynnistys: aloitetaan lataus...");
        AloitaAutomaattinenLataus();
    }

    private void AloitaAutomaattinenLataus()
    {
        _filesLoaded = 0;
        _filesToLoad = 2; // Vain otsikko ja teksti

        // 1. Otsikon lataus
        StartCoroutine(LoadTextFile(otsikkoUrl, (text) => {
            LadattuOtsikko = text;
            OnOtsikkoLoaded?.Invoke();
            CheckAllFilesLoaded();
        }));

        // 2. Leipätekstin lataus
        StartCoroutine(LoadTextFile(tekstiUrl, (text) => {
            LadattuTeksti = text;
            LadatunTekstinRivit = text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .ToArray();
            OnTekstiLoaded?.Invoke();
            CheckAllFilesLoaded();
        }));
    }

    private void CheckAllFilesLoaded()
    {
        _filesLoaded++;
        if (_filesLoaded >= _filesToLoad)
        {
            OnkoLadattu = true;
            Debug.Log("Molemmat tiedostot ladattu onnistuneesti!");
            PaivitaUIJosTarpeen();
            OnAllFilesLoaded?.Invoke();
        }
    }

    private void PaivitaUIJosTarpeen()
    {
        if (targetTextMeshPro != null && !string.IsNullOrEmpty(LadattuOtsikko))
        {
            targetTextMeshPro.text = "Otsikko: " + LadattuOtsikko;
        }
    }

    IEnumerator LoadTextFile(string url, System.Action<string> onComplete)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Latausvirhe ({url}): {webRequest.error}");
                onComplete?.Invoke("Latausvirhe");
            }
            else
            {
                onComplete?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    // Jos tarvitset myöhemmin napin, joka päivittää tiedot väkisin:
    public void PakotaPaivitys()
    {
        OnkoLadattu = false;
        AloitaAutomaattinenLataus();
    }
}