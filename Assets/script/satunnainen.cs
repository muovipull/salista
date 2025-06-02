using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class satunnainen : MonoBehaviour
{

    // Yhdist� t�m� InputField Unity-editorissa
    public TMP_InputField pituus;
    public TMP_Text n�ytt�salasana;

    public GameObject satuinnais;
    public GameObject paaruutu;

    private const int MAX_PASSWORD_LENGTH = 50;
    private const int MIN_PASSWORD_LENGTH = 1; // Salasanan v�himm�ispituus

    // N�m� ovat aina k�yt�ss�, kuten pyydetty
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NumberChars = "0123456789";
    private const string SpecialChars = "!@?#";
    public void kopioi()
    {
        TextEditor textEditor = new TextEditor
        {
            text = n�ytt�salasana.text
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

    public void poistuttu_alkun�yt�sta()
    {
        satuinnais.SetActive(true);
        paaruutu.SetActive(false);
        n�ytt�salasana.text = "";
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
                n�ytt�salasana.text = "Virhe: Aseta pituuskentt�!";
            }
            return;
        }

        int desiredLength;
        // Yrit� muuntaa InputFieldin teksti kokonaisluvuksi
        if (!int.TryParse(pituus.text, out desiredLength))
        {
            Debug.LogError("Virheellinen sy�te pituudelle. Sy�t� vain numeroita.");
            if (n�ytt�salasana != null)
            {
                n�ytt�salasana.text = "Virhe: Anna kelvollinen numero (1-" + MAX_PASSWORD_LENGTH + ")!";
            }
            return;
        }

        // Tarkista pituuden rajat
        if (desiredLength < MIN_PASSWORD_LENGTH || desiredLength > MAX_PASSWORD_LENGTH)
        {
            Debug.LogError($"Salasanan pituuden on oltava {MIN_PASSWORD_LENGTH} ja {MAX_PASSWORD_LENGTH} merkin v�lill�.");
            if (n�ytt�salasana != null)
            {
                n�ytt�salasana.text = $"Pituus oltava {MIN_PASSWORD_LENGTH}-{MAX_PASSWORD_LENGTH}!";
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

        // N�yt� salasana UI-tekstikent�ss�, jos sellainen on liitetty
        if (n�ytt�salasana != null)
        {
            n�ytt�salasana.text = generatedPassword;
        }
    }

 
}
