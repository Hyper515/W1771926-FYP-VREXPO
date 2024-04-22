using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;
using System;
using System.IO;

public class RoomData
{

    public string Uid;

    public string Roomname;
    public bool IsPrivate;
    public List<string> ValidUsers;

    public RoomData(string roomName, bool isPrivate)
    {
        Uid = Guid.NewGuid().ToString();

        ValidUsers = new List<string>();

        Roomname = roomName;
        IsPrivate = isPrivate;
    }

    public bool IsUserAllowed(string user)
    {
        if (IsPrivate == false) // Room is Public
        {
            return true; // Allowed Entry
        }

        return ValidUsers.Contains(user.ToLower()); // Search the Valid Names for the Room
    }

    public void AddUserToList(string user)
    {
        if (ValidUsers.Contains(user.ToLower()))
        {
            return;
        }

        ValidUsers.Add(user.ToLower());
    }

    public void SavePlayerData(string filePath)
    {
        string serialisedDataString = JSON.Serialize(this).CreateString();
        Debug.Log($"JSONText Text: {serialisedDataString}");
        AesEncryption aesEncryptionSave = new AesEncryption();
        // Text to be encrypted
        string base64 = aesEncryptionSave.EncodeToBase64(serialisedDataString);
        Debug.Log($"EncodeToBase64 Text: {base64}");

        // Encrypt the text
        string encryptedTextToSave = aesEncryptionSave.Encrypt(base64);
        Debug.Log($"Encrypted Text: {encryptedTextToSave}");

        File.WriteAllText(filePath, encryptedTextToSave);
    }

    public static RoomData LoadPlayerData(string filePath)
    {
        if (File.Exists(filePath))
        {

            string loadedEncryptedText = File.ReadAllText(filePath);
            AesEncryption aesEncryptionLoad = new AesEncryption();

            string decryptedText = aesEncryptionLoad.Decrypt(loadedEncryptedText);
            Debug.Log($"Decrypted Text: {decryptedText}");

            // Decode the text
            string json = aesEncryptionLoad.DecodeFromBase64(decryptedText);
            Debug.Log($"DecodeFromBase64 Text: {json}");

            var roomData = JSON.ParseString(json).Deserialize<RoomData>();

            return roomData;
        }

        throw new Exception($"File does not exist {filePath}");

        
    }
}
