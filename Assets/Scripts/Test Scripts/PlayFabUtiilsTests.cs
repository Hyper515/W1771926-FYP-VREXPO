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
        MockPlayFabUtiils.ConnectToPlayFab();
        roomData = new RoomData("TestRoom", true);
        MockPlayFabUtiils.SavePlayerData(roomData, "TestData.json");
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
        MockPlayFabUtiils.ConnectToPlayFab();
        Thread.Sleep(5000);
    }

    [Test]
    public void SavePlayerData_WithValidData_SavesDataToFile()
    {
        // Act
        MockPlayFabUtiils.SavePlayerData(roomData, "TestData.json");

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
        MockPlayFabUtiils.StoreJsonInPlayFab(roomData, true);

        // Assert
        Assert.IsTrue(MockPlayFabUtiils.RoomDataInternal != null);
        Assert.AreEqual("TestRoom", MockPlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(MockPlayFabUtiils.RoomDataInternal.IsPrivate);
    }

    [Test]
    public void StoreJsonInPlayFab_WithUnencryptedData_StoresDataInPlayFab()
    {
        // Act
        MockPlayFabUtiils.StoreJsonInPlayFab(roomData, false);

        // Assert
        Assert.IsTrue(MockPlayFabUtiils.RoomDataInternal != null);
        Assert.AreEqual("TestRoom", MockPlayFabUtiils.RoomDataInternal.Roomname);
        Assert.IsTrue(MockPlayFabUtiils.RoomDataInternal.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithEncryptedData_RetrievesDataFromPlayFab()
    {
        // Arrange
        MockPlayFabUtiils.StoreJsonInPlayFab(roomData, true);

        // Act
        RoomData retrievedRoomData = MockPlayFabUtiils.RetrieveJsonFromPlayFab("TestRoom", true);

        // Assert
        Assert.IsNotNull(retrievedRoomData);
        Assert.AreEqual("TestRoom", retrievedRoomData.Roomname);
        Assert.IsTrue(retrievedRoomData.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithUnencryptedData_RetrievesDataFromPlayFab()
    {
        // Arrange
        MockPlayFabUtiils.StoreJsonInPlayFab(roomData, false);

        // Act
        RoomData retrievedRoomData = MockPlayFabUtiils.RetrieveJsonFromPlayFab("TestRoom", false);

        // Assert
        Assert.IsNotNull(retrievedRoomData);
        Assert.AreEqual("TestRoom", retrievedRoomData.Roomname);
        Assert.IsTrue(retrievedRoomData.IsPrivate);
    }

    [Test]
    public void RetrieveJsonFromPlayFab_WithNonExistentRoom_ReturnsNull()
    {
        // Act
        RoomData retrievedRoomData = MockPlayFabUtiils.RetrieveJsonFromPlayFab("NonExistentRoom", true);

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
