using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyNetwork : MonoBehaviour
{
    [SerializeField]
    private GameObject disableConnectingScreen, enableMainMenu;

    public TextMeshProUGUI SignInUser, SignInPass;
    [SerializeField]
    private TextMeshProUGUI SignInButtonText;
    [SerializeField]
    private Button SignInButton;
    [SerializeField]
    private GameObject BackButton;
    [SerializeField]
    private GameObject disableSighIn, enableSingleOrMulti;

    [SerializeField]
    private MainMenu mainMenuScript;

    [SerializeField]
    private DBCManager dbManager;

    void Start ()
    {
        Application.targetFrameRate = 256; // setting the max FPS
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
        if(PhotonNetwork.JoinLobby(TypedLobby.Default))
        {
            ChangeTextAlpha(true);

            BackButton.SetActive(false);
            return true;
        }
        return false;
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
