using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tallnneus_random : MonoBehaviour
{
    // Yhdistä tämä InputField Unity-editorissa
    public TMP_InputField pituus;
    public TMP_Text näyttösalasana;
    public TMP_Text virheviesti;

    public GameObject koipiointi_nappi;

    public TMP_Text luo_nappi_text;

    public InputField salansana_input_normi;


    private const int MAX_PASSWORD_LENGTH = 50;
    private const int MIN_PASSWORD_LENGTH = 1; // Salasanan vähimmäispituus

    // Nämä ovat aina käytössä, kuten pyydetty
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumberChars = "0123456789";
    private const string SpecialChars = "!@?#";


    private void Start()
    {
        koipiointi_nappi.SetActive(false);

        luo_nappi_text.text = "Luo satunainen";

        pituus.text = "";
        näyttösalasana.text = "";

        if (virheviesti != null)
        {
            virheviesti.text = "Tai luo satunnainen salasana oman toiveen mukaan";
        }


    }


    // Funktio, joka luo satunnaisen salasanan InputFieldin arvon perusteella
    public void luo_satunnainen()
    {
        if (pituus == null)
        {
            Debug.LogError("InputField ei ole liitetty! Tarkista Inspector.");
            if (pituus != null)
            {
                virheviesti.text = "Virhe: Aseta pituuskenttä!";
            }
            return;
        }

        int desiredLength;
        // Yritä muuntaa InputFieldin teksti kokonaisluvuksi
        if (!int.TryParse(pituus.text, out desiredLength))
        {
            Debug.LogError("Virheellinen syöte pituudelle. Syötä vain numeroita.");
            if (virheviesti != null)
            {
                virheviesti.text = "Virhe: Anna kelvollinen numero (1-" + MAX_PASSWORD_LENGTH + ")!";
            }
            return;
        }

        // Tarkista pituuden rajat
        if (desiredLength < MIN_PASSWORD_LENGTH || desiredLength > MAX_PASSWORD_LENGTH)
        {
            Debug.LogError($"Salasanan pituuden on oltava {MIN_PASSWORD_LENGTH} ja {MAX_PASSWORD_LENGTH} merkin välillä.");
            if (virheviesti != null)
            {
                virheviesti.text = $"Pituus oltava {MIN_PASSWORD_LENGTH}-{MAX_PASSWORD_LENGTH}!";
            }
            return;
        }

        // Kokoa kaikki sallitut merkit
        StringBuilder availableChars = new StringBuilder();
        availableChars.Append(LowercaseChars);
        availableChars.Append(UppercaseChars);
        availableChars.Append(NumberChars);
        availableChars.Append(SpecialChars);

        StringBuilder password = new StringBuilder();

        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[1];
            for (int i = 0; i < desiredLength; i++)
            {
                rng.GetBytes(randomNumber);
                int randomIndex = randomNumber[0] % availableChars.Length;
                password.Append(availableChars[randomIndex]);
            }
        }

        string generatedPassword = password.ToString();
        Debug.Log("Luotu salasana: " + generatedPassword);

        koipiointi_nappi.SetActive(true);
        
        salansana_input_normi.text = generatedPassword;

        luo_nappi_text.text = "Luo uusi salasana";

        // Näytä salasana UI-tekstikentässä, jos sellainen on liitetty
        if (näyttösalasana != null)
        {
            näyttösalasana.text = generatedPassword;
        }
    }
}
