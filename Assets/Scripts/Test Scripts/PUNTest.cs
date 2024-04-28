using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;

//[TestFixture]
public class PUNTest
{
    [SetUp]
    public void Setup()
    {
        // Connect to Photon server before running each test
        PhotonNetwork.ConnectUsingSettings();
    }

    [TearDown]
    public void Teardown()
    {
        // Disconnect from Photon server after each test
        PhotonNetwork.Disconnect();
    }

    // [Test]
    public void TestJoinLobby()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player1";

        // Join the default lobby
        PhotonNetwork.JoinLobby();

        // Wait for the lobby join callback
        while (!PhotonNetwork.InLobby)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are connected to the lobby
        Assert.IsTrue(PhotonNetwork.InLobby);
    }

    // [Test]
    public void TestJoinRandomRoom()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player2";

        // Join a random room
        PhotonNetwork.JoinRandomRoom();

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
    }

    // [Test]
    public void TestCreateRoom()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player3";

        // Create a new room with custom options
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "GameMode", "Deathmatch" } }
        };

        PhotonNetwork.CreateRoom("TestRoom", roomOptions);

        // Wait for the room creation callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in the created room
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.AreEqual("TestRoom", PhotonNetwork.CurrentRoom.Name);
        Assert.AreEqual(4, PhotonNetwork.CurrentRoom.MaxPlayers);
        Assert.AreEqual("Deathmatch", PhotonNetwork.CurrentRoom.CustomProperties["GameMode"]);
    }

    // [Test]
    public void TestLoadLevel()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player4";

        // Load a level for all players in the room
        PhotonNetwork.LoadLevel("GameScene");

        // Assert that the level is loaded (you may need to wait for the level load to complete)
        Assert.AreEqual("GameScene", Application.loadedLevelName);
    }

    // [Test]
    public void TestJoinRandomRoomWithExpectedCustomProperties()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player5";

        // Define the expected custom properties for the room
        ExitGames.Client.Photon.Hashtable expectedCustomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "GameMode", "TeamDeathmatch" },
            { "MapName", "Desert" }
        };

        // Join a random room with the expected custom properties
        PhotonNetwork.JoinRandomRoom(expectedCustomProperties, 0);

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room with the expected custom properties
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
        Assert.AreEqual("TeamDeathmatch", PhotonNetwork.CurrentRoom.CustomProperties["GameMode"]);
        Assert.AreEqual("Desert", PhotonNetwork.CurrentRoom.CustomProperties["MapName"]);
    }

    // [Test]
    public void TestJoinRandomRoomWithExpectedMaxPlayers()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player6";

        // Define the expected maximum number of players for the room
        byte expectedMaxPlayers = 6;

        // Join a random room with the expected maximum number of players
        PhotonNetwork.JoinRandomRoom(null, expectedMaxPlayers);

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room with the expected maximum number of players
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
        Assert.AreEqual(expectedMaxPlayers, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    // [Test]
    public void TestJoinRandomRoomWithMatchingType()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player7";

        // Define the expected matching type for the room
        MatchmakingMode expectedMatchingType = MatchmakingMode.FillRoom;

        // Join a random room with the expected matching type
        PhotonNetwork.JoinRandomRoom(null, 0, expectedMatchingType, null, null);

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
    }

    // [Test]
    public void TestJoinRandomRoomWithTypedLobby()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player8";

        // Define the expected typed lobby for the room
        TypedLobby expectedLobby = new TypedLobby("LobbyName", LobbyType.Default);

        // Join a random room with the expected typed lobby
        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, expectedLobby, null, null);

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
    }

    // [Test]
    public void TestJoinRandomRoomWithExpectedUsers()
    {
        // Set the player nickname
        PhotonNetwork.NickName = "Player9";

        // Define the expected users for the room
        string[] expectedUsers = new string[] { "Player1", "Player2", "Player3" };

        // Join a random room with the expected users
        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null, expectedUsers);

        // Wait for the room join callback
        while (!PhotonNetwork.InRoom)
        {
            // Simulate waiting for a short time
            System.Threading.Thread.Sleep(100);
        }

        // Assert that we are in a room
        Assert.IsTrue(PhotonNetwork.InRoom);
        Assert.IsNotNull(PhotonNetwork.CurrentRoom);
    }
}
