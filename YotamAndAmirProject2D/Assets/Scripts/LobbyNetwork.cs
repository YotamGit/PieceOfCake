using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");

        PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /*void OnConnectedToMaster() // didn't work for some reason...
    {
        Debug.Log("Connected to Master As: " + PhotonNetwork.playerName);
        PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }*/

    private void OnJoinedLobby()
    {
        
        Debug.Log("Joined Lobby As: " + PhotonNetwork.player.NickName);
    }
}
