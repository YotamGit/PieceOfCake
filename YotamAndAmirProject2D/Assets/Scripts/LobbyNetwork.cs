using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");
    }

    private void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
}
