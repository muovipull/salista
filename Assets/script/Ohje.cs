using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Networking;

public class Ohje : MonoBehaviour
{
    [Header("Hakuasetukset")]
    public string baseRequestUrl = "http://192.168.101.195:7028/api";

    // Lista kaikista kategorioista, jotka haluat ladata heti
    public List<string> kategoriat = new List<string> { "pelaaminen", "aseet", "kosmetiikka" };

    [Header("UI Viittaukset")]
    public GameObject ohje_sivu;
    public TextMeshProUGUI sivu_otsikko_teksti;
    public TextMeshProUGUI sivu_leipateksti;

    [Header("Navigaatio")]
    public GameObject seura_nappi;
    public GameObject edelli_nappi;

    [Serializable]
    public class sivudata { public string otsikko; [TextArea(1, 15)] public string tesksti; }

    [Serializable]
    private class OhjeDataWrapper { public List<sivudata> ohjeet; }

    // Sanakirja (Dictionary) tallentaa kaikki ladatut sivut kategorian nimen mukaan
    private Dictionary<string, List<sivudata>> kaikkienSivujenMuisti = new Dictionary<string, List<sivudata>>();
    private int nykyinenIndeksi = 0;

    void Start()
    {
        // Kun sovellus käynnistyy, aloitetaan kaikkien kategorioiden päivitys taustalla
        StartCoroutine(LataaKaikkiTaustalla());
    }

    IEnumerator LataaKaikkiTaustalla()
    {
        Debug.Log("Aloitetaan ohjeiden tarkistus palvelimelta...");

        foreach (string kat in kategoriat)
        {
            yield return TarkistaJaLataaKategoria(kat);
        }

        Debug.Log("Kaikki ohjeet tarkistettu ja päivitetty!");
    }

    IEnumerator TarkistaJaLataaKategoria(string kategoria)
    {
        string paikallinenVersio = PlayerPrefs.GetString("Versio_" + kategoria, "0");
        string versioUrl = $"{baseRequestUrl}/checkversion?kategoria={kategoria}";

        using (UnityWebRequest vRequest = UnityWebRequest.Get(versioUrl))
        {
            yield return vRequest.SendWebRequest();

            if (vRequest.result == UnityWebRequest.Result.Success)
            {
                string serverinVersio = vRequest.downloadHandler.text.Trim();

                // Jos versio on uusi, ladataan data. Jos ei, ladataan silti muistiin, jos se puuttuu.
                if (serverinVersio != paikallinenVersio || !kaikkienSivujenMuisti.ContainsKey(kategoria))
                {
                    yield return LataaDataPalvelimelta(kategoria, serverinVersio);
                }
            }
        }
    }

    IEnumerator LataaDataPalvelimelta(string kategoria, string uusiVersio)
    {
        string latausUrl = $"{baseRequestUrl}/getdata?kategoria={kategoria}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(latausUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                OhjeDataWrapper wrapper = JsonUtility.FromJson<OhjeDataWrapper>(webRequest.downloadHandler.text);

                // Tallennetaan sanakirjaan
                kaikkienSivujenMuisti[kategoria] = wrapper.ohjeet;

                // Tallennetaan versio levylle
                PlayerPrefs.SetString("Versio_" + kategoria, uusiVersio);
                PlayerPrefs.Save();

                Debug.Log($"Kategoria '{kategoria}' päivitetty versioon {uusiVersio}");
            }
        }
    }

    // --- UI TOIMINNOT ---

    public void AvaaOhjeet()
    {
        ohje_sivu.SetActive(true);
        nykyinenIndeksi = 0; // Aloitetaan ekasta kategoriasta
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
        string kat = kategoriat[nykyinenIndeksi];

        if (kaikkienSivujenMuisti.ContainsKey(kat))
        {
            sivu_otsikko_teksti.text = kaikkienSivujenMuisti[kat][0].otsikko;
            sivu_leipateksti.text = kaikkienSivujenMuisti[kat][0].tesksti;
        }
        else
        {
            sivu_otsikko_teksti.text = "Ladataan...";
            sivu_leipateksti.text = "Tietoja haetaan vielä palvelimelta...";
        }

        // Nappien näkyvyys
        if (seura_nappi != null) seura_nappi.SetActive(nykyinenIndeksi < kategoriat.Count - 1);
        if (edelli_nappi != null) edelli_nappi.SetActive(nykyinenIndeksi > 0);
    }

    public void Sulje() => ohje_sivu.SetActive(false);
}