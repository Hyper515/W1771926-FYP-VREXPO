using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AesEncryption
{
    // The encryption key and initialization vector (IV)
    private string key = "1234567890123456";
    private string iv = "abcdefghijklmnop";

    public AesEncryption()
    {
        // Ensure that the key and IV are of appropriate length
        if (key.Length != 16 || iv.Length != 16)
        {
            throw new ArgumentException("Key and IV must be 16 characters long.");
        }
    }
    
    public AesEncryption(string sKey, string sIv)
    {
        key = sKey;
        iv = sIv;
        
        // Ensure that the key and IV are of appropriate length
        if (key.Length != 16 || iv.Length != 16)
        {
            throw new ArgumentException("Key and IV must be 16 characters long.");
        }
    }

    // Method to encode a JSON string to Base64
    public string EncodeToBase64(string jsonString)
    {
        // Convert the JSON string to byte array
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

        // Encode the byte array to Base64 string
        string base64EncodedString = Convert.ToBase64String(bytes);

        return base64EncodedString;
    }

    // Method to decode a Base64 string to a regular string
    public string DecodeFromBase64(string base64EncodedString)
    {
        // Convert the Base64 string to byte array
        byte[] bytes = Convert.FromBase64String(base64EncodedString);

        // Decode the byte array to a regular string using UTF-8 encoding
        string decodedString = Encoding.UTF8.GetString(bytes);

        return decodedString;
    }

    // Method to encrypt a plain text using AES encryption
    public string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }

                // Convert the encrypted stream to a Base64 string
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    // Method to decrypt a cipher text using AES decryption
    public string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream and place them in a string.
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
