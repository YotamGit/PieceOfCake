using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyNetwork : MonoBehaviour
{
    //[SerializeField]
    [SerializeField]
    private GameObject disableConnectingScreen, enableMainMenu;

    public TextMeshProUGUI SignInUser, SignInPass; // username
    [SerializeField]
    private TextMeshProUGUI SignInButtonText; // always: "sign in"
    [SerializeField]
    private Button SignInButton;
    [SerializeField]
    private GameObject BackButton;
    [SerializeField]
    private GameObject disableSighIn, enableSingleOrMulti;
    //when you go back to the main menu sign out. if this issue isnt fixed the player can log in infinite times (20 to be exact or less)

    /*private TextMeshProUGUI SighnInText
    {
        get { return _sighnInText; }
    }*/
    [SerializeField]
    private MainMenu mainMenuScript;

    [SerializeField]
    private DBCManager dbManager;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 256; // setting the max FPS
        
        
        //PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnConnectedToMaster()
    {
        if (disableConnectingScreen.activeInHierarchy && !PhotonNetwork.inRoom)
        {
            disableConnectingScreen.SetActive(false);
            enableMainMenu.SetActive(true);
            Debug.Log("Joined Server");
        }
    }

    // false - invalid user/pass
    public bool JoinLobbyAs()
    {
        /*string playerName = SignInUser.text;
        string playerPass = SignInPass.text;*/

        /*if (playerName.Length == 1 && playerPass.Length == 1) // this part is only for debugging
        {
            PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;
        }
        else if (IsValidText(playerName, 4) && IsValidText(playerPass, 7))
        {
            PhotonNetwork.playerName = playerName;
        }
        else
        {
            StartCoroutine(mainMenuScript.DisplayError("Invalid Username or Password\nPlease Try again")); // telling the main menu to display the error message
            return false;
            //PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;
        }*/
        
        //dbManager.Login_LoginButtonPressed(); // sending a login request to connect to the DB server

        PhotonNetwork.JoinLobby(TypedLobby.Default);

        ChangeTextAlpha(true);

        BackButton.SetActive(false);

        return true;
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
        if (disableSighIn.activeInHierarchy && !enableSingleOrMulti.activeInHierarchy) // checking if the toDisable is already disabled (if not, then it will be)
        {
            BackButton.SetActive(true);

            ChangeTextAlpha(false);

            disableSighIn.SetActive(false);
            enableSingleOrMulti.SetActive(true);
        }
        else
        {
            MainMenu.Instance.RoomLayoutGroup.OnReceivedRoomListUpdate(); // getting all the rooms
        }
    }

    public void LeaveLobbyUser()
    {
        PhotonNetwork.LeaveLobby();
    }
}
