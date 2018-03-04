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

    public TextMeshProUGUI SignInText; // username
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
    private GameObject errorMessage;
    [SerializeField]
    private TextMeshProUGUI errorText;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 256; // setting the max FPS
        if (!PhotonNetwork.connected) /*DDOL will initiate when you enter the menu scene, thus you shouldnt return to in. so just return to a diff simulare menu*/
        {
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings("game");
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.autoJoinLobby = false;
        }
        
        //PhotonNetwork.playerName = PlayerNetwork.instence.PlayerName;
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnConnectedToMaster()
    {
        if (disableConnectingScreen.activeInHierarchy)
        {
            disableConnectingScreen.SetActive(false);
            enableMainMenu.SetActive(true);
            Debug.Log("Joined Server");
        }
    }

    //username: min 3 letters. doesn't contain cerian charecters
    private bool IsValidUsername(string user)
    {
        if (user.Length > 3 && !user.Contains(" ") && !user.Contains("*") && !user.Contains("."))
        {
            return true;
        }
        return false;
    }

    public void JoinLobbyAs()
    {
        string playerName = SignInText.text;

        if (playerName.Length == 1) // this part is only for debugging
        {
            PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;
        }
        else if(IsValidUsername(playerName))
        {
            PhotonNetwork.playerName = playerName;
        }
        else
        {
            StartCoroutine(DisplayError("Invalid Username\nPlease Try again"));
            return;
            //PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;
        }

        PhotonNetwork.JoinLobby(TypedLobby.Default);

        ChangeTextAlpha(true);

        BackButton.SetActive(false);
    }

    // changing the error message and displaying it to the user
    private IEnumerator DisplayError(string error)
    {
        errorText.text = error;
        errorMessage.SetActive(true);
        
        yield return new WaitForSeconds(2f); // waiting 2 seconds
        errorMessage.SetActive(false);
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
