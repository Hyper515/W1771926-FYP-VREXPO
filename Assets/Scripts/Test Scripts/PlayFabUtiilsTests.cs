using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using System.IO;
using SimpleJSON;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.EditorModels;
using System.Threading.Tasks;
using System.Threading;

[TestFixture]
public class PlayFabUtiilsTests
{
    private RoomData roomData;
    public const string Uid = "VREXPO.515.Test";

    [SetUp]
    public void SetUp()   
    {
        //LoginToPlayFab();
        PlayFabUtiils.ConnectToPlayFab();
        Thread.Sleep(5000);
        //PlayFabUtiils.LoginToPlayFabAsync().GetAwaiter().GetResult();
        //PlayFabUtiils.ConnectToPlayFab().GetAwaiter().GetResult();
        roomData = new RoomData("TestRoom", true);
        PlayFabUtiils.SavePlayerData(roomData, "TestData.json");
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the test data file if it exists
        if (File.Exists("TestData.json"))
        {
            File.Delete("TestData.json");
        }
    }

    [Test]
    public void PlayFabUtiils_Login()
    {
        //LoginToPlayFab();
        PlayFabUtiils.ConnectToPlayFab();
        Thread.Sleep(5000);
    }

    [Test]
    public void SavePlayerData_WithValidData_SavesDataToFile()
    {
        // Act
        PlayFabUtiils.SavePlayerData(roomData, "TestData.json");

        // Assert
        Assert.IsTrue(File.Exists("TestData.json"));
    }

    [Test]
    public void LoadPlayerData_WithExistingFile_LoadsDataFromFile()
    {
        // Arrange
        PlayFabUtiils.SavePlayerData(roomData, "TestData.json");

        // Act
        RoomData loadedRoomData = PlayFabUtiils.LoadPlayerData( "TestData.json");

        // Assert
        Assert.IsNotNull(loadedRoomData);
        Assert.AreEqual("TestRoom", loadedRoomData.Roomname);
        Assert.IsTrue(loadedRoomData.IsPrivate);
    }

    [Test]
    public void LoadPlayerData_WithNonExistentFile_ThrowsException()
    {

        // Arrange, Act & Assert
        Assert.Throws<Exception>(() => PlayFabUtiils.LoadPlayerData( "NonExistentFile.json"), "File does not exist NonExistentFile.json");
    }

    [Test]
    public void StoreJsonInPlayFab_WithEncryptedData_StoresDataInPlayFab()
    {
        // Act
        PlayFabUtiils.StoreJsonInPlayFab(roomData, true);

        // Assert
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal != null);
        Assert.AreEqual("TestRoom", PlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal.IsPrivate);
    }

    [Test]
    public void StoreJsonInPlayFab_WithUnencryptedData_StoresDataInPlayFab()
    {
        // Act
        PlayFabUtiils.StoreJsonInPlayFab(roomData, false);

        // Assert
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal != null);
        Assert.AreEqual("TestRoom", PlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithEncryptedData_RetrievesDataFromPlayFab()
    {
        // Arrange
        PlayFabUtiils.StoreJsonInPlayFab(roomData, true);

        // Act
        RoomData retrievedRoomData = PlayFabUtiils.RetrieveJsonFromPlayFab("TestRoom", true);

        // Assert
        Assert.IsNotNull(retrievedRoomData);
        Assert.AreEqual("TestRoom", retrievedRoomData.Roomname);
        Assert.IsTrue(retrievedRoomData.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithUnencryptedData_RetrievesDataFromPlayFab()
    {
        // Arrange
        PlayFabUtiils.StoreJsonInPlayFab(roomData, false);

        // Act
        RoomData retrievedRoomData = PlayFabUtiils.RetrieveJsonFromPlayFab("TestRoom", false);

        // Assert
        Assert.IsNotNull(retrievedRoomData);
        Assert.AreEqual("TestRoom", retrievedRoomData.Roomname);
        Assert.IsTrue(retrievedRoomData.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithNonExistentRoom_ReturnsNull()
    {
        // Act
        RoomData retrievedRoomData = PlayFabUtiils.RetrieveJsonFromPlayFab("NonExistentRoom", true);

        // Assert
        Assert.IsNull(retrievedRoomData);
    }

    [Test]
    public void ProcessJsonData_WithEncryptedData_DecryptsAndConvertsData()
    {
        // Arrange
        string encryptedJsonData = roomData.EncryptJSON();

        // Act
        PlayFabUtiils.ProcessJsonData(true, encryptedJsonData);

        // Assert
        Assert.IsNotNull(PlayFabUtiils.RoomDataInternal);
        Assert.AreEqual("TestRoom", PlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal.IsPrivate);
    }

    [Test]
    public void ProcessJsonData_WithUnencryptedData_ConvertsDataDirectly()
    {
        // Arrange
        string unencryptedJsonData = roomData.ConvertToJSON();

        // Act
        PlayFabUtiils.ProcessJsonData(false, unencryptedJsonData);

        // Assert
        Assert.IsNotNull(PlayFabUtiils.RoomDataInternal);
        Assert.AreEqual("TestRoom", PlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(PlayFabUtiils.RoomDataInternal.IsPrivate);
    }
}
