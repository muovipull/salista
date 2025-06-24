using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Linq; // Tarvitaan .ToArray() -metodille

public class Päälataus : MonoBehaviour
{
    // URL-osoitteet tekstitiedostoille
    public static string otsikkoUrl = "https://muovipull.github.io/paivitys/otsikko1.txt";
    public static string otsikkoUrl2 = "https://muovipull.github.io/paivitys/otsikko2.txt";
    // --- MUUTOS TÄSSÄ: Nimi muutettu teksti1.txt -> leipa1.txt ---
    public static string tekstiUrl = "https://muovipull.github.io/paivitys/leipa1.txt";

    // Valinnainen: Vedä ja pudota TextMeshProUGUI-objektisi tähän Inspectorissa
    public TextMeshProUGUI targetTextMeshPro;

    // Staattiset muuttujat ladatulle sisällölle
    public static string LadattuOtsikko { get; private set; } = string.Empty;
    public static string LadattuOtsikko2 { get; private set; } = string.Empty;
    public static string LadattuTeksti { get; private set; } = string.Empty;

    // Tähän tallennetaan tekstitiedoston sisältö riveittäin
    public static string[] LadatunTekstinRivit { get; private set; } = new string[0];

    // Tapahtumat ilmoittamaan, kun jokin lataus on valmis
    public static event System.Action OnAllFilesLoaded;
    public static event System.Action OnOtsikkoLoaded;
    public static event System.Action OnOtsikko2Loaded;
    public static event System.Action OnTekstiLoaded;

    private int _filesToLoad = 0;
    private int _filesLoaded = 0;

    void Start()
    {
        Debug.Log("Päälataus-skripti käynnistyy. Aloitetaan tiedostojen lataus automaattisesti...");
        AloitaAutomaattinenLataus();
    }

    private void AloitaAutomaattinenLataus()
    {
        _filesLoaded = 0;
        _filesToLoad = 3;

        StartCoroutine(LoadTextFile(otsikkoUrl, (text) => {
            LadattuOtsikko = text;
            Debug.Log($"Ladattu otsikko ({otsikkoUrl}):\n{LadattuOtsikko}");
            if (targetTextMeshPro != null) targetTextMeshPro.text = "Otsikko ladattu: " + text.Substring(0, Mathf.Min(text.Length, 50));
            OnOtsikkoLoaded?.Invoke();
            CheckAllFilesLoaded();
        }));
        StartCoroutine(LoadTextFile(otsikkoUrl2, (text) => {
            LadattuOtsikko = text;
            Debug.Log($"Ladattu otsikko ({otsikkoUrl2}):\n{LadattuOtsikko2}");
            if (targetTextMeshPro != null) targetTextMeshPro.text = "Otsikko ladattu: " + text.Substring(0, Mathf.Min(text.Length, 50));
            OnOtsikko2Loaded?.Invoke();
            CheckAllFilesLoaded();
        }));

        StartCoroutine(LoadTextFile(tekstiUrl, (text) => {
            LadattuTeksti = text;

            // Tekstin jakaminen riveiksi
            LadatunTekstinRivit = text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim()) // Poista ylimääräiset välilyönnit rivien alusta/lopusta
                                      .ToArray();

            Debug.Log($"Ladattu koko teksti ({tekstiUrl}):\n{LadattuTeksti}");
            Debug.Log($"Tekstitiedostossa rivejä yhteensä: {LadatunTekstinRivit.Length}");

            // Tulostetaan jokainen rivi konsoliin
            Debug.Log("--- Ladatun tekstin rivit ---");
            for (int i = 0; i < LadatunTekstinRivit.Length; i++)
            {
                Debug.Log($"Rivi {i + 1}: {LadatunTekstinRivit[i]}");
            }
            Debug.Log("-----------------------------");

            OnTekstiLoaded?.Invoke();
            CheckAllFilesLoaded();
        }));
    }

    private void CheckAllFilesLoaded()
    {
        _filesLoaded++;
        if (_filesLoaded >= _filesToLoad)
        {
            Debug.Log("Kaikki tiedostot ladattu!");
            OnAllFilesLoaded?.Invoke();
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
                Debug.LogError($"Virhe ladattaessa tekstitiedostoa URL:stä {url}: " + webRequest.error);
                onComplete?.Invoke($"VIRHE: Lataus epäonnistui URL:stä {url}.");
            }
            else
            {
                onComplete?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public void ReloadOtsikko() { StartCoroutine(LoadTextFile(otsikkoUrl, (text) => LadattuOtsikko = text)); }
    public void ReloadOtsikko2() { StartCoroutine(LoadTextFile(otsikkoUrl2, (text) => LadattuOtsikko2 = text)); }
    public void ReloadTeksti() { StartCoroutine(LoadTextFile(tekstiUrl, (text) => LadattuTeksti = text)); }
}