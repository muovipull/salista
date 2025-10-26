using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;


public class poistu : MonoBehaviour
{
    public lluo muuttuja;
    public GameObject canvas;
    public GameObject canvas1;

    [Header("Nayta id")]
    public GameObject nayta_id_paneli;
    public TextMeshProUGUI nayta_id_text;

    [Header("Aseta id & Siirtoavain")]
    public GameObject aseta_id_paneli;
    public TMP_InputField aseta_id_input; // Uuden laitteen ID
    // UUSI: InputField siirtoavaimen sy�tt��n
    public TMP_InputField one_time_key_input;
    public TextMeshProUGUI varoitus;
    // UUSI: Tekstikentt� avaimen n�ytt�miseen
    public TextMeshProUGUI generated_key_text;
    public TextMeshProUGUI key_expiry_text; // Tekstikentt� vanhenemisajan n�ytt�miseen

    [Header("Input Fields (Add New Item)")]
    public InputField nimi1;
    public InputField numero1;
    public InputField lisatieto1;
    public InputField maara1;
    public InputField verkkosivu1;

    private string currentUserId;
    private string currentOneTimeKey; // Tilap�isesti luotu avain
    private float keyExpiryTime;      // Avaimen vanhenemisaika (Unix-aikaleima)

    public Transform ruudukkoja;

    public List<lluo> tavara_lista = new List<lluo>();

    public static poistu Instance;

    [Header("debug")]
    public GameObject debug_sivu;
    public TMP_InputField debug_input;
    public TextMeshProUGUI debug_text;
    // HUOM: Tarkista IP-osoite ja portti ennen k�ytt��!
    private readonly string baseUrl = "https://fa6b3b1bda97.ngrok-free.app"; // julkaisu palvelin
    //private readonly string baseUrl = "http://192.168.101.107:5001"; //testi palvelin

    // --- K�ytt�j�n ID:n ja avaimen hallinta ---

    public void nayta_id()
    {
        nayta_id_paneli.SetActive(true);
        nayta_id_text.text = currentUserId;
    }

    public void piilota_id()
    {
        nayta_id_paneli.SetActive(false);
    }

    /// <summary>
    /// Avaa paneelin, jolla k�ytt�j�n ID ja tilap�inen siirtoavain voidaan asettaa.
    /// </summary>
    public void avaa_id_asetus()
    {
        aseta_id_paneli.SetActive(true);
        aseta_id_input.text = "";
        one_time_key_input.text = ""; // Tyhjennet��n avainkentt� aina
        varoitus.text = "";
        generated_key_text.text = "Avainta ei pyydetty";
        key_expiry_text.text = "";
    }

    /// <summary>
    /// Pyyt�� palvelimelta uuden 8-numeroisen siirtoavaimen, joka on voimassa 15 min.
    /// Kutsutaan vanhalla laitteella, josta tiedot halutaan siirt��.
    /// </summary>
    public void PyydaSiirtoavain()
    {
        StartCoroutine(GenerateTransferKey(currentUserId));
    }

    /// <summary>
    /// Yritt�� asettaa uuden ID:n ja ladata datan tilap�isell� siirtoavaimella.
    /// Kutsutaan uudella laitteella, johon tiedot halutaan siirt��.
    /// </summary>
    /// 
    public void avaa_debug_sivu()
    {
        debug_sivu.SetActive(true);
        
        debug_text.text = "Nykyinen k�ytt�j�n ID: " + currentUserId;

        debug_input.text = "";
    }
    public void aseta_debug_id()
    {

        currentUserId = debug_input.text.Trim();

        PlayerPrefs.SetString("CurrentUserId", currentUserId);
        PlayerPrefs.Save();

        StartCoroutine(GetItemsForRegularLoad(currentUserId));
        debug_text.text = "Uusi k�ytt�j�n ID asetettu: " + currentUserId;

    }
    public void sulje_debug_sivu()
    {
        debug_sivu.SetActive(false);
    }


    public void aseta_id()
    {
        string newId = aseta_id_input.text.Trim();
        string oneTimeKey = one_time_key_input.text.Trim(); // Tilap�inen 8-numeroinen avain

        if (string.IsNullOrEmpty(newId) || newId.Length != 64)
        {
            varoitus.text = "ID:n tulee olla 64 merkki� pitk� ja ei saa olla tyhj�.";
            return;
        }

        // T�RKE��: Jos k�ytt�j� antoi tilap�isen avaimen, yrit�mme siirt�� tiedot
        if (!string.IsNullOrEmpty(oneTimeKey) && oneTimeKey.Length == 8)
        {
            varoitus.text = "Yritet��n ladata tiedot tilap�isell� avaimella...";

            // Aseta ID paikallisesti ja tallenna
            currentUserId = newId;
            PlayerPrefs.SetString("CurrentUserId", currentUserId);
            PlayerPrefs.Save();

            // Yrit� hakea tiedot avaimella
            StartCoroutine(GetItemsWithTransferKey(currentUserId, oneTimeKey));
        }
        else if (!string.IsNullOrEmpty(oneTimeKey) && oneTimeKey.Length != 8)
        {
            varoitus.text = "Siirtoavaimen tulee olla tasan 8 numeroa pitk�.";
            return;
        }
        else
        {
            // Jos avainta ei annettu, aseta vain ID ja lataa olemassa oleva data (jos l�ytyy)
            StartCoroutine(CheckUserIdExists(newId, (exists) =>
            {
                currentUserId = newId;
                PlayerPrefs.SetString("CurrentUserId", currentUserId);
                PlayerPrefs.Save();

                if (exists)
                {
                    // Ladataan olemassa oleva data uudella/asetetulla ID:ll� ilman siirtoavainta
                    varoitus.text = "ID asetettu. Ladataan olemassa oleva data...";
                    StartCoroutine(GetItemsForRegularLoad(currentUserId));
                }
                else
                {
                    varoitus.text = "Uusi ID asetettu onnistuneesti. Aloitetaan tyhj�n�.";
                }

                Debug.Log($"Uusi k�ytt�j�n ID asetettu: {currentUserId}");
                aseta_id_paneli.SetActive(false);
                // Lataus t�ss� ei ole v�ltt�m�t�n, jos GetItemsForRegularLoad lataa tiedot heti
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            }));
        }
    }

    public void sulje_id_asetus()
    {
        aseta_id_paneli.SetActive(false);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Kutsu korutiinia, joka hoitaa k�ytt�j�n ID:n lataamisen/generoinnin ja datan latauksen
        StartCoroutine(InitializeUserAndLoadData());

        // Varmista, ett� paneelit ovat oikein
        canvas.SetActive(false);
        canvas1.SetActive(true);
        nayta_id_paneli.SetActive(false);
        aseta_id_paneli.SetActive(false);
    }

    /// <summary>
    /// Lataa tallennetun ID:n tai luo uuden. Jos ID on tallessa, yritet��n ladata data.
    /// </summary>
    IEnumerator InitializeUserAndLoadData()
    {
        string storedUserId = PlayerPrefs.GetString("CurrentUserId", "");
        bool isNewUser = string.IsNullOrEmpty(storedUserId);
        bool idExistsOnServer = false;

        if (isNewUser)
        {
            // Generoi uusi ID, jos ei ole tallennettuna
            string generatedId = GenerateRandomString(64);
            Debug.Log($"Ei tallennettua k�ytt�j�n ID:t�. Generoidaan uusi: {generatedId}");

            // Tarkista, onko ID jo olemassa (ep�todenn�k�ist�, mutta varmistetaan)
            yield return StartCoroutine(CheckUserIdExists(generatedId, (exists) => idExistsOnServer = exists));

            if (idExistsOnServer)
            {
                Debug.LogWarning("Generoitunut ID oli yll�tt�en jo k�yt�ss�! Generoidaan toinen.");
                generatedId = GenerateRandomString(64);
            }

            currentUserId = generatedId;
            PlayerPrefs.SetString("CurrentUserId", currentUserId);
            PlayerPrefs.Save();
            Debug.Log($"Uusi k�ytt�j�n ID tallennettu: {currentUserId}");
        }
        else
        {
            currentUserId = storedUserId;
            Debug.Log($"Latautunut k�ytt�j�n ID: {currentUserId}");

            // UUSI LATAUSLOGIIKKA: Lataa data heti, kun vanha ID l�ytyy (ei vaadi siirtoavainta)
            yield return StartCoroutine(GetItemsForRegularLoad(currentUserId));
        }

        // HUOM: Vanha GetItems-kutsu (tyhj�ll� avaimella) poistettu, koska se ep�onnistui aina (400 Bad Request).
    }


    /// <summary>
    /// Generoi satunnaisen merkkijonon (64 merkki�).
    /// </summary>
    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringBuilder = new StringBuilder(length);
        using (var rng = new RNGCryptoServiceProvider())
        {
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[bytes[i] % chars.Length]);
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// L�hett�� pyynn�n palvelimelle luodakseen tilap�isen siirtoavaimen (POST /generate_transfer_key).
    /// </summary>
    IEnumerator GenerateTransferKey(string userId)
    {
        string url = baseUrl + "/generate_transfer_key";

        var payloadData = new Dictionary<string, string>
        {
            { "user_id", userId }
        };
        string jsonPayload = JsonConvert.SerializeObject(payloadData);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Virhe avaimen luonnissa: {webRequest.error}");
                generated_key_text.text = "Virhe avaimen luonnissa!";
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                JSONNode jsonResponse = JSON.Parse(responseText);

                currentOneTimeKey = jsonResponse["one_time_key"]?.Value ?? "";
                int expirySeconds = jsonResponse["expires_in_seconds"].AsInt;
                keyExpiryTime = Time.time + expirySeconds;

                generated_key_text.text = $"Avain: <color=yellow>{currentOneTimeKey}</color>";
                Debug.Log($"Tilap�inen siirtoavain luotu: {currentOneTimeKey}. Voimassa {expirySeconds}s.");

                // Aloita ajastin, joka p�ivitt�� vanhenemisajan teksti�
                StartCoroutine(UpdateExpiryDisplay(expirySeconds));
            }
        }
    }

    /// <summary>
    /// P�ivitt�� vanhenemisajan n�yt�n ajastimella (15 minuutin aikaraja).
    /// </summary>
    IEnumerator UpdateExpiryDisplay(int totalSeconds)
    {
        float startTime = Time.time;
        float expiryTime = startTime + totalSeconds;

        while (Time.time < expiryTime && aseta_id_paneli.activeSelf)
        {
            float remainingTime = expiryTime - Time.time;
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

            string colorTag = remainingTime > 60 ? "<color=green>" : "<color=red>";
            key_expiry_text.text = $"Voimassa: {colorTag}{minutes:00}:{seconds:00}</color>";

            yield return new WaitForSeconds(1f);
        }

        if (Time.time >= expiryTime)
        {
            key_expiry_text.text = "<color=red>Avain vanhentunut!</color>";
            currentOneTimeKey = ""; // Tyhjenn� avain paikallisesti
        }
    }


    IEnumerator CheckUserIdExists(string userIdToCheck, System.Action<bool> callback)
    {
        string url = baseUrl + "/check_user/" + userIdToCheck;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Virhe k�ytt�j�n ID:n tarkistuksessa: {webRequest.error}");
                callback?.Invoke(false);
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                JSONNode jsonResponse = JSON.Parse(responseText);
                bool exists = jsonResponse["exists"].AsBool;
                Debug.Log($"K�ytt�j�n ID '{userIdToCheck}' olemassaolo palvelimella: {exists}");
                callback?.Invoke(exists);
            }
        }
    }

    // --- CRUD Toiminnot (Add, Get, Delete) ---

    public void NewItem()
    {
        if (string.IsNullOrWhiteSpace(nimi1.text))
        {
            Debug.LogWarning("Esineen nimi ei voi olla tyhj�.");
            return;
        }

        StartCoroutine(SendItemToServer(
            currentUserId,
            nimi1.text,
            numero1.text,
            lisatieto1.text,
            maara1.text,
            verkkosivu1.text
        ));

        nimi1.text = "";
        numero1.text = "";
        lisatieto1.text = "";
        maara1.text = "";
        verkkosivu1.text = "";

        CloseAddItemForm();
    }

    public void OpenAddItemForm()
    {
        canvas.SetActive(true);
        canvas1.SetActive(false);
    }

    public void CloseAddItemForm()
    {
        canvas.SetActive(false);
        canvas1.SetActive(true);
    }

    public void poista_tuote(lluo tavara)
    {
        if (tavara.itemId != 0)
        {
            StartCoroutine(DeleteItemFromServer(currentUserId, tavara.itemId, tavara));
        }
        else
        {
            Debug.LogWarning("Esineell� ei ole ID:t�, ei voida poistaa palvelimelta. Poistetaan vain paikallisesti.");
            tavara_lista.Remove(tavara);
            Destroy(tavara.gameObject);
        }
    }

    IEnumerator SendItemToServer(string userId, string nimi, string numero, string lisatieto, string maara, string verkkosivu)
    {
        string url = baseUrl + "/add_item";

        var payloadData = new Dictionary<string, string>
        {
            { "user_id", userId },
            { "nimi", nimi },
            { "numero", numero },
            { "lisatieto", lisatieto },
            { "maara", maara },
            { "verkkosivu", verkkosivu }
        };
        string jsonPayload = JsonConvert.SerializeObject(payloadData);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";
                Debug.LogError($"Virhe esineen l�hetyksess�: {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                Debug.Log("Esine l�hetetty - Vastaus: " + webRequest.downloadHandler.text);
                // KORJAUS: P�ivit� lista onnistuneen l�hetyksen j�lkeen s��nn�llisell� latauksella
                yield return StartCoroutine(GetItemsForRegularLoad(currentUserId));
            }
        }
    }

    /// <summary>
    /// UUSI: Hakee esineet palvelimelta s��nn�llisess� k�yt�ss� ilman siirtoavainta (GET /get_all_items_by_id/{user_id}).
    /// </summary>
    IEnumerator GetItemsForRegularLoad(string userId)
    {
        // HUOM: Oletetaan, ett� palvelimella on p��tepiste s��nn�lliseen GET-hakuun ID:ll�.
        string url = baseUrl + "/get_all_items_by_id/" + userId;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";
                Debug.LogError($"Virhe esineiden haussa (S��nn�llinen haku): {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                Debug.Log("Esineet vastaanotettu (S��nn�llinen haku): " + responseText);
                ProcessItemsResponse(responseText);
            }
        }
    }

    /// <summary>
    /// Hakee esineet palvelimelta tilap�isell� avaimella varmennettuna (POST /get_items).
    /// </summary>
    IEnumerator GetItemsWithTransferKey(string userId, string oneTimeKey)
    {
        if (string.IsNullOrEmpty(oneTimeKey))
        {
            Debug.LogWarning("GetItemsWithTransferKey: Tilap�inen avain puuttuu. Ei l�hetet� pyynt��.");
            ClearItemsList();
            yield break;
        }

        string url = baseUrl + "/get_items";

        var payloadData = new Dictionary<string, string>
        {
            { "user_id", userId },
            { "one_time_key", oneTimeKey } // L�hetet��n tilap�inen avain
        };
        string jsonPayload = JsonConvert.SerializeObject(payloadData);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";

                if (webRequest.responseCode == 403)
                {
                    Debug.LogError("VIRHE (403): Autentikaatio ep�onnistui. Avain puuttuu/v��r�/vanhentunut. Vastaus: " + responseText);
                    varoitus.text = "Tietojen lataus ep�onnistui: Avain virheellinen tai vanhentunut.";
                }
                else
                {
                    Debug.LogError($"Virhe esineiden haussa (Siirtoavain): {webRequest.error} | Vastaus: {responseText}");
                }
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                Debug.Log("Esineet vastaanotettu (Siirtoavain): " + responseText);

                if (aseta_id_paneli.activeSelf)
                {
                    varoitus.text = "Tiedot ladattiin onnistuneesti! Sulje paneeli jatkaaksesi.";
                    one_time_key_input.text = ""; // Tyhjenn� avainkentt� onnistumisen j�lkeen
                }

                ProcessItemsResponse(responseText);
            }
        }
    }

    /// <summary>
    /// Yksinkertaistettu logiikka vastausdatan k�sittelyyn (k�ytet��n sek� s��nn�llisess� latauksessa ett� siirtoavaimella).
    /// </summary>
    private void ProcessItemsResponse(string responseText)
    {
        ClearItemsList();

        if (string.IsNullOrWhiteSpace(responseText) || responseText == "[]")
        {
            Debug.LogWarning("Palvelimelta saatu vastaus oli tyhj� []. Ei esineit� ladattavaksi.");
            return;
        }

        try
        {
            JSONNode jsonResponse = JSON.Parse(responseText);

            if (jsonResponse.IsArray)
            {
                foreach (JSONNode itemNode in jsonResponse.AsArray)
                {
                    int id = itemNode["id"].AsInt;
                    string nimi = itemNode["nimi"]?.Value ?? "";
                    string numero = itemNode["numero"]?.Value ?? "";
                    string lisatieto = itemNode["lisatieto"]?.Value ?? "";
                    string maara = itemNode["maara"]?.Value ?? "";
                    string verkkosivu = itemNode["verkkosivu"]?.Value ?? "";

                    // HUOM: Muuttuja "muuttuja" on esineen prefab
                    lluo uusikuva = Instantiate(muuttuja, ruudukkoja);
                    uusikuva.Valmistele(id, nimi, numero, lisatieto, maara, verkkosivu);
                    tavara_lista.Add(uusikuva);
                }
            }
            else
            {
                Debug.LogWarning("Palvelimen vastaus ei ollut odotettu JSON-taulukko: " + responseText);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Virhe JSON-vastauksen parsinnassa: {e.Message}. Vastaus: {responseText}");
        }
    }


    IEnumerator DeleteItemFromServer(string userId, int itemId, lluo tavaraToDelete)
    {
        string url = baseUrl + "/delete_item/" + userId + "/" + itemId;

        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";
                Debug.LogError($"Virhe esineen poistossa: {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Palvelin ei palauttanut vastausdataa.";
                Debug.Log("Esine poistettu - Vastaus: " + responseText);

                if (webRequest.responseCode == 200)
                {
                    tavara_lista.Remove(tavaraToDelete);
                    Destroy(tavaraToDelete.gameObject);
                }
                else
                {
                    Debug.LogWarning($"Poistopyynt� ei palauttanut 200 OK. Statuskoodi: {webRequest.responseCode}. Vastaus: {responseText}");
                }
            }
        }
    }

    private void ClearItemsList()
    {
        foreach (lluo item in tavara_lista.ToList())
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        tavara_lista.Clear();
    }
}
