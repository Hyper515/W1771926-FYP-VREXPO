using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[TestFixture]
public class RoomManagerTest
{
    private RoomManager roomManager;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and attach the RoomManager script
        GameObject gameObject = new GameObject();
        roomManager = gameObject.AddComponent<RoomManager>();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the GameObject and RoomManager instance
        Object.Destroy(roomManager.gameObject);
    }

    [Test]
    public void Start_ConnectsToPhotonAndJoinsLobby()
    {
        // Arrange
        PhotonNetwork.Disconnect();

        // Act
        roomManager.SetUpSystem();

        // Assert
        Assert.IsTrue(PhotonNetwork.IsConnected);

        bool inLobby = PhotonNetwork.JoinLobby();
        Assert.IsTrue(PhotonNetwork.InLobby);
    }

    [Test]
    public void JoinRandomRoom_JoinsRandomRoomOrCreatesNew()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();

        // Act
        roomManager.JoinRandomRoom();

        // Assert
        Assert.IsTrue(PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady);
    }

    [TestCase("RoomA", MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_A)]
    [TestCase("RoomB", MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B)]
    [TestCase("RoomC", MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_C)]
    public void OnEnterButtonClicked_PublicRoom_JoinsOrCreatesRoom(string roomName, string mapType)
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();

        // Act
        switch (roomName)
        {
            case "RoomA":
                roomManager.OnEnterButtonClicked_RoomA();
                break;
            case "RoomB":
                roomManager.OnEnterButtonClicked_RoomB();
                break;
            case "RoomC":
                roomManager.OnEnterButtonClicked_RoomC();
                break;
        }

        // Assert
        Assert.IsTrue(PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady);
        if (PhotonNetwork.InRoom)
        {
            Assert.AreEqual(mapType, PhotonNetwork.CurrentRoom.CustomProperties[MultiplayerVRConstants.MAP_TYPE_KEY]);
        }
    }

    [TestCase("PrivateRoomA", MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_A)]
    [TestCase("PrivateRoomB", MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_B)]
    [TestCase("PrivateRoomC", MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_C)]
    public void OnEnterButtonClicked_PrivateRoom_JoinsOrCreatesRoomWithValidUser(string roomName, string mapType)
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = "ValidUser";

        // Act
        switch (roomName)
        {
            case "PrivateRoomA":
                roomManager.OnEnterButtonClicked_Private_RoomA();
                break;
            case "PrivateRoomB":
                roomManager.OnEnterButtonClicked_Private_RoomB();
                break;
            case "PrivateRoomC":
                roomManager.OnEnterButtonClicked_Private_RoomC();
                break;
        }

        // Assert
        Assert.IsTrue(PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady);
        if (PhotonNetwork.InRoom)
        {
            Assert.AreEqual(mapType, PhotonNetwork.CurrentRoom.CustomProperties[MultiplayerVRConstants.MAP_TYPE_KEY]);
        }
    }

    [TestCase("PrivateRoomA")]
    [TestCase("PrivateRoomB")]
    [TestCase("PrivateRoomC")]
    public void OnEnterButtonClicked_PrivateRoom_DoesNotJoinRoomWithInvalidUser(string roomName)
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = "InvalidUser";

        // Act
        switch (roomName)
        {
            case "PrivateRoomA":
                roomManager.OnEnterButtonClicked_Private_RoomA();
                break;
            case "PrivateRoomB":
                roomManager.OnEnterButtonClicked_Private_RoomB();
                break;
            case "PrivateRoomC":
                roomManager.OnEnterButtonClicked_Private_RoomC();
                break;
        }

        // Assert
        Assert.IsFalse(PhotonNetwork.InRoom);
    }

    [Test]
    public void OnJoinRandomFailed_CreatesAndJoinsNewRoom()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();

        // Act
        roomManager.OnJoinRandomFailed(0, "Test");

        // Assert
        Assert.IsTrue(PhotonNetwork.InRoom);
    }

    [Test]
    public void OnConnectedToMaster_JoinsLobby()
    {
        // Arrange
        PhotonNetwork.Disconnect();

        // Act
        roomManager.OnConnectedToMaster();

        // Assert
        Assert.IsTrue(PhotonNetwork.IsMasterClient);
    }

    [Test]
    public void OnCreatedRoom_LogsRoomName()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        string expectedRoomName = "TestRoom";
        PhotonNetwork.CreateRoom(expectedRoomName);

        // Act
        roomManager.OnCreatedRoom();

        // Assert
        // Verify that the room name is logged (you can use a mocking framework or check the debug log)
        // Example using a mocking framework like Moq:
        // mockedLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void OnJoinedRoom_LoadsSceneBasedOnMapType()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        string expectedMapType = MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_A;
        string expectedSceneName = "Public_Room_A";
        PhotonNetwork.CreateRoom("TestRoom", new RoomOptions { CustomRoomProperties = { { MultiplayerVRConstants.MAP_TYPE_KEY, expectedMapType } } });

        // Act
        roomManager.OnJoinedRoom();

        // Assert
        Assert.AreEqual(expectedSceneName, SceneManager.GetActiveScene().name);
    }

    [Test]
    public void OnJoinedRoom_WithoutMapType_DoesNotLoadScene()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.CreateRoom("TestRoom");
        string initialSceneName = SceneManager.GetActiveScene().name;

        // Act
        roomManager.OnJoinedRoom();

        // Assert
        Assert.AreEqual(initialSceneName, SceneManager.GetActiveScene().name);
    }

    [Test]
    public void OnRoomListUpdate_WithEmptyList_UpdatesOccupancyTexts()
    {
        // Arrange
        PhotonNetwork.ConnectUsingSettings();
        List<RoomInfo> roomList = new List<RoomInfo>();

        // Act
        roomManager.OnRoomListUpdate(roomList);

        // Assert
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_PRA.text);
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_PRB.text);
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_PRC.text);
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_Private_RA.text);
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_Private_RB.text);
        Assert.AreEqual("0 / 20", roomManager.OccupancyRateText_For_Private_RC.text);
    }

    [Test]
    public void OnJoinedLobby_LogsLobbyJoined()
    {
        // Arrange
        PhotonNetwork.Disconnect();

        // Act
        roomManager.OnJoinedLobby();

        // Assert
        // Verify that the "Lobby Joined" message is logged (you can use a mocking framework or check the debug log)
        // Example using a mocking framework like Moq:
        // mockedLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
    }
}
