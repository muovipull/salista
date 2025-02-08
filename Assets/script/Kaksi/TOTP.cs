using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using UnityEngine;
using TMPro;

public class TOTP : MonoBehaviour

{
    public string secretKey;
    public TMP_InputField tunnus;
    public TMP_Text valitus;
    public GameObject sisalla;
    public GameObject varmsitus;

    public static string GenerateTOTP(string base32Secret, int timeStep = 30, int totpDigits = 6, HashAlgorithmName hashAlgorithm = default)
    {
        if (hashAlgorithm == default)
            hashAlgorithm = HashAlgorithmName.SHA1; // RFC 6238 suosittelee SHA-1 oletuksena

        // Muunnetaan Base32-avaimen tavutaulukoksi
        var keyBytes = Base32Decode(base32Secret);

        // Lasketaan UNIX-aikaleima sekunneissa ja jaetaan aikav‰lill‰
        var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var counter = unixTimestamp / timeStep;

        // Muunnetaan laskuri tavumuotoon
        var counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(counterBytes);

        // Luodaan HMAC-instanssi valitun hajautusalgoritmin perusteella
        byte[] hash;
        using (var hmac = CreateHMAC(hashAlgorithm, keyBytes))
        {
            hash = hmac.ComputeHash(counterBytes);
        }

        // Dynaaminen trunkiinti: haetaan viimeisen tavun arvo & 0xf
        int offset = hash[hash.Length - 1] & 0xf;
        int binaryCode = (hash[offset] & 0x7f) << 24
                         | (hash[offset + 1] & 0xff) << 16
                         | (hash[offset + 2] & 0xff) << 8
                         | (hash[offset + 3] & 0xff);

        int totp = binaryCode % (int)Math.Pow(10, totpDigits);
        return totp.ToString(new string('0', totpDigits)); // Palauttaa TOTP t‰ytettyn‰ nollilla
    }

    private static HMAC CreateHMAC(HashAlgorithmName hashAlgorithm, byte[] key)
    {
        return hashAlgorithm.Name switch
        {
            nameof(HashAlgorithmName.SHA1) => new HMACSHA1(key),
            nameof(HashAlgorithmName.SHA256) => new HMACSHA256(key),
            nameof(HashAlgorithmName.SHA512) => new HMACSHA512(key),
            _ => throw new ArgumentException("Unsupported hash algorithm", nameof(hashAlgorithm)),
        };
    }

    private static byte[] Base32Decode(string base32)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var outputBytes = new List<byte>();
        int buffer = 0, bitsLeft = 0;

        foreach (var c in base32.ToUpperInvariant())
        {
            if (c == '=') break;
            int value = alphabet.IndexOf(c);
            if (value < 0) throw new ArgumentException("Invalid Base32 character", nameof(base32));

            buffer = (buffer << 5) | value;
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                bitsLeft -= 8;
                outputBytes.Add((byte)(buffer >> bitsLeft));
            }
        }

        return outputBytes.ToArray();
    }

    public void Main()
    {
        valitus.text = "";
        string secretKey = Kaksi_vaiheinen_tunnistus.secret; // Esimerkkisalaus Base32-muodossa
        string totp = GenerateTOTP(secretKey);
        print("TOTP: " + totp);
        print("avain " + secretKey);
        if (totp == tunnus.text.ToString())
        {
            sisalla.SetActive(true);

            varmsitus.SetActive(false);
            valitus.text = "";

        }
        if (Kaksi_vaiheinen_tunnistus.vara == tunnus.text.ToString())
        {

            sisalla.SetActive(true);

            varmsitus.SetActive(false);
            valitus.text = "";


        }
        else
        {
            valitus.text = "v‰‰r‰ tunnus yrit‰ uudelleen";
            tunnus.text = "";
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Main();
        }
    }
    private void Start()
    {
        valitus.text = "";
    }
    public void ohita_kirjaus()
    {
        sisalla.SetActive(true);

        varmsitus.SetActive(false);
        

    }
}

