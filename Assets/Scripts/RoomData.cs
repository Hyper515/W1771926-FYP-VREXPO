using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;
using System;
using System.IO;
using System.Buffers.Text;

public class RoomData
{

    public string Uid;

    public string Roomname;
    public bool IsPrivate;
    public List<string> ValidUsers;

    public RoomData()
    {
        
    }

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

    // ///////////////////////////////////////////////////////////////////////////////////////////

    public static RoomData LoadPlayerData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string loadedEncryptedText = File.ReadAllText(filePath);
            string json = DecryptJSON(loadedEncryptedText);

            RoomData roomData = ConvertJSONStringToRoomData(json);

            return roomData;
        }

        throw new Exception($"File does not exist {filePath}");


    }

    public void SavePlayerData(string filePath)
    {
        string encryptedTextToSave = EncryptJSON();
        File.WriteAllText(filePath, encryptedTextToSave);
    }

    // ///////////////////////////////////////////////////////////////////////////////////////////

    private static string EncryptJSON(string serialisedDataString)
    {
        string base64 = ConvertToBase64(serialisedDataString);
        string encryptedTextToSave = EncryptString(base64);
        return encryptedTextToSave;
    }

    public string EncryptJSON()
    {
        string serialisedDataString = ConvertToJSON();
        return EncryptJSON(serialisedDataString);
    }

    public static string DecryptJSON(string encryptedText)
    {
        string decryptedText = DecryptString(encryptedText);
        string json = DecodeFromBase64(decryptedText);
        return json;
    }

    public static RoomData ConvertJSONStringToRoomData(string json)
    {
        return JSON.ParseString(json).Deserialize<RoomData>();
    }

    public static string ConvertToBase64(string serialisedDataString)
    {
        var aesEncryptionSave = new AesEncryption();
        // Text to be encrypted
        string base64 = aesEncryptionSave.EncodeToBase64(serialisedDataString);
        Debug.Log($"EncodeToBase64 Text: {base64}");

        return base64;
    }

    public static string DecodeFromBase64(string encodedString)
    {
        var aesEncryptionLoad = new AesEncryption();
        // Text to be encrypted
        string raw = aesEncryptionLoad.DecodeFromBase64(encodedString);
        Debug.Log($"DecodeFromBase64 Text: {raw}");

        return raw;
    }

    public static string EncryptString(string dataString)
    {
        var aesEncryptionSave = new AesEncryption();
        // Text to be encrypted
        string encryptedTextToSave = aesEncryptionSave.Encrypt(dataString);
        Debug.Log($"Encrypted Text: {encryptedTextToSave}");

        return encryptedTextToSave;
    }

    public static string DecryptString(string dataString)
    {
        var aesEncryptionLoad = new AesEncryption();
        // Text to be encrypted
        string decryptedText = aesEncryptionLoad.Decrypt(dataString);
        Debug.Log($"Decrypted Text: {decryptedText}");

        return decryptedText;
    }

    public string ConvertToJSON()
    {
        string serialisedDataString = JSON.Serialize(this).CreateString();
        Debug.Log($"JSONText Text: {serialisedDataString}");
        return serialisedDataString;
    }


}
