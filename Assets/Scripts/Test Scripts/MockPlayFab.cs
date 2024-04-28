using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MockPlayFabClientAPI
{
    private static Dictionary<string, string> UserData = new Dictionary<string, string>();

    public static void LoginWithCustomID(string customId, bool createAccount)
    {
        // Simulate successful login
        // No callback required
    }

    public static void UpdateUserData(Dictionary<string, string> data)
    {
        foreach (var element in data)
        {
            UserData[element.Key] = element.Value;
        }
    }

    public static Dictionary<string, string> GetUserData()
    {
        return UserData;
    }
}

public static class MockPlayFabUtiils
{
    public const string Uid = "VREXPO.515.Test";

    public static void ConnectToPlayFab()
    {
        LoginToPlayFab();
    }

    public static void SavePlayerData(RoomData roomData, string fileName)
    {
        roomData.SavePlayerData(fileName);
    }

    public static void LoadPlayerData(RoomData roomData, string fileName)
    {
        roomData = RoomData.LoadPlayerData(fileName);
    }

    static void LoginToPlayFab()
    {
        MockPlayFabClientAPI.LoginWithCustomID(Uid, true);
    }

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void StoreJsonInPlayFab(RoomData roomData, bool encryptedDataInPlayFab)
    {
        string jsonData = encryptedDataInPlayFab ? roomData.EncryptJSON() : roomData.ConvertToJSON();

        Debug.Log($"Successfully converted to Json : {jsonData}");
        string roomNameKey = roomData.Roomname;
        RoomNameKeyInternal = roomNameKey;
        EncryptedDataInPlayFab = encryptedDataInPlayFab;

        var data = new Dictionary<string, string>
        {
            { roomNameKey, jsonData }
        };

        MockPlayFabClientAPI.UpdateUserData(data);
        OnDataUpdateSuccess();
    }

    internal static string RoomNameKeyInternal;
    internal static RoomData RoomDataInternal;
    internal static bool EncryptedDataInPlayFab;

    public static RoomData RetrieveJsonFromPlayFab(string roomNameKey, bool encryptedDataInPlayFab)
    {
        RoomNameKeyInternal = roomNameKey;
        EncryptedDataInPlayFab = encryptedDataInPlayFab;
        Dictionary<string, string> userData = MockPlayFabClientAPI.GetUserData();
        OnDataRetrieveSuccess(userData);
        return RoomDataInternal;
    }

    private static void OnDataUpdateSuccess()
    {
        Debug.Log($"JSON data stored successfully in PlayFab! For Room : {RoomNameKeyInternal}");
    }

    private static void OnDataRetrieveSuccess(Dictionary<string, string> userData)
    {
        if (userData != null && userData.ContainsKey(RoomNameKeyInternal))
        {
            string jsonData = userData[RoomNameKeyInternal];
            Debug.Log("Retrieved JSON data from PlayFab: " + jsonData);

            // Process the retrieved JSON data as needed
            ProcessJsonData(EncryptedDataInPlayFab, jsonData);
        }
        else
        {
            Debug.Log("No JSON data found in PlayFab");
            RoomDataInternal = null;
        }
    }

    private static void ProcessJsonData(bool isEncrypted, string jsonData)
    {
        string json = null;
        if (isEncrypted) json = RoomData.DecryptJSON(jsonData);
        RoomDataInternal = isEncrypted ? RoomData.ConvertJSONStringToRoomData(json) : RoomData.ConvertJSONStringToRoomData(jsonData);
    }
}
