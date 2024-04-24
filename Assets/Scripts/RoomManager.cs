using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string mapType;

    // Update UserNames, set to true
    public bool isHardcodedUserNames = true;
    public bool encryptedDataInPlayFab = true;


    public TextMeshProUGUI OccupancyRateText_For_PRA;
    public TextMeshProUGUI OccupancyRateText_For_PRB;
    public TextMeshProUGUI OccupancyRateText_For_PRC;

    public TextMeshProUGUI OccupancyRateText_For_Private_RA;
    public TextMeshProUGUI OccupancyRateText_For_Private_RB;
    public TextMeshProUGUI OccupancyRateText_For_Private_RC;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnectedAndReady) 
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
        PlayFabUtiils.ConnectToPlayFab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region UI Callback Methods
    public void JoinRandomRoom() 
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // Public Rooms
    public void OnEnterButtonClicked_RoomA()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_A;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterButtonClicked_RoomB() 
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterButtonClicked_RoomC()
    {
            mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_C;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    // Private Rooms
    public void OnEnterButtonClicked_Private_RoomA()
    {
        string roomName = "RoomA";
 
        RoomData room = null;

        if (isHardcodedUserNames)
        {
            room = new RoomData(roomName, true);
            room.AddUserToList("Bob");
            room.AddUserToList("Mike");
            room.AddUserToList("Ethan");

            PlayFabUtiils.StoreJsonInPlayFab(room, encryptedDataInPlayFab);

            // Retrieve updated data
            room = PlayFabUtiils.RetrieveJsonFromPlayFab(roomName, encryptedDataInPlayFab);
        }
        else
        {
            room = PlayFabUtiils.RetrieveJsonFromPlayFab(roomName, encryptedDataInPlayFab);

            if (room == null)
            {
                Debug.Log($"Cannot retrieve RoomData from PlayFab : {roomName}");
                return;
            }
        }

        var userName = PhotonNetwork.NickName;
        bool isAllowed = room.IsUserAllowed(userName);
        if (isAllowed)
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_A;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        }
        else
        {
            Debug.Log($"{userName} Is not allowed to enter this Room {room.Roomname}");
        }
    }

    public void OnEnterButtonClicked_Private_RoomB()
    {
        var roomB = new RoomData("RoomB", true);
        roomB.AddUserToList("Bob");
        roomB.AddUserToList("Mike");

        var userName = PhotonNetwork.NickName;
        bool isAllowed = roomB.IsUserAllowed(userName);
        if (isAllowed)
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_B;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        }
        else
        {
            Debug.Log($"{userName} Is not allowed to enter this Room {roomB.Roomname}");
        }
    }

    public void OnEnterButtonClicked_Private_RoomC()
    {
        var roomC = new RoomData("RoomC", true);
        roomC.AddUserToList("Bob");
        roomC.AddUserToList("Mike");

        var userName = PhotonNetwork.NickName;
        bool isAllowed = roomC.IsUserAllowed(userName);
        if (isAllowed)
        {
            mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_C;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        }
        else
        {
            Debug.Log($"{userName} Is not allowed to enter this Room {roomC.Roomname}");
        }

    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server Again.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A room called " + PhotonNetwork.CurrentRoom.Name + "Was created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The Local Player: " + PhotonNetwork.NickName + "Joined: " + PhotonNetwork.CurrentRoom.Name + "Which has: " + PhotonNetwork.CurrentRoom.PlayerCount + "Playes.");

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY)) 
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType)) 
            {
                Debug.Log("Joined room with the map: " + (string)mapType);

                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_A) 
                {
                    //load Public_Room_A scene
                    PhotonNetwork.LoadLevel("Public_Room_A");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B)
                {
                    //load Public_Room_B scene
                    PhotonNetwork.LoadLevel("Public_Room_B");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_C)
                {
                    //load Public_Room_C scene
                    PhotonNetwork.LoadLevel("Public_Room_C");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_A)
                {
                    //load Private_Room_A scene
                    PhotonNetwork.LoadLevel("Private_Room_A");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_B)
                {
                    //load Private_Room_B scene
                    PhotonNetwork.LoadLevel("Private_Room_B");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_PRIVATE_ROOM_C)
                {
                    //load Private_Room_C scene
                    PhotonNetwork.LoadLevel("Private_Room_C");
                }
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName+ "Joined. Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0) 
        {
            //The room doesn't exist
            OccupancyRateText_For_PRA.text = 0 + " / " + 20;
            OccupancyRateText_For_PRB.text = 0 + " / " + 20;
            OccupancyRateText_For_PRC.text = 0 + " / " + 20;
            OccupancyRateText_For_Private_RA.text = 0 + " / " + 20;
            OccupancyRateText_For_Private_RB.text = 0 + " / " + 20;
            OccupancyRateText_For_Private_RC.text = 0 + " / " + 20;
        }

        foreach (RoomInfo room in roomList) 
        {
            Debug.Log(room.Name);

            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_A)) 
            {
                //Update Room A room occupancy field
                Debug.Log("Room is Public Room A. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_PRA.text = room.PlayerCount + " / " + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B)) 
            {
                //Update Room B room occupancy field
                Debug.Log("Room is Public Room B. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_PRB.text = room.PlayerCount + " / " + 20;

            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_C))
            {
                //Update Room C room occupancy field
                Debug.Log("Room is Public Room C. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_PRC.text = room.PlayerCount + " / " + 20;

            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B))
            {
                //Update Private Room A room occupancy field
                Debug.Log("Room is Private Room A. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_Private_RA.text = room.PlayerCount + " / " + 20;

            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_C))
            {
                //Update Private Room B room occupancy field
                Debug.Log("Room is Private Room B. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_Private_RB.text = room.PlayerCount + " / " + 20;

            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_ROOM_B))
            {
                //Update Private Room C room occupancy field
                Debug.Log("Room is Private Room C. Player count is: " + room.PlayerCount);
                OccupancyRateText_For_Private_RC.text = room.PlayerCount + " / " + 20;

            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby Joined");
    }
    #endregion

    #region Private Methods
    public void CreateAndJoinRoom() 
    {
        string randomRoomName = "Room_" + mapType + Random.Range(0, 100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MAP_TYPE_KEY };
        //there are 3 different maps
        //Public Room A & Public Room B & Public Room C

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion
}
