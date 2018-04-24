using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateRoom : MonoBehaviour {

    [SerializeField]
    private GameObject toEnable, toDisable, backButton;

    [SerializeField]
    private TextMeshProUGUI RoomName, changeAlpha, cancelText;

    [SerializeField]
    private MainMenu mainMenuScript;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private bool RoomNameCheck(string roomName)
    {
        if(roomName.Length < 4)
        {
            return false;
        }
        return true;
    }

    public void OnClick_CreateRoom()
    {
        if (!RoomNameCheck(RoomName.text.ToUpper()))
        {
            StartCoroutine(mainMenuScript.DisplayError("Room Name Is Invalid"));
            return;
        }

        DisableSelf();//disabling create room button

        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        if (PhotonNetwork.CreateRoom(RoomName.text.ToUpper(), roomOptions, TypedLobby.Default))
        {
            Debug.Log("Create Room Successfully Sent");
        }
    }

    // enableing the create room (and back) button, and telling the player that this room name is taken 
    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        if (codeAndMessage[1].ToString() == "A game with the specified id already exist.")
        {
            button.enabled = true;

            backButton.SetActive(true);

            changeAlpha.faceColor = new Color(1, 1, 1, 1F);
            StartCoroutine(mainMenuScript.DisplayError("Room Already Exists"));
        }
        else
        {
            Debug.Log("Create Room Failed: " + codeAndMessage[1]);
        }
    }
    
    private void DisableSelf()//disabling the join button and the back button
    {
        changeAlpha.faceColor = new Color(1, 1, 1, 0.4F);
        button.enabled = false;
        backButton.SetActive(false);//diactivating the backbutton
    }

    private void OnCreatedRoom()
    {
        button.enabled = true;

        changeAlpha.faceColor = new Color(1, 1, 1, 1F);
        cancelText.faceColor = new Color(1, 1, 1, 1F);

        Debug.Log("Room Created successfully");
        toDisable.SetActive(false);
        toEnable.SetActive(true);
    }
}
