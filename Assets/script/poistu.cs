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

[System.Serializable]
public class ItemData
{
    public int id;
    public string nimi;
    public string numero;
    public string lisatieto;
    public string maara;
    public string verkkosivu;
    public bool onkoJonossa;
}

public class poistu : MonoBehaviour
{
    public lluo muuttuja;
    public GameObject canvas;
    public GameObject canvas1;

    [Header("Input Fields")]
    public InputField nimi1;
    public InputField numero1;
    public InputField lisatieto1;
    public InputField maara1;
    public InputField verkkosivu1;

    [Header("ID & Siirtoavain UI")]
    public GameObject aseta_id_paneli;
    public GameObject nayta_id_paneli;
    public TextMeshProUGUI nayta_id_text;
    public TMP_InputField aseta_id_input;
    public TMP_InputField one_time_key_input;
    public TextMeshProUGUI varoitus;
    public TextMeshProUGUI generated_key_text;
    public TextMeshProUGUI key_expiry_text;

    public Transform ruudukkoja;
    public List<lluo> tavara_lista = new List<lluo>();
    private List<ItemData> offline_data_lista = new List<ItemData>();

    public static poistu Instance;
    private string currentUserId;
    private string currentOneTimeKey;

    [Header("Server Config")]
    public string baseUrl = "https://4085df7d84ea.ngrok-free.app";

    private void Awake() { Instance = this; }

    private void Start()
    {
        LataaOfflineMuistista();

        currentUserId = PlayerPrefs.GetString("CurrentUserId", "");
        if (string.IsNullOrEmpty(currentUserId))
        {
            currentUserId = GenerateRandomString(64);
            PlayerPrefs.SetString("CurrentUserId", currentUserId);
            PlayerPrefs.Save();
        }

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(SynkronoiKaikki());
        }

        // Varmista alkutila
        if (aseta_id_paneli) aseta_id_paneli.SetActive(false);
        if (nayta_id_paneli) nayta_id_paneli.SetActive(false);
    }

    // --- SYNKRONOINTI ---

    IEnumerator SynkronoiKaikki()
    {
        Debug.Log("Synkronoidaan...");
        var jonossaOlevat = offline_data_lista.Where(x => x.onkoJonossa).ToList();
        foreach (var tuote in jonossaOlevat)
        {
            yield return StartCoroutine(SendItemToServer(currentUserId, tuote));
        }
        yield return StartCoroutine(GetItemsForRegularLoad(currentUserId));
    }

    // --- CRUD ---

    public void NewItem()
    {
        if (string.IsNullOrWhiteSpace(nimi1.text)) return;

        ItemData uusi = new ItemData
        {
            id = 0,
            nimi = nimi1.text,
            numero = numero1.text,
            lisatieto = lisatieto1.text,
            maara = maara1.text,
            verkkosivu = verkkosivu1.text,
            onkoJonossa = true
        };

        offline_data_lista.Add(uusi);
        TallennaOfflineMuistiin();
        PaivitaRuutu();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(SendItemToServer(currentUserId, uusi));
        }

        TyhjennaKentat();
        CloseAddItemForm();
    }

    public void poista_tuote(lluo tavara)
    {
        offline_data_lista.RemoveAll(x => (x.id != 0 && x.id == tavara.itemId) ||
                                          (x.nimi == tavara.nimiTeksti.text && x.numero == tavara.numeroTeksti.text));
        TallennaOfflineMuistiin();

        if (tavara.itemId != 0 && Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(DeleteItemFromServer(currentUserId, tavara.itemId));
        }

        tavara_lista.Remove(tavara);
        Destroy(tavara.gameObject);
    }

    // --- VERKKOPYYNNÖT ---

    IEnumerator SendItemToServer(string userId, ItemData item)
    {
        string url = baseUrl + "/add_item";
        var payload = new Dictionary<string, string> {
            { "user_id", userId }, { "nimi", item.nimi }, { "numero", item.numero },
            { "lisatieto", item.lisatieto }, { "maara", item.maara }, { "verkkosivu", item.verkkosivu }
        };
        string json = JsonConvert.SerializeObject(payload);

        using (UnityWebRequest webRequest = CreatePostRequest(url, json))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                item.onkoJonossa = false;
                TallennaOfflineMuistiin();
            }
        }
    }

    IEnumerator GetItemsForRegularLoad(string userId)
    {
        string url = baseUrl + "/get_all_items_by_id/" + userId;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var uusiLista = JsonConvert.DeserializeObject<List<ItemData>>(webRequest.downloadHandler.text);
                var lahettamattomat = offline_data_lista.Where(x => x.onkoJonossa).ToList();
                offline_data_lista = uusiLista;
                offline_data_lista.AddRange(lahettamattomat);
                TallennaOfflineMuistiin();
                PaivitaRuutu();
            }
        }
    }

    IEnumerator DeleteItemFromServer(string userId, int itemId)
    {
        string url = baseUrl + "/delete_item/" + userId + "/" + itemId;
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            yield return webRequest.SendWebRequest();
        }
    }

    // --- SIIRTOAVAIN LOGIIKKA ---

    public void PyydaSiirtoavain()
    {
        StartCoroutine(GenerateTransferKey(currentUserId));
    }

    IEnumerator GenerateTransferKey(string userId)
    {
        string url = baseUrl + "/generate_transfer_key";
        var payload = new Dictionary<string, string> { { "user_id", userId } };
        string json = JsonConvert.SerializeObject(payload);

        using (UnityWebRequest webRequest = CreatePostRequest(url, json))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                JSONNode jsonResponse = JSON.Parse(webRequest.downloadHandler.text);
                currentOneTimeKey = jsonResponse["one_time_key"]?.Value;
                int expiry = jsonResponse["expires_in_seconds"].AsInt;
                generated_key_text.text = $"Avain: <color=yellow>{currentOneTimeKey}</color>";
                StartCoroutine(UpdateExpiryDisplay(expiry));
            }
        }
    }

    IEnumerator UpdateExpiryDisplay(int totalSeconds)
    {
        float expiryTime = Time.time + totalSeconds;
        while (Time.time < expiryTime)
        {
            float rem = expiryTime - Time.time;
            key_expiry_text.text = $"Voimassa: {Mathf.FloorToInt(rem / 60):00}:{Mathf.FloorToInt(rem % 60):00}";
            yield return new WaitForSeconds(1f);
        }
        key_expiry_text.text = "Vanhentunut";
    }

    public void aseta_id()
    {
        string newId = aseta_id_input.text.Trim();
        string key = one_time_key_input.text.Trim();

        if (newId.Length != 64) { varoitus.text = "ID:n pituus virheellinen."; return; }

        currentUserId = newId;
        PlayerPrefs.SetString("CurrentUserId", currentUserId);
        PlayerPrefs.Save();

        if (!string.IsNullOrEmpty(key))
            StartCoroutine(GetItemsWithTransferKey(currentUserId, key));
        else
            StartCoroutine(GetItemsForRegularLoad(currentUserId));
    }

    IEnumerator GetItemsWithTransferKey(string userId, string key)
    {
        string url = baseUrl + "/get_items";
        var payload = new Dictionary<string, string> { { "user_id", userId }, { "one_time_key", key } };
        using (UnityWebRequest webRequest = CreatePostRequest(url, JsonConvert.SerializeObject(payload)))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                varoitus.text = "Tiedot haettu avaimella!";
                var uusiLista = JsonConvert.DeserializeObject<List<ItemData>>(webRequest.downloadHandler.text);
                offline_data_lista = uusiLista;
                TallennaOfflineMuistiin();
                PaivitaRuutu();
            }
            else varoitus.text = "Virhe siirtoavaimessa.";
        }
    }

    // --- APUMETODIT ---

    private void TallennaOfflineMuistiin() { PlayerPrefs.SetString("OfflineTallennus", JsonConvert.SerializeObject(offline_data_lista)); PlayerPrefs.Save(); }
    private void LataaOfflineMuistista()
    {
        string json = PlayerPrefs.GetString("OfflineTallennus", "");
        if (!string.IsNullOrEmpty(json)) { offline_data_lista = JsonConvert.DeserializeObject<List<ItemData>>(json); PaivitaRuutu(); }
    }

    private void PaivitaRuutu()
    {
        foreach (var t in tavara_lista) if (t != null) Destroy(t.gameObject);
        tavara_lista.Clear();
        foreach (var d in offline_data_lista)
        {
            lluo uusikuva = Instantiate(muuttuja, ruudukkoja);
            uusikuva.Valmistele(d.id, d.nimi, d.numero, d.lisatieto, d.maara, d.verkkosivu);
            tavara_lista.Add(uusikuva);
        }
    }

    private UnityWebRequest CreatePostRequest(string url, string json)
    {
        UnityWebRequest req = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        return req;
    }

    private void TyhjennaKentat() { nimi1.text = ""; numero1.text = ""; lisatieto1.text = ""; maara1.text = ""; verkkosivu1.text = ""; }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[4];
            while (res.Length < length)
            {
                rng.GetBytes(uintBuffer);
                uint num = System.BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(chars[(int)(num % (uint)chars.Length)]);
            }
        }
        return res.ToString();
    }

    public void OpenAddItemForm() { canvas.SetActive(true); canvas1.SetActive(false); }
    public void CloseAddItemForm() { canvas.SetActive(false); canvas1.SetActive(true); }
    public void nayta_id() { nayta_id_paneli.SetActive(true); nayta_id_text.text = currentUserId; }
    public void piilota_id() { nayta_id_paneli.SetActive(false); }
    public void avaa_id_asetus() { aseta_id_paneli.SetActive(true); varoitus.text = ""; }
    public void sulje_id_asetus() { aseta_id_paneli.SetActive(false); }
}