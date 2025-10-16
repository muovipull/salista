using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro-komponenteille
using UnityEngine.UI; // InputField- ja Button-komponenteille

[System.Serializable]
public class lluo : MonoBehaviour
{
    // Näytettävät tekstikentät
    public TextMeshProUGUI nimiTeksti;
    public TextMeshProUGUI numeroTeksti;
    public TextMeshProUGUI lisatietoTeksti;
    public TextMeshProUGUI maaraTeksti;
    public TextMeshProUGUI verkkosivuTeksti;

    public int itemId; // TÄRKEÄ: Tähän tallennetaan esineen tietokanta-ID

    // Poiston vahvistus- ja PIN-komponentit
    public GameObject poista_hyvaksy; // Vahvistusdialogin pääobjekti
    public InputField salis; // PIN-koodin syöttökenttä

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

    // Ylikuormitusmetodi, jos ID:tä ei ole saatavilla aluksi (esim. uusi lisättävä esine)
    public void Valmistele(string nimi, string numero, string lisatieto, string maara, string verkkosivu)
    {
        Valmistele(0, nimi, numero, lisatieto, maara, verkkosivu); // Aseta ID nollaksi
    }

    // Tuotteen poiston aloitus
    public void poista_tuote() // Tätä kutsutaan esim. Delete-napista
    {
        if (poista_hyvaksy != null)
        {
            poista_hyvaksy.SetActive(true); // Näytä vahvistusdialogi
            if (salis != null) salis.text = ""; // Tyhjennä PIN-kenttä
        }
        else
        {
            Debug.LogWarning("Poista_hyvaksy GameObject not assigned in Inspector for lluo: " + gameObject.name);
            // Jos vahvistusdialogia ei ole, voit ohittaa PIN-tarkistuksen tai kutsua poiston suoraan (ei suositella ilman varmistusta)
            // poistu.Instance.poista_tuote(this);
        }
    }

    // Vahvistus PIN-koodin tarkistuksen jälkeen
    public void kylla() // Sopii poistaa -kutsutaan vahvistusdialogin Kyllä-napista
    {
        // Oletetaan, että 'salasananvaihto' on olemassa ja siinä on staattinen 'salistatlalla' PIN-koodi
        if (salis != null && salis.text == salasananvaihto.salistatlalla)
        {
            if (poistu.Instance != null)
            {
                poistu.Instance.poista_tuote(this); // Pyydä poistu-skriptiä poistamaan tämä esine
            }
            if (poista_hyvaksy != null)
            {
                poista_hyvaksy.SetActive(false); // Piilota vahvistusdialogi
            }
            // ÄLÄ tuhota GameObjectia tässä! poistu-skripti tekee sen, kun se on saanut vahvistuksen palvelimelta.
            // Destroy(gameObject); // Tämä rivi poistettu täältä
        }
        else
        {
            Debug.LogWarning("Virheellinen PIN-koodi tai 'salis' InputField puuttuu.");
            // Voit näyttää virheviestin käyttäjälle
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