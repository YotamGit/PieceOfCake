using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    [SerializeField]
    private TextMesh _roomName;
    private TextMesh RoomName
    {
        get{ return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        if(PhotonNetwork.CreateRoom(RoomName.text))
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
}
