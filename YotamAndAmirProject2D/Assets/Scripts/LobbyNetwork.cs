using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyNetwork : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _sighnInText;
    private TextMeshProUGUI SighnInText
    {
        get { return _sighnInText; }
    }

    // Use this for initialization
    void Start () {
        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;
        //PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void JoinLobbyAs()
    {
        string playerName = SighnInText.text;
        if (playerName != "" && !playerName.Contains(" "))
        {
            PhotonNetwork.playerName = playerName;
        }
        else
        {
            PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        }
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
