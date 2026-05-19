using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Networking;

public class Päivitykset : MonoBehaviour
{
    public Kirajudu_sisaaan kirajudu_Sisaaan;
    [Header("Version tarkastaminen")]
    public string tallennettu_versio;
    [Header("Version asetukset")]
    public TextMeshProUGUI versio_text;
    [Header("Hakuasetukset")]
    public string baseRequestUrl = "http://127.0.0.1/api";

    [Header("Kategoriat")]
    public List<string> kategoriat = new List<string>();

    [Header("UI Viittaukset")]
    public GameObject paivitykset_sivu;
    public TextMeshProUGUI sivu_otsikko_teksti;
    public TextMeshProUGUI sivu_leipateksti;

    [Header("Navigaatio")]
    public GameObject seura_nappi;
    public GameObject edelli_nappi;

    [Serializable]
    public class sivudata
    {
        public string otsikko;
        [TextArea(1, 15)] public string tesksti;
        [TextArea(1, 15)] public string teksti;
    }

    [Serializable]
    private class PaivitysDataWrapper { public List<sivudata> ohjeet; }

    private Dictionary<string, List<sivudata>> kaikkienSivujenMuisti = new Dictionary<string, List<sivudata>>();
    private int nykyinenIndeksi = 0;

    void Start()
    {
        if (versio_text != null) versio_text.text = $"V {Application.version}";
        tallennettu_versio = PlayerPrefs.GetString("tallennettu versio1", "virhe");

        // ILMOITETAAN LATAUSHALLINNALLE, ETTÄ MEILLÄ ALKAA LATAUS

        if (kategoriat.Count > 0)
        {
            StartCoroutine(LataaKaikkiTaustalla());
        }
        else
        {
            StartCoroutine(Alku());
        }
    }

    IEnumerator Alku()
    {
        yield return new WaitForSeconds(0.5f);

        if (tallennettu_versio != Application.version)
        {
            AvaaPaivitykset();
            tallennettu_versio = Application.version;
            PlayerPrefs.SetString("tallennettu versio1", Application.version);
            PlayerPrefs.Save();
        }
        else
        {
            Sulje();
        }

        // TÄMÄ KOODI ON VALMIS: ILMOITETAAN LATAUSHALLINNALLE
        
    }

    IEnumerator LataaKaikkiTaustalla()
    {
        foreach (string kat in kategoriat)
        {
            yield return TarkistaJaLataaKategoria(kat);
        }
        StartCoroutine(Alku());
    }

    // (TarkistaJaLataaKategoria, LataaDataPalvelimelta ja UI-metodit pysyvät täysin samoina kuin ennen...)
    IEnumerator TarkistaJaLataaKategoria(string kategoria)
    {
        string paikallinenVersio = PlayerPrefs.GetString("Versio_" + kategoria, "0");
        string versioUrl = $"{baseRequestUrl}/checkversion?kategoria={UnityWebRequest.EscapeURL(kategoria)}";

        using (UnityWebRequest vRequest = UnityWebRequest.Get(versioUrl))
        {
            yield return vRequest.SendWebRequest();

            if (vRequest.result == UnityWebRequest.Result.Success)
            {
                string serverinVersio = vRequest.downloadHandler.text.Trim();

                if (serverinVersio != paikallinenVersio || !kaikkienSivujenMuisti.ContainsKey(kategoria))
                {
                    yield return LataaDataPalvelimelta(kategoria, serverinVersio);
                }
            }
        }
    }

    IEnumerator LataaDataPalvelimelta(string kategoria, string uusiVersio)
    {
        string latausUrl = $"{baseRequestUrl}/getdata?kategoria={UnityWebRequest.EscapeURL(kategoria)}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(latausUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    PaivitysDataWrapper wrapper = JsonUtility.FromJson<PaivitysDataWrapper>(webRequest.downloadHandler.text);

                    if (wrapper != null && wrapper.ohjeet != null)
                    {
                        kaikkienSivujenMuisti[kategoria] = wrapper.ohjeet;
                        PlayerPrefs.SetString("Versio_" + kategoria, uusiVersio);
                        PlayerPrefs.Save();
                    }
                    else
                    {
                        kaikkienSivujenMuisti[kategoria] = new List<sivudata>();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"JSON-virhe kategoriassa {kategoria}: {e.Message}");
                }
            }
        }
    }

    public void AvaaPaivitykset()
    {
        if (paivitykset_sivu != null) paivitykset_sivu.SetActive(true);
        nykyinenIndeksi = 0;
        PaivitaUI();
    }

    public void Seuraava()
    {
        if (nykyinenIndeksi < kategoriat.Count - 1)
        {
            nykyinenIndeksi++;
            PaivitaUI();
        }
    }

    public void Edellinen()
    {
        if (nykyinenIndeksi > 0)
        {
            nykyinenIndeksi--;
            PaivitaUI();
        }
    }

    public void PaivitaUI()
    {
        if (kategoriat.Count == 0 || nykyinenIndeksi >= kategoriat.Count) return;

        string kat = kategoriat[nykyinenIndeksi];

        if (kaikkienSivujenMuisti.ContainsKey(kat) && kaikkienSivujenMuisti[kat].Count > 0)
        {
            sivu_otsikko_teksti.text = kaikkienSivujenMuisti[kat][0].otsikko;

            if (!string.IsNullOrEmpty(kaikkienSivujenMuisti[kat][0].teksti))
            {
                sivu_leipateksti.text = kaikkienSivujenMuisti[kat][0].teksti;
            }
            else
            {
                sivu_leipateksti.text = kaikkienSivujenMuisti[kat][0].tesksti;
            }
        }
        else
        {
            sivu_otsikko_teksti.text = kat;
            sivu_leipateksti.text = "Tämän kategorian päivitystiedot ovat tyhjiä palvelimella.";
        }

        if (seura_nappi != null) seura_nappi.SetActive(nykyinenIndeksi < kategoriat.Count - 1);
        if (edelli_nappi != null) edelli_nappi.SetActive(nykyinenIndeksi > 0);
    }

    public void Sulje()
    {
        if (paivitykset_sivu != null) paivitykset_sivu.SetActive(false);
    }
}