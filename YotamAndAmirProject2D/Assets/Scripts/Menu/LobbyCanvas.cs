using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup RoomLayoutGroup;
    //private RoomLayoutGroup RoomLayoutGroup
    //{
    //    get { return _roomLayoutGroup; }
    //}


    [SerializeField]
    private GameObject toEnable, toDisable;

    public void OnClickJoinRoom(string roomName)
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        RoomInfo thisRoom = null;

        foreach (RoomInfo room in rooms)
        {
            if(room.Name == roomName)
            {
                thisRoom = room;
                break;
            }
        }

        if(thisRoom.PlayerCount < thisRoom.MaxPlayers)
        {
            if (PhotonNetwork.JoinRoom(roomName))
            {
                Debug.Log("Joined Room Successfully!");
            }
            toEnable.SetActive(true);
            toDisable.SetActive(false);
        }
        else
        {
            Debug.Log("Joined Room Failed!");
        }
    }
}
