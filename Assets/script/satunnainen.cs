using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class satunnainen : MonoBehaviour
{

    // Yhdistä tämä InputField Unity-editorissa
    public TMP_InputField pituus;
    public TMP_Text näyttösalasana;

    public GameObject satuinnais;
    public GameObject paaruutu;

    private const int MAX_PASSWORD_LENGTH = 50;
    private const int MIN_PASSWORD_LENGTH = 1; // Salasanan vähimmäispituus

    // Nämä ovat aina käytössä, kuten pyydetty
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumberChars = "0123456789";
    private const string SpecialChars = "!@?#";
    public void kopioi()
    {
        TextEditor textEditor = new TextEditor
        {
            text = näyttösalasana.text
        };
        textEditor.SelectAll();
        textEditor.Copy();
        print("Kopioitu");
    }
    public void peruuta()
    {
        print("Peruutettu");
        satuinnais.SetActive(false);
        paaruutu.SetActive(true);
    }

    public void poistuttu_alkunäytösta()
    {
        satuinnais.SetActive(true);
        paaruutu.SetActive(false);
        näyttösalasana.text = "";
        pituus.Select();
        pituus.text = "";
    }




    // Funktio, joka luo satunnaisen salasanan InputFieldin arvon perusteella
    public void GeneratePasswordFromInput()
    {
        if (pituus == null)
        {
            Debug.LogError("InputField ei ole liitetty! Tarkista Inspector.");
            if (pituus != null)
            {
                näyttösalasana.text = "Virhe: Aseta pituuskenttä!";
            }
            return;
        }

        int desiredLength;
        // Yritä muuntaa InputFieldin teksti kokonaisluvuksi
        if (!int.TryParse(pituus.text, out desiredLength))
        {
            Debug.LogError("Virheellinen syöte pituudelle. Syötä vain numeroita.");
            if (näyttösalasana != null)
            {
                näyttösalasana.text = "Virhe: Anna kelvollinen numero (1-" + MAX_PASSWORD_LENGTH + ")!";
            }
            return;
        }

        // Tarkista pituuden rajat
        if (desiredLength < MIN_PASSWORD_LENGTH || desiredLength > MAX_PASSWORD_LENGTH)
        {
            Debug.LogError($"Salasanan pituuden on oltava {MIN_PASSWORD_LENGTH} ja {MAX_PASSWORD_LENGTH} merkin välillä.");
            if (näyttösalasana != null)
            {
                näyttösalasana.text = $"Pituus oltava {MIN_PASSWORD_LENGTH}-{MAX_PASSWORD_LENGTH}!";
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

        // Näytä salasana UI-tekstikentässä, jos sellainen on liitetty
        if (näyttösalasana != null)
        {
            näyttösalasana.text = generatedPassword;
        }
    }

 
}
