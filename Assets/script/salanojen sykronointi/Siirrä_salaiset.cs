using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Siirrä_salaiset : MonoBehaviour
{
    // Noudetaan arvot muista skripteistäsi
    private string salasana => salasananvaihto.salistatlalla;
    private string varasalis => salasananvaihto.varasalistalla;
    private string sahkoposti => Kaksi_vaiheinen_tunnistus.tili;
    private string tfk => Kaksi_vaiheinen_tunnistus.secret;

    public void vie()
    {
        StartCoroutine(PostVienti());
    }

    // NYT VAATII SEKÄ ID:N ETTÄ KOODIN
    public void tuo(string syotettyUserId, string syotettyKoodi)
    {
        StartCoroutine(PostTuonti(syotettyUserId, syotettyKoodi));
    }

    IEnumerator PostVienti()
    {
        string uid = poistu.Instance.currentUserId;
        string url = poistu.Instance.baseUrl + "/vienti";

        // Luodaan JSON-objekti vientiä varten
        var data = new
        {
            user_id = uid,
            pin_code = salasana,
            backup_pin = varasalis,
            two_factor_key = tfk,
            backup_password = sahkoposti
        };

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        yield return SendRequest(url, json);
    }

    IEnumerator PostTuonti(string uid, string koodi)
    {
        string url = poistu.Instance.baseUrl + "/tuonti";

        // Luodaan JSON-objekti jossa on User ID ja siirtokoodi
        var data = new
        {
            user_id = uid,
            one_time_key = koodi
        };

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        using (UnityWebRequest request = CreateWebRequest(url, json))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Puretaan vastaus
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(request.downloadHandler.text);

                // Päivitetään paikalliset muuttujat tuoduilla arvoilla
                salasananvaihto.salistatlalla = response["pin_code"];
                salasananvaihto.varasalistalla = response["backup_pin"];
                Kaksi_vaiheinen_tunnistus.tili = response["backup_password"];
                Kaksi_vaiheinen_tunnistus.secret = response["two_factor_key"];

                Debug.Log("Tuonti onnistui User ID:llä: " + uid);
            }
            else
            {
                Debug.LogError("Tuonti epäonnistui: " + request.downloadHandler.text);
            }
        }
    }

    // Apufunktiot verkkopyyntöihin
    IEnumerator SendRequest(string url, string json)
    {
        using (UnityWebRequest req = CreateWebRequest(url, json))
        {
            yield return req.SendWebRequest();
        }
    }

    UnityWebRequest CreateWebRequest(string url, string json)
    {
        UnityWebRequest req = new UnityWebRequest(url, "POST");
        byte[] body = Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        return req;
    }
}