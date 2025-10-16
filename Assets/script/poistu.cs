using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using TMPro;
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
    [Header("Aseta id")]

    public GameObject aseta_id_paneli;
    public InputField aseta_id_input;
    public TextMeshProUGUI varoitus;

    [Header("Input Fields (Add New Item)")]
    public InputField nimi1;
    public InputField numero1;
    public InputField lisatieto1;
    public InputField maara1;
    public InputField verkkosivu1;

    private string currentUserId;

    public Transform ruudukkoja;

    public List<lluo> tavara_lista = new List<lluo>();

    public static poistu Instance;

    private readonly string baseUrl = "http://192.168.101.147:5001";




    public void nayta_id()
    {
        nayta_id_paneli.SetActive(true);
        nayta_id_text.text = currentUserId; // Asetetaan nykyinen käyttäjän ID näkyviin

    }
    public void piilota_id()
    {
        nayta_id_paneli.SetActive(false);
    }

    public void avaa_id_asetus()
    {
        aseta_id_paneli.SetActive(true);
        aseta_id_input.text = currentUserId; // Asetetaan nykyinen käyttäjän ID syöttökenttään
    }

    public void aseta_id()
    {
        string newId = aseta_id_input.text.Trim();
        if (string.IsNullOrEmpty(newId) || newId.Length != 64)
        {
            varoitus.text = "ID:n tulee olla 64 merkkiä pitkä ja ei saa olla tyhjä.";
            return;
        }
        // **KORJAUS TÄSSÄ:** Odotetaan CheckUserIdExists-korutiinin valmistumista
        StartCoroutine(CheckUserIdExists(newId, (exists) =>
        {
            if (!exists)
            {
                currentUserId = newId;
                PlayerPrefs.SetString("CurrentUserId", currentUserId);
                PlayerPrefs.Save();
                varoitus.text = "Uusi ID asetettu onnistuneesti. Sovellus";
                Debug.Log($"Uusi käyttäjän ID asetettu: {currentUserId}");
                aseta_id_paneli.SetActive(false);
                string currentSceneName = SceneManager.GetActiveScene().name;

                // Lataa kohtaus uudelleen nimellä
                SceneManager.LoadScene(currentSceneName);

            }
            else
            {
                varoitus.text = "Annettu ID on jo käytössä. Valitse toinen ID.";
                Debug.LogWarning("Annettu ID on jo käytössä. Valitse toinen ID.");
            }
        }));
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
        // Kutsu korutiinia, joka hoitaa käyttäjän ID:n lataamisen/generoinnin ja datan haun
        StartCoroutine(InitializeUserAndLoadData());
    }

    // KORJATTU METODI: Yhdistetty käyttäjän ID:n käsittely ja datan lataus
    IEnumerator InitializeUserAndLoadData()
    {
        string storedUserId = PlayerPrefs.GetString("CurrentUserId", "");
        bool isNewUser = string.IsNullOrEmpty(storedUserId);
        bool idExistsOnServer = false; // Muuttuja, johon tallennetaan tarkistuksen tulos

        if (isNewUser)
        {
            // Generoi uusi ID, jos ei ole tallennettuna
            string generatedId = GenerateRandomString(64);
            Debug.Log($"Ei tallennettua käyttäjän ID:tä. Generoidaan uusi: {generatedId}");

            // **KORJAUS TÄSSÄ:** Odotetaan CheckUserIdExists-korutiinin valmistumista
            // ja asetetaan idExistsOnServer-muuttuja callbackin kautta.
            yield return StartCoroutine(CheckUserIdExists(generatedId, (exists) => idExistsOnServer = exists));

            // Jos ID oli jo olemassa (erittäin epätodennäköistä), generoi uusi ID.
            if (idExistsOnServer)
            {
                Debug.LogWarning("Generoitunut ID oli yllättäen jo käytössä! Generoidaan toinen.");
                generatedId = GenerateRandomString(64); // Generoi toinen kerta
                // Tässä kohtaa emme enää tarkista toista kertaa palvelimelta, koska todennäköisyys
                // toiselle törmäykselle on käytännössä olematon 64-merkkisellä.
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
        }

        // Nyt kun currentUserId on varmasti asetettu ja (toivottavasti) uniikki, ladataan tiedot
        StartCoroutine(GetItems(currentUserId));
    }


    // Pysyy samana
    IEnumerator CheckUserIdExists(string userIdToCheck, System.Action<bool> callback)
    {
        string url = baseUrl + "/check_user/" + userIdToCheck;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Virhe käyttäjän ID:n tarkistuksessa: {webRequest.error}");
                callback?.Invoke(true); // Oletetaan varmuuden vuoksi, että ID on olemassa virheen sattuessa
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

    // Pysyy samana
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

    // --- Muut metodit (NewItem, OpenAddItemForm, CloseAddItemForm, poista_tuote, SendItemToServer, GetItems, DeleteItemFromServer, ClearItemsList) pysyvät samoina ---

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
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Palvelin ei palauttanut vastausdataa.";
                Debug.Log("Esine lähetetty - Vastaus: " + responseText);

                ClearItemsList();
                StartCoroutine(GetItems(currentUserId));
            }
        }
    }

    IEnumerator GetItems(string userId)
    {
        string url = baseUrl + "/get_items/" + userId;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Ei vastausdataa";
                Debug.LogError($"Virhe esineiden haussa: {webRequest.error} | Vastaus: {responseText}");
            }
            else
            {
                string responseText = (webRequest.downloadHandler != null) ? webRequest.downloadHandler.text : "Palvelin ei palauttanut vastausdataa.";
                Debug.Log("Esineet vastaanotettu: " + responseText);
                ClearItemsList();

                if (string.IsNullOrWhiteSpace(responseText))
                {
                    Debug.LogWarning("Palvelimelta saatu vastaus oli tyhjä tai null. Ei esineitä ladattavaksi.");
                    yield break;
                }

                JSONNode jsonResponse = JSON.Parse(responseText);

                if (jsonResponse.IsArray)
                {
                    foreach (JSONNode itemNode in jsonResponse.AsArray)
                    {
                        int id = itemNode["id"].AsInt;
                        string nimi = itemNode["nimi"] ?? "";
                        string numero = itemNode["numero"] ?? "";
                        string lisatieto = itemNode["lisatieto"] ?? "";
                        string maara = itemNode["maara"] ?? "";
                        string verkkosivu = itemNode["verkkosivu"] ?? "";

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