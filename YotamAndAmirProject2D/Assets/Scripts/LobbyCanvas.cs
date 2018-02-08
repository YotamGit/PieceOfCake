using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }


    [SerializeField]
    private GameObject toEnable, toDisable;

    public void OnClickJoinRoom(string roomName)
    {
        toEnable.SetActive(true);
        toDisable.SetActive(false);

        if (PhotonNetwork.JoinRoom(roomName))
        {
            Debug.Log("Joined Room Successfully!");
        }
        else
        {
            Debug.Log("Joined Room Failed!");
        }
    }
}
