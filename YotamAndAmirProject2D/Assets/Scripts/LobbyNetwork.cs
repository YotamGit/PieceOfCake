using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyNetwork : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI SignInText; // username
    [SerializeField]
    private TextMeshProUGUI SignInButtonText; // always: "sign in"
    [SerializeField]
    private Button SignInButton;
    [SerializeField]
    private Button BackButton;
    [SerializeField]
    private GameObject toDisable;
    [SerializeField]
    private GameObject toEnable;
    //when you go back to the main menu sign out. if this issue isnt fixed the player can log in infinite times (20 to be exact or less)

    /*private TextMeshProUGUI SighnInText
    {
        get { return _sighnInText; }
    }*/

    // Use this for initialization
    void Start () {
        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;
        //PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnConnectedToServer()
    {
        Debug.Log("Joined Server");
    }

    public void JoinLobbyAs()
    {
        string playerName = SignInText.text;
        if (playerName != "" && !playerName.Contains(" "))
        {
            PhotonNetwork.playerName = playerName;
        }
        else
        {
            PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;
        }
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        ChangeTextAlpha(true);

        BackButton.enabled = false;
    }

    public void ChangeTextAlpha(bool change) // true: pale, false: normal
    {
        if(change)
        {
            SignInButtonText.faceColor = new Color(1, 1, 1, 0.4F);
        }
        else
        {
            SignInButtonText.faceColor = new Color(1, 1, 1, 1F);
        }
        SignInButton.interactable = !SignInButton.interactable;
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

        BackButton.enabled = true;

        ChangeTextAlpha(false);

        toDisable.SetActive(false);
        toEnable.SetActive(true);
    }

    public void LeaveLobbyUser()
    {
        PhotonNetwork.LeaveLobby();
    }
}
