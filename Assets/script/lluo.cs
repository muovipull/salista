using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro-komponenteille
using UnityEngine.UI; // InputField- ja Button-komponenteille

[System.Serializable]
public class lluo : MonoBehaviour
{
    // N�ytett�v�t tekstikent�t
    public TextMeshProUGUI nimiTeksti;
    public TextMeshProUGUI numeroTeksti;
    public TextMeshProUGUI lisatietoTeksti;
    public TextMeshProUGUI maaraTeksti;
    public TextMeshProUGUI verkkosivuTeksti;

    public int itemId; // T�RKE�: T�h�n tallennetaan esineen tietokanta-ID

    // Poiston vahvistus- ja PIN-komponentit
    public GameObject poista_hyvaksy; // Vahvistusdialogin p��objekti
    public InputField salis; // PIN-koodin sy�tt�kentt�

    // Valmistele-metodi asettaa tekstikenttien arvot ja ID:n
    public void Valmistele(int id, string nimi, string numero, string lisatieto, string maara, string verkkosivu)
    {
        itemId = id; // Aseta tietokanta-ID
        if (nimiTeksti != null) nimiTeksti.text = nimi;
        if (numeroTeksti != null) numeroTeksti.text = numero;
        if (lisatietoTeksti != null) lisatietoTeksti.text = lisatieto;
        if (maaraTeksti != null) maaraTeksti.text = maara;
        if (verkkosivuTeksti != null) verkkosivuTeksti.text = verkkosivu;
    }

    // Ylikuormitusmetodi, jos ID:t� ei ole saatavilla aluksi (esim. uusi lis�tt�v� esine)
    public void Valmistele(string nimi, string numero, string lisatieto, string maara, string verkkosivu)
    {
        Valmistele(0, nimi, numero, lisatieto, maara, verkkosivu); // Aseta ID nollaksi
    }

    // Tuotteen poiston aloitus
    public void poista_tuote() // T�t� kutsutaan esim. Delete-napista
    {
        if (poista_hyvaksy != null)
        {
            poista_hyvaksy.SetActive(true); // N�yt� vahvistusdialogi
            if (salis != null) salis.text = ""; // Tyhjenn� PIN-kentt�
        }
        else
        {
            Debug.LogWarning("Poista_hyvaksy GameObject not assigned in Inspector for lluo: " + gameObject.name);
            // Jos vahvistusdialogia ei ole, voit ohittaa PIN-tarkistuksen tai kutsua poiston suoraan (ei suositella ilman varmistusta)
            // poistu.Instance.poista_tuote(this);
        }
    }

    // Vahvistus PIN-koodin tarkistuksen j�lkeen
    public void kylla() // Sopii poistaa -kutsutaan vahvistusdialogin Kyll�-napista
    {
        // Oletetaan, ett� 'salasananvaihto' on olemassa ja siin� on staattinen 'salistatlalla' PIN-koodi
        if (salis != null && salis.text == salasananvaihto.salistatlalla)
        {
            if (poistu.Instance != null)
            {
                poistu.Instance.poista_tuote(this); // Pyyd� poistu-skripti� poistamaan t�m� esine
            }
            if (poista_hyvaksy != null)
            {
                poista_hyvaksy.SetActive(false); // Piilota vahvistusdialogi
            }
            // �L� tuhota GameObjectia t�ss�! poistu-skripti tekee sen, kun se on saanut vahvistuksen palvelimelta.
            // Destroy(gameObject); // T�m� rivi poistettu t��lt�
        }
        else
        {
            Debug.LogWarning("Virheellinen PIN-koodi tai 'salis' InputField puuttuu.");
            // Voit n�ytt�� virheviestin k�ytt�j�lle
        }
    }

    // Peruuta poisto
    public void ei_kay() // Kutsutaan vahvistusdialogin Peruuta-napista
    {
        if (poista_hyvaksy != null)
        {
            poista_hyvaksy.SetActive(false); // Piilota vahvistusdialogi
        }
    }
}