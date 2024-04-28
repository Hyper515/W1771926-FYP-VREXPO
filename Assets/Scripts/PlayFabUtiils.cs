using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Leguar.TotalJSON;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
//using PlayFab.PfEditor.EditorModels;

public static class PlayFabUtiils
{
    public const string Uid = "VREXPO.515";

    public static void ConnectToPlayFab()
    {
        
        LoginToPlayFab();
    }

    public static void SavePlayerData(RoomData roomData, string fileName)
    {
        roomData.SavePlayerData(fileName);
    }

    public static RoomData LoadPlayerData(string fileName)
    {
        return RoomData.LoadPlayerData(fileName);
    }

    internal static void LoginToPlayFab()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = Uid,
        };

        Debug.Log("Attempting to Login to Playfab");

        PlayFabClientAPI.LoginWithCustomID(request, PlayFabLoginResults, PlayFabLoginError);
    }
    
    internal static async Task LoginToPlayFabAsync()
    {
        LoginToPlayFab();

        await Task.Delay(5000); // Wait for the async operation to complete
    }

    static void PlayFabLoginResults(LoginResult loginResult)
    {
        Debug.Log("PlayFab - Login Succeded: " + loginResult.ToJson());
    }

    static void PlayFabLoginError(PlayFabError loginError)
    {
        Debug.Log("PlayFab - Login Failed: " + loginError.ErrorMessage);
    }

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void StoreJsonInPlayFab(RoomData roomData, bool encryptedDataInPlayFab)
    {
        string jsonData = encryptedDataInPlayFab ? roomData.EncryptJSON() : roomData.ConvertToJSON();

        Debug.Log($"Successfully converted to Json : {jsonData}");
        string roomNameKey = roomData.Roomname;
        RoomNameKeyInternal = roomNameKey;
        EncryptedDataInPlayFab = encryptedDataInPlayFab;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { roomNameKey, jsonData }
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataUpdateSuccess, OnError);
    }

    internal static string RoomNameKeyInternal;
    internal static RoomData RoomDataInternal;
    internal static bool EncryptedDataInPlayFab;

    public static RoomData RetrieveJsonFromPlayFab(string roomNameKey, bool encryptedDataInPlayFab)
    {
        RoomNameKeyInternal = roomNameKey;
        EncryptedDataInPlayFab = encryptedDataInPlayFab;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRetrieveSuccess, OnError);
        return RoomDataInternal;
    }

    private static void OnDataUpdateSuccess(UpdateUserDataResult result)
    {
        Debug.Log($"JSON data stored successfully in PlayFab! For Room : {RoomNameKeyInternal}");
    }

    private static void OnDataRetrieveSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey(RoomNameKeyInternal))
        {
            string jsonData = result.Data[RoomNameKeyInternal].Value;
            Debug.Log("Retrieved JSON data from PlayFab: " + jsonData);

            // Process the retrieved JSON data as needed
            ProcessJsonData(EncryptedDataInPlayFab, jsonData);
        }
        else
        {
            Debug.LogError("No JSON data found in PlayFab");
            RoomDataInternal = null;
        }
    }

    private static void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
    }

    internal static void ProcessJsonData(bool isEncrypted, string jsonData)
    {
        string json = null;
        if (isEncrypted) json = RoomData.DecryptJSON(jsonData);

        RoomDataInternal = isEncrypted ? RoomData.ConvertJSONStringToRoomData(json) : RoomData.ConvertJSONStringToRoomData(jsonData);
    }
}
