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
    // UUSI: InputField siirtoavaimen syöttöön
    public TMP_InputField one_time_key_input;
    public TextMeshProUGUI varoitus;
    // UUSI: Tekstikenttä avaimen näyttämiseen
    public TextMeshProUGUI generated_key_text;
    public TextMeshProUGUI key_expiry_text; // Tekstikenttä vanhenemisajan näyttämiseen

    [Header("Input Fields (Add New Item)")]
    public InputField nimi1;
    public InputField numero1;
    public InputField lisatieto1;
    public InputField maara1;
    public InputField verkkosivu1;

    private string currentUserId;
    private string currentOneTimeKey; // Tilapäisesti luotu avain
    private float keyExpiryTime;      // Avaimen vanhenemisaika (Unix-aikaleima)

    public Transform ruudukkoja;

    public List<lluo> tavara_lista = new List<lluo>();

    public static poistu Instance;

    [Header("debug")]
    public GameObject debug_sivu;
    public TMP_InputField debug_input;
    public TextMeshProUGUI debug_text;
<<<<<<< HEAD
    // HUOM: Tarkista IP-osoite ja portti ennen k�ytt��! vaihda nykyisin editorissa

    [Header("server url")]

    public string baseUrl; 
=======
    // HUOM: Tarkista IP-osoite ja portti ennen käyttöä!
    private readonly string baseUrl = "https://4085df7d84ea.ngrok-free.app"; // julkaisu palvelin
    //private readonly string baseUrl = "http://192.168.101.107:5001"; //testi palvelin
>>>>>>> a6638add7ebd4ab340bcf62fbedf2652ab85b6f1

    // --- Käyttäjän ID:n ja avaimen hallinta ---

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
    /// Avaa paneelin, jolla käyttäjän ID ja tilapäinen siirtoavain voidaan asettaa.
    /// </summary>
    public void avaa_id_asetus()
    {
        aseta_id_paneli.SetActive(true);
        aseta_id_input.text = "";
        one_time_key_input.text = ""; // Tyhjennetään avainkenttä aina
        varoitus.text = "";
        generated_key_text.text = "Avainta ei pyydetty";
        key_expiry_text.text = "";
    }

    /// <summary>
    /// Pyytää palvelimelta uuden 8-numeroisen siirtoavaimen, joka on voimassa 15 min.
    /// Kutsutaan vanhalla laitteella, josta tiedot halutaan siirtää.
    /// </summary>
    public void PyydaSiirtoavain()
    {
        StartCoroutine(GenerateTransferKey(currentUserId));
    }

    /// <summary>
    /// Yrittää asettaa uuden ID:n ja ladata datan tilapäisellä siirtoavaimella.
    /// Kutsutaan uudella laitteella, johon tiedot halutaan siirtää.
    /// </summary>
    /// 
    public void avaa_debug_sivu()
    {
        debug_sivu.SetActive(true);
        
        debug_text.text = "Nykyinen käyttäjän ID: " + currentUserId;

        debug_input.text = "";
    }
    public void aseta_debug_id()
    {

        currentUserId = debug_input.text.Trim();

        PlayerPrefs.SetString("CurrentUserId", currentUserId);
        PlayerPrefs.Save();

        StartCoroutine(GetItemsForRegularLoad(currentUserId));
        debug_text.text = "Uusi käyttäjän ID asetettu: " + currentUserId;

    }
    public void sulje_debug_sivu()
    {
        debug_sivu.SetActive(false);
    }


    public void aseta_id()
    {
        string newId = aseta_id_input.text.Trim();
        string oneTimeKey = one_time_key_input.text.Trim(); // Tilapäinen 8-numeroinen avain

        if (string.IsNullOrEmpty(newId) || newId.Length != 64)
        {
            varoitus.text = "ID:n tulee olla 64 merkkiä pitkä ja ei saa olla tyhjä.";
            return;
        }

        // TÄRKEÄÄ: Jos käyttäjä antoi tilapäisen avaimen, yritämme siirtää tiedot
        if (!string.IsNullOrEmpty(oneTimeKey) && oneTimeKey.Length == 8)
        {
            varoitus.text = "Yritetään ladata tiedot tilapäisellä avaimella...";

            // Aseta ID paikallisesti ja tallenna
            currentUserId = newId;
            PlayerPrefs.SetString("CurrentUserId", currentUserId);
            PlayerPrefs.Save();

            // Yritä hakea tiedot avaimella
            StartCoroutine(GetItemsWithTransferKey(currentUserId, oneTimeKey));
        }
        else if (!string.IsNullOrEmpty(oneTimeKey) && oneTimeKey.Length != 8)
        {
            varoitus.text = "Siirtoavaimen tulee olla tasan 8 numeroa pitkä.";
            return;
        }
        else
        {
            // Jos avainta ei annettu, aseta vain ID ja lataa olemassa oleva data (jos löytyy)
            StartCoroutine(CheckUserIdExists(newId, (exists) =>
            {
                currentUserId = newId;
                PlayerPrefs.SetString("CurrentUserId", currentUserId);
                PlayerPrefs.Save();

                if (exists)
                {
                    // Ladataan olemassa oleva data uudella/asetetulla ID:llä ilman siirtoavainta
                    varoitus.text = "ID asetettu. Ladataan olemassa oleva data...";
                    StartCoroutine(GetItemsForRegularLoad(currentUserId));
                }
                else
                {
                    varoitus.text = "Uusi ID asetettu onnistuneesti. Aloitetaan tyhjänä.";
                }

                Debug.Log($"Uusi käyttäjän ID asetettu: {currentUserId}");
                aseta_id_paneli.SetActive(false);
                // Lataus tässä ei ole välttämätön, jos GetItemsForRegularLoad lataa tiedot heti
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
        // Kutsu korutiinia, joka hoitaa käyttäjän ID:n lataamisen/generoinnin ja datan latauksen
        StartCoroutine(InitializeUserAndLoadData());

        // Varmista, että paneelit ovat oikein
        canvas.SetActive(false);
        canvas1.SetActive(true);
        nayta_id_paneli.SetActive(false);
        aseta_id_paneli.SetActive(false);
    }

    /// <summary>
    /// Lataa tallennetun ID:n tai luo uuden. Jos ID on tallessa, yritetään ladata data.
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
            Debug.Log($"Ei tallennettua käyttäjän ID:tä. Generoidaan uusi: {generatedId}");

            // Tarkista, onko ID jo olemassa (epätodennäköistä, mutta varmistetaan)
            yield return StartCoroutine(CheckUserIdExists(generatedId, (exists) => idExistsOnServer = exists));

            if (idExistsOnServer)
            {
                Debug.LogWarning("Generoitunut ID oli yllättäen jo käytössä! Generoidaan toinen.");
                generatedId = GenerateRandomString(64);
            }

            currentUserId = generatedId;
            PlayerPrefs.SetString("CurrentUserId", currentUserId);
            PlayerPrefs.Save();
            Debug.Log($"Uusi käyttäjän ID tallennettu: {currentUserId}");
        }
        else
        {
            currentUserId = storedUserId;
            Debug.Log($"Latautunut käyttäjän ID: {currentUserId}");

            // UUSI LATAUSLOGIIKKA: Lataa data heti, kun vanha ID löytyy (ei vaadi siirtoavainta)
            yield return StartCoroutine(GetItemsForRegularLoad(currentUserId));
        }

        // HUOM: Vanha GetItems-kutsu (tyhjällä avaimella) poistettu, koska se epäonnistui aina (400 Bad Request).
    }


    /// <summary>
    /// Generoi satunnaisen merkkijonon (64 merkkiä).
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
    /// Lähettää pyynnön palvelimelle luodakseen tilapäisen siirtoavaimen (POST /generate_transfer_key).
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
                Debug.Log($"Tilapäinen siirtoavain luotu: {currentOneTimeKey}. Voimassa {expirySeconds}s.");

                // Aloita ajastin, joka päivittää vanhenemisajan tekstiä
                StartCoroutine(UpdateExpiryDisplay(expirySeconds));
            }
        }
    }

    /// <summary>
    /// Päivittää vanhenemisajan näytön ajastimella (15 minuutin aikaraja).
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
            currentOneTimeKey = ""; // Tyhjennä avain paikallisesti
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
                Debug.LogError($"Virhe käyttäjän ID:n tarkistuksessa: {webRequest.error}");
                callback?.Invoke(false);
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                JSONNode jsonResponse = JSON.Parse(responseText);
                bool exists = jsonResponse["exists"].AsBool;
                Debug.Log($"Käyttäjän ID '{userIdToCheck}' olemassaolo palvelimella: {exists}");
                callback?.Invoke(exists);
            }
        }
    }

    // --- CRUD Toiminnot (Add, Get, Delete) ---

    public void NewItem()
    {
        if (string.IsNullOrWhiteSpace(nimi1.text))
        {
            Debug.LogWarning("Esineen nimi ei voi olla tyhjä.");
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
            Debug.LogWarning("Esineellä ei ole ID:tä, ei voida poistaa palvelimelta. Poistetaan vain paikallisesti.");
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
                Debug.LogError($"Virhe esineen lähetyksessä: {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                Debug.Log("Esine lähetetty - Vastaus: " + webRequest.downloadHandler.text);
                // KORJAUS: Päivitä lista onnistuneen lähetyksen jälkeen säännöllisellä latauksella
                yield return StartCoroutine(GetItemsForRegularLoad(currentUserId));
            }
        }
    }

    /// <summary>
    /// UUSI: Hakee esineet palvelimelta säännöllisessä käytössä ilman siirtoavainta (GET /get_all_items_by_id/{user_id}).
    /// </summary>
    IEnumerator GetItemsForRegularLoad(string userId)
    {
        // HUOM: Oletetaan, että palvelimella on päätepiste säännölliseen GET-hakuun ID:llä.
        string url = baseUrl + "/get_all_items_by_id/" + userId;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";
                Debug.LogError($"Virhe esineiden haussa (Säännöllinen haku): {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "";
                Debug.Log("Esineet vastaanotettu (Säännöllinen haku): " + responseText);
                ProcessItemsResponse(responseText);
            }
        }
    }

    /// <summary>
    /// Hakee esineet palvelimelta tilapäisellä avaimella varmennettuna (POST /get_items).
    /// </summary>
    IEnumerator GetItemsWithTransferKey(string userId, string oneTimeKey)
    {
        if (string.IsNullOrEmpty(oneTimeKey))
        {
            Debug.LogWarning("GetItemsWithTransferKey: Tilapäinen avain puuttuu. Ei lähetetä pyyntöä.");
            ClearItemsList();
            yield break;
        }

        string url = baseUrl + "/get_items";

        var payloadData = new Dictionary<string, string>
        {
            { "user_id", userId },
            { "one_time_key", oneTimeKey } // Lähetetään tilapäinen avain
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
                    Debug.LogError("VIRHE (403): Autentikaatio epäonnistui. Avain puuttuu/väärä/vanhentunut. Vastaus: " + responseText);
                    varoitus.text = "Tietojen lataus epäonnistui: Avain virheellinen tai vanhentunut.";
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
                    one_time_key_input.text = ""; // Tyhjennä avainkenttä onnistumisen jälkeen
                }

                ProcessItemsResponse(responseText);
            }
        }
    }

    /// <summary>
    /// Yksinkertaistettu logiikka vastausdatan käsittelyyn (käytetään sekä säännöllisessä latauksessa että siirtoavaimella).
    /// </summary>
    private void ProcessItemsResponse(string responseText)
    {
        ClearItemsList();

        if (string.IsNullOrWhiteSpace(responseText) || responseText == "[]")
        {
            Debug.LogWarning("Palvelimelta saatu vastaus oli tyhjä []. Ei esineitä ladattavaksi.");
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
                    Debug.LogWarning($"Poistopyyntö ei palauttanut 200 OK. Statuskoodi: {webRequest.responseCode}. Vastaus: {responseText}");
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
