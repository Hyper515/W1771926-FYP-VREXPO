using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using System.IO;

[TestFixture]
public class RoomDataTests
{
    [Test]
    public void Constructor_WithRoomNameAndIsPrivate_CreatesRoomDataObject()
    {
        // Arrange
        string roomName = "RoomA";
        bool isPrivate = true;

        // Act
        RoomData roomData = new RoomData(roomName, isPrivate);

        // Assert
        Assert.IsNotNull(roomData.Uid);
        Assert.AreEqual(roomName, roomData.Roomname);
        Assert.AreEqual(isPrivate, roomData.IsPrivate);
        Assert.IsEmpty(roomData.ValidUsers);
    }

    [Test]
    public void IsUserAllowed_OnPublicRoom_ReturnsTrue()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomB", false);

        // Act
        bool result = roomData.IsUserAllowed("Alice");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsUserAllowed_OnPrivateRoomWithoutUser_ReturnsFalse()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomC", true);

        // Act
        bool result = roomData.IsUserAllowed("Bob");

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsUserAllowed_OnPrivateRoomWithUser_ReturnsTrue()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomD", true);
        roomData.AddUserToList("Ethan");

        // Act
        bool result = roomData.IsUserAllowed("Ethan");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void AddUserToList_WithNewUser_AddsUserToValidUsers()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomE", true);

        // Act
        roomData.AddUserToList("Alice");

        // Assert
        Assert.Contains("alice", roomData.ValidUsers);
    }

    [Test]
    public void AddUserToList_WithExistingUser_DoesNotDuplicateUser()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomF", true);
        roomData.AddUserToList("Bob");

        // Act
        roomData.AddUserToList("Bob");

        // Assert
        Assert.AreEqual(1, roomData.ValidUsers.Count);
    }

    [Test]
    public void LoadPlayerData_WithExistingFile_LoadsRoomDataFromFile()
    {
        // Arrange
        string filePath = "RoomDataG.json";
        RoomData expectedRoomData = new RoomData("RoomG", true);
        expectedRoomData.SavePlayerData(filePath);

        // Act
        RoomData actualRoomData = RoomData.LoadPlayerData(filePath);

        // Assert
        Assert.AreEqual(expectedRoomData.Roomname, actualRoomData.Roomname);
        Assert.AreEqual(expectedRoomData.IsPrivate, actualRoomData.IsPrivate);
    }

    [Test]
    public void LoadPlayerData_WithNonExistentFile_ThrowsException()
    {
        // Arrange
        string filePath = "TestData/NonExistentFile.json";

        // Act & Assert
        Assert.Throws<Exception>(() => RoomData.LoadPlayerData(filePath));
    }

    [Test]
    public void SavePlayerData_SavesRoomDataToFile()
    {
        // Arrange
        string filePath = "SavedRoomData.json";
        RoomData roomData = new RoomData("RoomH", false);

        // Act
        roomData.SavePlayerData(filePath);

        // Assert
        Assert.IsTrue(File.Exists(filePath));
    }

    [Test]
    public void EncryptAndDecryptJSON_ReturnsOriginalRoomData()
    {
        // Arrange
        RoomData originalRoomData = new RoomData("RoomI", true);

        // Act
        string encryptedJSON = originalRoomData.EncryptJSON();
        string decryptedJSON = RoomData.DecryptJSON(encryptedJSON);
        RoomData decryptedRoomData = RoomData.ConvertJSONStringToRoomData(decryptedJSON);

        // Assert
        Assert.AreEqual(originalRoomData.Roomname, decryptedRoomData.Roomname);
        Assert.AreEqual(originalRoomData.IsPrivate, decryptedRoomData.IsPrivate);
    }

    [Test]
    public void ConvertToAndFromBase64_ReturnsOriginalString()
    {
        // Arrange
        string originalString = "Hello, World!";

        // Act
        string base64String = RoomData.ConvertToBase64(originalString);
        string decodedString = RoomData.DecodeFromBase64(base64String);

        // Assert
        Assert.AreEqual(originalString, decodedString);
    }

    [Test]
    public void EncryptAndDecryptString_ReturnsOriginalString()
    {
        // Arrange
        string originalString = "Hello, World!";

        // Act
        string encryptedString = RoomData.EncryptString(originalString);
        string decryptedString = RoomData.DecryptString(encryptedString);

        // Assert
        Assert.AreEqual(originalString, decryptedString);
    }

    [Test]
    public void ConvertToJSON_ReturnsJSONString()
    {
        // Arrange
        RoomData roomData = new RoomData("RoomJ", false);

        // Act
        string jsonString = roomData.ConvertToJSON();

        // Assert
        StringAssert.Contains("\"Roomname\":\"RoomJ\"", jsonString);
        StringAssert.Contains("\"IsPrivate\":false", jsonString);
    }
}
