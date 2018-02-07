using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class CreateRoom : MonoBehaviour {

    [SerializeField]
    private GameObject toEnable;
    [SerializeField]
    private GameObject toDisable;

    [SerializeField]
    private TextMeshProUGUI _roomName;
    private TextMeshProUGUI RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

        if(PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            Debug.Log("Create Room Successfully Sent");
        }
        else
        {
            Debug.Log("Create Room Failed To Sent");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.Log("Create Room Failed: " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        Debug.Log("Room Created successfully");
        toDisable.SetActive(false);
        toEnable.SetActive(true);
    }
}
