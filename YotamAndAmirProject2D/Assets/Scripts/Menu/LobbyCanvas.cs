using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup RoomLayoutGroup;
    //private RoomLayoutGroup RoomLayoutGroup
    //{
    //    get { return _roomLayoutGroup; }
    //}


    [SerializeField]
    private GameObject toEnable, toDisable;

    [SerializeField]
    private MainMenu mainMenuScript;

    public void OnClickJoinRoom(string roomName, GameObject sender)
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
            Destroy(sender);
            StartCoroutine(mainMenuScript.DisplayError("Room Already Full")); // telling the main menu to display the error message
        }
    }
}
